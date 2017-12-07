using System;
using System.IO;
using System.Windows;
using GameEditor.Models;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace GameEditor.Services
{
    internal class WorldEditorService : IWorldEditorService
    {
        public WorldEditorService() { }
        
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
    }
}
