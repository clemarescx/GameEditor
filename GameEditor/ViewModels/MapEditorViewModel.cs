using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GameEditor.Models;
using GameEditor.Properties;
using GameEditor.Services;
using Microsoft.Win32;
using Newtonsoft.Json;
//using GalaSoft.MvvmLight.Command; 
// ^^^^  using this piece of ***** instead of CommandWpf below 
// cost me 4 hours in debugging !! 
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GameEditor.Messages;


namespace GameEditor.ViewModels
{
    public class MapEditorViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public const string TerrainTilesPropertyName = "TerrainTiles";
        public const string LogicTilesPropertyName = "LogicTiles";
        public const string BrushTilePropertyName = "BrushTile";
        public const string MapPropertyName = "AreaMap";

        private readonly IMapEditorService _mapEditorService;


        private AreaMap _areaMap;
        private string _brushTile;
        private Dictionary<string, BitmapImage> _logicTiles;
        private string _mapName;

        private Dictionary<string, BitmapImage> _terrainSprites;
        public ObservableCollection<string> TerrainSpriteNames{ get; set; }

        public string MapName
        {
            get => _mapName;
            set
            {
                _mapName = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand BtnPrintMapCommand{ get; }
        public RelayCommand BtnImportMapCommand{ get; }
        public RelayCommand BtnExportMapCommand{ get; }
        public RelayCommand BtnClearMapCommand{ get; }
        public RelayCommand BtnDebugCommand{ get; }
        public RelayCommand BtnSaveMapCommand { get; }


        public AreaMap AreaMap
        {
            get => _areaMap;
            set
            {
                if(_areaMap == value)

                    return;

                _areaMap = value;
                OnPropertyChanged();
            }
        }

        public string BrushTile
        {
            get => _brushTile;
            set
            {
                _brushTile = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<string, BitmapImage> TerrainTiles
        {
            get => _terrainSprites;
            set
            {
                if(_terrainSprites == value) return;

                _terrainSprites = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<string, BitmapImage> LogicTiles
        {
            get => _logicTiles;
            set
            {
                if(_logicTiles == value) return;

                _logicTiles = value;
                OnPropertyChanged();
            }
        }


        public MapEditorViewModel(IMapEditorService service)
        {
            //            _mapEditorService = new MapEditorService();
            _mapEditorService = service;
            Messenger.Default.Register<MapSelectedMessage>(this, msg => AreaMap = msg.SelectedMap);
            
            service.GetTerrainTiles(
                (sprites, error) => {
                    if(error != null)
                        MessageBox.Show(error.Message);
                    else
                    {
                        TerrainTiles = new Dictionary<string, BitmapImage>(sprites);
                        TerrainSpriteNames = new ObservableCollection<string>(TerrainTiles.Keys);
                    }
                });
            service.GetLogicTiles(
                (sprites, error) => {
                    if(error != null)
                        MessageBox.Show(error.Message);
                    else
                        LogicTiles = new Dictionary<string, BitmapImage>(sprites);
                });

            BtnPrintMapCommand = new RelayCommand(PrintMap, () => AreaMap != null);
            BtnImportMapCommand = new RelayCommand(ImportMap);
            BtnExportMapCommand = new RelayCommand(ExportMap, () => AreaMap != null);
            BtnClearMapCommand = new RelayCommand(ClearMap);
            BtnDebugCommand = new RelayCommand(() => AreaMap = new AreaMap(8, "CreatedfromDebug"));
            BtnSaveMapCommand = new RelayCommand(SaveMap);
        }

        private void SaveMap()
        {
            Messenger.Default.Send(new SaveMapMessage(AreaMap));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Console.WriteLine($"changed {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PrintMap()
        {
            Console.WriteLine($@"Content of map '{AreaMap.Name}':");
            Console.WriteLine(@"Done.");
            for(var i = 0; i < AreaMap.Rows; i++)
            {
                Console.Write("\t");
                for(var j = 0; j < AreaMap.Columns; j++) Console.Write($@"{AreaMap.Grid[ i, j ]}, ");

                Console.WriteLine();
            }

            Console.WriteLine(@"Done.");
        }

        // Load from JSON
        private void ImportMap()
        {
            var openFileDialog = new OpenFileDialog{ InitialDirectory = Directory.GetCurrentDirectory() };

            if(openFileDialog.ShowDialog() != true)
                return;

            try
            {
                var jsonZone = File.ReadAllText(openFileDialog.FileName);
                AreaMap = JsonConvert.DeserializeObject<AreaMap>(jsonZone);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: \n" + ex.Message);
                throw;
            }
        }

        // Save to JSON
        private void ExportMap()
        {
            var jsonConvertZone = JsonConvert.SerializeObject(AreaMap);

            var filename = string.Empty.Equals(AreaMap.Name) || null == AreaMap.Name ? "newMap" : AreaMap.Name;
            filename += ".json";

            var saveFileDialog = new SaveFileDialog{
                FileName = filename,
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "JSON file (*.json)|*.json"
            };
            AreaMap.Name = saveFileDialog.SafeFileName;

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

        private void ClearMap()
        {
            AreaMap.Fill(null);
        }


        public new event PropertyChangedEventHandler PropertyChanged;
    }
}
