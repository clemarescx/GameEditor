using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GameEditor.Messages;
using GameEditor.Models;
using GameEditor.Properties;
using GameEditor.Services;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace GameEditor.ViewModels
{
    public class WorldEditorViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private int mapNamingCounter = 1;
        private readonly IWorldEditorService _worldEditorService;
        private ObservableCollection<AreaMap> _areaMaps;
        private RelayCommand _btnAddMapCommand;
        private RelayCommand _btnCreateWorldCommand;

        private RelayCommand _btnLoadWorldCommand;
        private RelayCommand _btnPrintWorldCommand;
        private RelayCommand _btnRemoveMapCommand;
        private RelayCommand _btnSaveWorldCommand;
        private RelayCommand _btnWorldDebugCommand;
        private WorldMap _worldMap;
        private AreaMap _selectedMap;

        public ObservableCollection<AreaMap> AreaMaps
        {
            get => _areaMaps;
            set
            {
                _areaMaps = value;
                OnPropertyChanged();
            }
        }

        public WorldMap WorldMap
        {
            get => _worldMap;
            set
            {
                _worldMap = value;
                OnPropertyChanged();
            }
        }

        public AreaMap SelectedMap {
            get => _selectedMap;
            set {
                _selectedMap = value;
                Messenger.Default.Send(new MapSelectedMessage(_selectedMap));
                OnPropertyChanged();
            }
        }

        public RelayCommand BtnLoadWorldCommand
        {
            get { return _btnLoadWorldCommand ?? ( _btnLoadWorldCommand = new RelayCommand(LoadWorld) ); }
        }

        public RelayCommand BtnSaveWorldCommand
        {
            get
            {
                return _btnSaveWorldCommand
                       ?? ( _btnSaveWorldCommand = new RelayCommand(SaveWorld, () => WorldMap != null) );
            }
        }
        public RelayCommand BtnAddMapCommand
        {
            get
            {
                return _btnAddMapCommand
                       ?? ( _btnAddMapCommand = new RelayCommand(
                                () => {
                                    var newmapname = $"newMap{mapNamingCounter++}";
                                    Console.WriteLine(@"Creating " + newmapname);
                                    AreaMaps.Add(new AreaMap(8, newmapname));
                                },
                                () => WorldMap != null) );
            }
        }

        public RelayCommand BtnWorldDebugCommand
        {
            get
            {
                return _btnWorldDebugCommand
                       ?? ( _btnWorldDebugCommand = new RelayCommand(
                                () => {
                                    Console.WriteLine("Change world name to 'potatoes'");
                                    WorldMap.Name = "World potatoes";
                                },
                                () => WorldMap != null) );
            }
        }

        public RelayCommand BtnRemoveMapCommand
        {
            get
            {
                return _btnRemoveMapCommand
                       ?? ( _btnRemoveMapCommand = new RelayCommand(
                                () => AreaMaps.Remove(SelectedMap),
                                () => WorldMap != null) );
            }
        }

        public RelayCommand BtnCreateWorldCommand
        {
            get { return _btnCreateWorldCommand ?? ( _btnCreateWorldCommand = new RelayCommand(CreateWorld) ); }
        }

        public RelayCommand BtnWorldPrintCommand
        {
            get
            {
                return _btnPrintWorldCommand
                       ?? ( _btnPrintWorldCommand = new RelayCommand(PrintWorld, () => WorldMap != null) );
            }
        }

        public WorldEditorViewModel(IWorldEditorService service)
        {
            _worldEditorService = service;
            AreaMaps = new ObservableCollection<AreaMap>();
            Messenger.Default.Register<SaveMapMessage>(this,
                msg => {
                    if(AreaMaps.Contains(SelectedMap))
                    {
                        Console.WriteLine("Removing '{0}'", SelectedMap.Name);
                        AreaMaps.Remove(SelectedMap);
                        AreaMaps.Add(msg.SavedMap);
                    }
                });
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        private void CreateWorld()
        {
            if(WorldMap != null)
            {
                var res = MessageBox.Show(
                    "Would you like to save the current world?",
                    "Load world",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);
                switch(res)
                {
                    case MessageBoxResult.Yes:
                        SaveWorld();
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        return;
                }
            }

            WorldMap = new WorldMap("newWorld");
            AreaMaps.Clear();
            AreaMaps.Add(new AreaMap(8, "Start_Area"));
            PrintWorld();
        }
        
        

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Console.WriteLine($"changed {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PrintWorld()
        {
            Console.WriteLine($@"Content of world '{WorldMap.Name}':");
            foreach(var m in WorldMap.Areas)
                Console.WriteLine($@"	{m.Name}");

            Console.WriteLine($@"Content of AreaMaps:");
            foreach(var m in AreaMaps)
                Console.WriteLine($@"	{m.Name}");
        }

        private void dLoadWorld()
        {
            if(WorldMap != null)
            {
                var res = MessageBox.Show(
                    "Would you like to save the current world?",
                    "Load world",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);
                switch(res)
                {
                    case MessageBoxResult.Yes:
                        SaveWorld();
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        return;
                }
            }

            var openFileDialog = new OpenFileDialog{ InitialDirectory = Directory.GetCurrentDirectory() };

            if(openFileDialog.ShowDialog() != true)
                return;

            try
            {
                var jsonZone = File.ReadAllText(openFileDialog.FileName);
                WorldMap = JsonConvert.DeserializeObject<WorldMap>(jsonZone);
                AreaMaps = new ObservableCollection<AreaMap>(WorldMap.Areas);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: \n" + ex.Message);
                throw;
            }
        }

        // "Proof of concept" to use the Service class
        private void LoadWorld()
        {
            if(WorldMap != null)
            {
                var res = MessageBox.Show(
                    "Would you like to save the current world?",
                    "Load world",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);
                switch(res)
                {
                    case MessageBoxResult.Yes:
                        SaveWorld();
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        return;
                }
            }

            _worldEditorService.LoadWorld(
                (world, error) => {
                    if(error != null)
                        MessageBox.Show("Could not load world from service: " + error.Message);
                    else
                    {
                        if(world != null)
                        {
                            Console.WriteLine("Loaded!");
                            WorldMap = world;
                            AreaMaps = new ObservableCollection<AreaMap>(WorldMap.Areas);
                            PrintWorld();
                        }
                    }
                });
        }

        // Save to JSON
        private void SaveWorld()
        {
            WorldMap.Areas = AreaMaps.ToList();
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
                    Console.WriteLine("Saved!");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: \n" + ex.Message);
                }
            }
            PrintWorld();
        }
    }
}
