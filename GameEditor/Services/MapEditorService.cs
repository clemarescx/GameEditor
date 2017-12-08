using System;
using System.IO;
using System.Windows;
using GameEditor.Models;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace GameEditor.Services
{
    public class MapEditorService : IMapEditorService
    {
        /// <summary>
        ///     Parse Map from JSON.
        ///     Returns loaded Map via callback.
        /// </summary>
        /// <param name="callback"></param>
        public void LoadMap(Action<Map, Exception> callback)
        {
            var openFileDialog = new OpenFileDialog{ InitialDirectory = Directory.GetCurrentDirectory() };

            if(openFileDialog.ShowDialog() != true)
                return;

            Map map = null;
            Exception error = null;
            try
            {
                var mapJson = File.ReadAllText(openFileDialog.FileName);
                map = JsonConvert.DeserializeObject<Map>(mapJson);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: \n" + ex.Message);
                error = ex;
            }
            callback(map, error);
        }

        /// <summary>
        ///     Serialise Map to JSON
        /// </summary>
        /// <param name="map"></param>
        public void SaveMap(Map map)
        {
            var mapJson = JsonConvert.SerializeObject(map);

            var filename = string.Empty.Equals(map.Name) || null == map.Name ? "newMap" : map.Name;
            filename += ".json";

            var saveFileDialog = new SaveFileDialog{
                FileName = filename,
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "JSON file (*.json)|*.json"
            };
            map.Name = saveFileDialog.SafeFileName;

            if(saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, mapJson);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: \n" + ex.Message);
                }
            }
        }
    }
}
