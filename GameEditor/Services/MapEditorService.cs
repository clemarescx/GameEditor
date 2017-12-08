using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameEditor.Models;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace GameEditor.Services
{
    /// <summary>
    ///     Loads the tile graphics per category (folder) and stores them in distinct
    ///     collections as Tile objects.
    /// </summary>
    public class MapEditorService : IMapEditorService
    {
        /// <summary>
        /// Parse an AreaMap from JSON.
        /// Returns loaded map via callback.
        /// </summary>
        /// <param name="callback"></param>
        public void LoadAreaMap(Action<AreaMap, Exception> callback)
        {
            var openFileDialog = new OpenFileDialog{ InitialDirectory = Directory.GetCurrentDirectory() };

            if(openFileDialog.ShowDialog() != true)
                return;

            AreaMap map = null;
            Exception error = null;
            try
            {
                var mapJson = File.ReadAllText(openFileDialog.FileName);
                map = JsonConvert.DeserializeObject<AreaMap>(mapJson);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: \n" + ex.Message);
                error = ex;
            }
            callback(map, error);
        }

        /// <summary>
        ///     Serialise an AreaMap to JSON
        /// </summary>
        /// <param name="map"></param>
        public void SaveAreaMap(AreaMap map)
        {
            var jsonConvertZone = JsonConvert.SerializeObject(map);

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
                    File.WriteAllText(saveFileDialog.FileName, jsonConvertZone);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: \n" + ex.Message);
                }
            }
        }
    }
}
