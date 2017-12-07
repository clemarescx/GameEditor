using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GameEditor.Messages;
using GameEditor.Models;
using GameEditor.Properties;
using GameEditor.Services;

namespace GameEditor.ViewModels
{
    public class WorldEditorViewModel : ViewModelBase, INotifyPropertyChanged
    {
        // This is for the MVVMlight framework, not really necessary as we don't use the
        // "design" implementation 
        private readonly IWorldEditorService _worldEditorService;

        private ObservableCollection<AreaMap> _areaMaps;
        private RelayCommand _btnAddMapCommand;
        private RelayCommand _btnCreateWorldCommand;
        private RelayCommand _btnLoadWorldCommand;
        private RelayCommand _btnPrintWorldCommand;
        private RelayCommand _btnRemoveMapCommand;
        private RelayCommand _btnSaveWorldCommand;

        private int _mapNamingCounter = 1;
        private AreaMap _selectedMap;
        private WorldMap _worldMap;

        // All member data with OnPropertyChanged()
        // are watched for updates by the XAML view
        public ObservableCollection<AreaMap> AreaMaps
        {
            get => _areaMaps;
            set
            {
                _areaMaps = value;
                OnPropertyChanged();
                Messenger.Default.Send(new AreamapsAvailableMessage(new List<AreaMap>(_areaMaps)));
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

        /// <summary>
        ///     The map currently selected in the listbox.
        ///     Sent to the Map Editor as soon as it is selected
        /// </summary>
        public AreaMap SelectedMap
        {
            get => _selectedMap;
            set
            {
                _selectedMap = value;
                Messenger.Default.Send(new MapSelectedMessage(_selectedMap));
                OnPropertyChanged();
            }
        }

        public RelayCommand BtnLoadWorldCommand =>
            _btnLoadWorldCommand ?? ( _btnLoadWorldCommand = new RelayCommand(LoadWorld) );

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
                                    var newmapname = $"newMap{_mapNamingCounter++}";
                                    AreaMaps.Add(new AreaMap(newmapname));
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

        public RelayCommand BtnCreateWorldCommand =>
            _btnCreateWorldCommand ?? ( _btnCreateWorldCommand = new RelayCommand(CreateWorld) );

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

            // inter-viewModel messaging system 
            // with MVVMlight ;)
            Messenger.Default.Register<SaveMapMessage>(
                this,
                msg => {
                    if(AreaMaps.Contains(SelectedMap))
                    {
                        Console.WriteLine("Removing '{0}'", SelectedMap.Name);
                        AreaMaps.Remove(SelectedMap);
                        AreaMaps.Add(msg.SavedMap);
                        SelectedMap = msg.SavedMap;
                    }
                });
        }

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
            AreaMaps.Add(new AreaMap("Start_Area"));
            PrintWorld();
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
            _worldEditorService.SaveWorld(WorldMap);
            PrintWorld();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Console.WriteLine($"changed {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public new event PropertyChangedEventHandler PropertyChanged;
    }
}
