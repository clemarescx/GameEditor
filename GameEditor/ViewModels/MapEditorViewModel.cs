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
    public class MapEditorViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public const string TerrainTilesPropertyName = "TerrainTiles";
        public const string LogicTilesPropertyName = "LogicTiles";
        public const string BrushTilePropertyName = "BrushTile";
        public const string MapPropertyName = "Map";

        private static MapEditorService _mapEditorService;
        private Tile _brushTile;
        private Dictionary<string, BitmapImage> _logicTiles;


        private Map _map;
        private string _mapName;

        private Dictionary<string, BitmapImage> _terrainSprites;
        //        public ObservableCollection<TerrainTile> TerrainTiles { get; set; }
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
        public RelayCommand BtnLoadMapCommand{ get; }
        public RelayCommand BtnSaveMapCommand{ get; }
        public RelayCommand BtnClearMapCommand{ get; }
        public RelayCommand BtnDebugCommand{ get; }


        public Map Map
        {
            get => _map;
            set
            {
                _map = value;
                OnPropertyChanged();
            }
        }

        public Tile BrushTile
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
                //                RaisePropertyChanged(TerrainTilesPropertyName);
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
                //                RaisePropertyChanged(LogicTilesPropertyName);
                OnPropertyChanged();
            }
        }


        public MapEditorViewModel(IMapEditorService service)
        {
            _mapEditorService = new MapEditorService();

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

            Map = new Map(8, "newMap");
            MapName = Map.Name;
            var defaultTile = new Tile();
            defaultTile.SpriteName = "sand_1.png";
            Map.Fill(defaultTile);
            BtnPrintMapCommand = new RelayCommand(PrintMap, () => Map != null);
            BtnLoadMapCommand = new RelayCommand(LoadMap);
            BtnSaveMapCommand = new RelayCommand(SaveMap, () => Map != null);
            BtnClearMapCommand = new RelayCommand(ClearMap);
            BtnDebugCommand = new RelayCommand(() => Map.Name = "potatoes");
        }


        //        public void RaisePropertyChanged(string propertyName)
        //        {
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Console.WriteLine($"changed {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PrintMap()
        {
            Console.WriteLine($@"Content of map '{Map.Name}':");
            Console.WriteLine(@"Done.");
            for(var i = 0; i < Map.Rows; i++)
            {
                Console.Write("\t");
                for(var j = 0; j < Map.Columns; j++) Console.Write($@"{Map.Grid[ i, j ]}, ");

                Console.WriteLine();
            }

            Console.WriteLine(@"Done.");
        }

        // Load from JSON
        private void LoadMap()
        {
            var openFileDialog = new OpenFileDialog{ InitialDirectory = Directory.GetCurrentDirectory() };

            if(openFileDialog.ShowDialog() != true)
                return;

            try
            {
                var jsonZone = File.ReadAllText(openFileDialog.FileName);
                Map = JsonConvert.DeserializeObject<Map>(jsonZone);

                //TxtMapName.Text = Map.SpriteName;
                //DrawMap();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: \n" + ex.Message);
                throw;
            }
        }

        // Save to JSON
        private void SaveMap()
        {
            var jsonConvertZone = JsonConvert.SerializeObject(Map);

            var filename = string.Empty.Equals(Map.Name) || null == Map.Name ? "newMap" : Map.Name;
            filename += ".json";

            var saveFileDialog = new SaveFileDialog{
                FileName = filename,
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "JSON file (*.json)|*.json"
            };
            Map.Name = saveFileDialog.SafeFileName;

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
            Map.Fill(null);
        }


        public new event PropertyChangedEventHandler PropertyChanged;
    }
}
