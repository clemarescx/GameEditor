using System;
using System.IO;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using GameEditor.Messages;
using GameEditor.Models;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace GameEditor.Services
{
    internal class WorldEditorService : IWorldEditorService
    {
        public WorldEditorService() { Messenger.Default.Register<SaveWorldMessage>(this,
            msg => {
                SaveWorld(msg.SavedWorldMap);
            });}
        
        // Load World from JSON
        public void LoadWorld(Action<WorldMap, Exception> callback)
        {
            var openFileDialog = new OpenFileDialog{ InitialDirectory = Directory.GetCurrentDirectory() };

            if(openFileDialog.ShowDialog() != true)
                return;

            WorldMap worldMap = null;
            Exception e = null;
            try
            {
                var jsonZone = File.ReadAllText(openFileDialog.FileName);
                worldMap = JsonConvert.DeserializeObject<WorldMap>(jsonZone);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: \n" + ex.Message);
                e = ex;
            }

            callback(worldMap, e);
        }

        public void SaveWorld(WorldMap worldMap)
        {
            var jsonConvertZone = JsonConvert.SerializeObject(worldMap);

            var filename = string.Empty.Equals(worldMap.Name) || null == worldMap.Name ? "newWorld" : worldMap.Name;
            filename += ".json";

            var saveFileDialog = new SaveFileDialog
            {
                FileName = filename,
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "JSON file (*.json)|*.json"
            };
            worldMap.Name = saveFileDialog.SafeFileName;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, jsonConvertZone);
                    Console.WriteLine("Saved!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: \n" + ex.Message);
                }
            }

        }
    }
}
