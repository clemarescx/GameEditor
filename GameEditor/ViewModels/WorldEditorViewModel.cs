using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GameEditor.Models;
using GameEditor.Properties;
using GameEditor.Services;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace GameEditor.ViewModels
{
    public class WorldEditorViewModel : ViewModelBase, INotifyPropertyChanged
    {
        
        private static WorldEditorService _worldEditorService;
        private WorldMap _worldMap;
        private ObservableCollection<Map> _maps;

        public RelayCommand BtnLoadWorldCommand{ get; }
        public RelayCommand BtnSaveWorldCommand{ get; }
        public RelayCommand BtnDebugCommand{ get; }


        public WorldMap WorldMap
        {
            get => _worldMap;
            set
            {
                _worldMap = value;
                OnPropertyChanged();
            }
        }

        public WorldEditorViewModel(IWorldEditorService service)
        {
            _worldEditorService = new WorldEditorService();
            
            BtnLoadWorldCommand = new RelayCommand(LoadWorld);
            BtnSaveWorldCommand = new RelayCommand(SaveWorld, () => WorldMap != null);
            BtnDebugCommand = new RelayCommand(() => WorldMap.Name = "World potatoes");
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Console.WriteLine($"changed {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PrintMap()
        {
            Console.WriteLine($@"Content of world '{WorldMap.Name}':");

            foreach(var m in WorldMap.Maps)
            {
                    Console.WriteLine($"\t{m.Name}");
            }

            Console.WriteLine(@"Done.");
        }

        // Load from JSON
        private void LoadWorld()
        {
            var openFileDialog = new OpenFileDialog{ InitialDirectory = Directory.GetCurrentDirectory() };

            if(openFileDialog.ShowDialog() != true)
                return;

            try
            {
                var jsonZone = File.ReadAllText(openFileDialog.FileName);
                WorldMap = JsonConvert.DeserializeObject<WorldMap>(jsonZone);

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: \n" + ex.Message);
                throw;
            }
        }

        // Save to JSON
        private void SaveWorld()
        {
            var jsonConvertZone = JsonConvert.SerializeObject(WorldMap);

            var filename = string.Empty.Equals(WorldMap.Name) || null == WorldMap.Name ? "newWorld" : WorldMap.Name;
            filename += ".json";

            var saveFileDialog = new SaveFileDialog{
                FileName = filename,
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "JSON file (*.json)|*.json"
            };
            WorldMap.Name = saveFileDialog.SafeFileName;

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


        public new event PropertyChangedEventHandler PropertyChanged;
    }
}
