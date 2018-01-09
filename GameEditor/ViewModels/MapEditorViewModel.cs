using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GameEditor.Messages;
using GameEditor.Models;
using GameEditor.Properties;
using GameEditor.Services;

//using GalaSoft.MvvmLight.Command; 
// ^^^^  using this piece of ***** instead of CommandWpf 
// cost me 4 hours in debugging!


namespace GameEditor.ViewModels
{
    public class MapEditorViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly IMapEditorService _mapEditorService;
        private string _brushTile;
        private ObservableCollection<Character> _characters;


        private Map _map;
        private ObservableCollection<Map> _otherMaps;
        private Tile _selectedTile;
        private ObservableCollection<ObservableCollection<Tile>> _tileGrid =
            new ObservableCollection<ObservableCollection<Tile>>();
        private bool campaignLoaded;

        public ObservableCollection<string> TerrainSpriteNames{ get; set; }

        //////
        /// Commands, initialised in the constructor
        public RelayCommand BtnPrintMapCommand{ get; }
        public RelayCommand BtnImportMapCommand{ get; }
        public RelayCommand BtnExportMapCommand{ get; }
        public RelayCommand BtnClearMapCommand{ get; }
        public RelayCommand BtnDebugCommand{ get; }
        public RelayCommand BtnSaveMapCommand{ get; }
        public RelayCommand RbtnPaintModeCommand{ get; }
        public RelayCommand RbtnSelectModeCommand{ get; }


        public Map Map
        {
            get => _map;
            set
            {
                if(_map == value)

                    return;

                _map = value;
                if(_map != null)
                    MapToBindableGrid();
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Bound to the editor grid
        /// </summary>
        public ObservableCollection<ObservableCollection<Tile>> TileGrid
        {
            get => _tileGrid;
            set
            {
                _tileGrid = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        ///     The name of the sprite selected in the sprite selector
        /// </summary>
        public string BrushTile
        {
            get => _brushTile;
            set
            {
                _brushTile = value;
                OnPropertyChanged();
                Console.WriteLine($"Sprite: {_brushTile}");
            }
        }

        /// <summary>
        ///     TODO: updates with the tile selected in the editor grid
        /// </summary>
        public Tile SelectedTile
        {
            get => _selectedTile;
            set
            {
                _selectedTile = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Bound to "Portal cell" dropdown in "Tile properties" box
        /// </summary>
        public ObservableCollection<Map> OtherMaps
        {
            get => _otherMaps;
            set
            {
                _otherMaps = value;
                // TODO: fix OtherMaps also showing current map in Destination tile dropdown box 
                _otherMaps.Remove(_map);
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Character> Characters
        {
            get => _characters;
            set
            {
                _characters = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     ViewModel for the MapEditor view. Contains commands and functionality
        ///     that would otherwise be in the code-behind.
        ///     The constructor registers to messages fired by other viewModels, as well
        ///     initialise the commands.
        /// </summary>
        /// <param name="service"></param>
        public MapEditorViewModel(IMapEditorService service)
        {
            _mapEditorService = service;

            // When selecting a map in the CampaignEditor view, it is sent to this view
            // as the working Map.
            Messenger.Default.Register<MapSelectedMessage>(this, msg => { Map = msg.SelectedMap; });

            Messenger.Default.Register<AvailableMapsMessage>(
                this,
                msg => {
                    OtherMaps = new ObservableCollection<Map>(msg.AllMaps);
                    campaignLoaded = true;
                });

            // For the list of sprites used in the sprite selector
            TerrainSpriteNames = new ObservableCollection<string>(SpriteLoader.TerrainSprites.Keys);

            BtnPrintMapCommand = new RelayCommand(PrintMap, () => Map != null);
            BtnImportMapCommand = new RelayCommand(ImportMap);
            BtnExportMapCommand = new RelayCommand(ExportMap, () => Map != null);
            BtnClearMapCommand = new RelayCommand(ClearMap);
            BtnDebugCommand = new RelayCommand(DebugFunction);
            BtnSaveMapCommand = new RelayCommand(SaveMap, ()=> campaignLoaded);
            RbtnPaintModeCommand = new RelayCommand(() => { Console.WriteLine("Tile painting not implemented"); });
            RbtnSelectModeCommand = new RelayCommand(() => { Console.WriteLine("Tile selection not implemented"); });
            Map = new Map("");
        }

        private void DebugFunction()
        {
            var debugMap = new Map("CreatedfromDebug");
            for(var i = 0; i < debugMap.Rows; i++)
            {
                for(var j = 0; j < debugMap.Columns; j++)
                {
                    debugMap.Grid[ i, j ] = new Tile{ SpriteName = "sand_1.png" };
                    if(i == j)
                    {
                        debugMap.Grid[ i, j ].SpriteName = "plateau_left.png";
                        debugMap.Grid[ i, j ].IsWalkable = true;
                        debugMap.Grid[ i, j ].IsSpawnPoint = true;
                        debugMap.Grid[ i, j ].IsTransitionSpot = true;
                    }
                }
            }

            
            
            Map = debugMap;
        }

        /// <summary>
        ///     Transform the Tile[,] to be bindable.
        ///     The idea to bind the data to a list of horizontal StackPanels
        ///     was taken from:
        ///     http://www.thinkbottomup.com.au/site/blog/Game_of_Life_in_XAML_WPF_using_embedded_Python
        /// </summary>
        private void MapToBindableGrid()
        {
            Console.WriteLine("Map to grid");
            TileGrid.Clear();
            for(var i = 0; i < Map.Grid.GetLength(0); i++)
            {
                TileGrid.Add(new ObservableCollection<Tile>());

                for(var j = 0; j < Map.Grid.GetLength(1); j++)
                    TileGrid[ i ].Add(Map.Grid[ i, j ]);
            }
        }

        /// <summary>
        ///     Return the current map state of the editor to a 2D array of Tiles
        /// </summary>
        /// <param name="flattenedAreaMap"></param>
        /// <returns>The updated grid in its DTO form</returns>
        private Tile[,] BindableGridToMap(ObservableCollection<ObservableCollection<Tile>> flattenedAreaMap)
        {
            //            Console.WriteLine("Grid serialisation not yet implemented!");
            for(var i = 0; i < flattenedAreaMap.Count; i++)
            {
                for(var j = 0; j < flattenedAreaMap[ i ].Count; j++)
                    Map.Grid[ i, j ] = flattenedAreaMap[ i ][ j ];
            }

            return Map?.Grid;
        }

        /// <summary>
        ///     Save the map state by event
        /// </summary>
        private void SaveMap()
        {
            
            Map.Grid = BindableGridToMap(_tileGrid);
            Messenger.Default.Send(new SaveMapMessage(Map));
        }


        /// <summary>
        ///     Import data for a single area
        /// </summary>
        private void ImportMap()
        {
            _mapEditorService.LoadMap(
                (areaMap, error) => {
                    if(error != null)
                        MessageBox.Show("Could not load world from service: " + error.Message);
                    else
                    {
                        if(areaMap != null)
                        {
                            Console.WriteLine("Loaded!");
                            Map = areaMap;
                            MapToBindableGrid();
                        }
                    }
                });
        }

        /// <summary>
        ///     Export the current area on its own
        /// </summary>
        private void ExportMap()
        {
            Map.Grid = BindableGridToMap(_tileGrid);
            _mapEditorService.SaveMap(Map);
        }


        /// <summary>
        ///     Invalidates the map.
        ///     TODO: Make ClearMap produce a valid Map
        /// </summary>
        private void ClearMap()
        {
            Map.Fill(null);
        }

        /// <summary>
        ///     Print the content of the current loaded Map
        ///     for debugging. Only prints the sprite name for now.
        /// </summary>
        private void PrintMap()
        {
            Console.WriteLine($@"Content of map '{Map.Name}':");
            for(var i = 0; i < Map.Rows; i++)
            {
                Console.Write("\t");
                for(var j = 0; j < Map.Columns; j++)
                    Console.Write($@"{Map.Grid[ i, j ]?.SpriteName}, ");

                Console.WriteLine();
            }

            Console.WriteLine("Walkable tiles:");
            for(var i = 0; i < Map.Rows; i++)
            {
                Console.Write("\t");
                for(var j = 0; j < Map.Columns; j++)
                {
                    if(Map.Grid[ i, j ] != null && Map.Grid[ i, j ].IsWalkable)
                        Console.Write("W ");
                    else
                        Console.Write(". ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("Spawn tiles:");
            for(var i = 0; i < Map.Rows; i++)
            {
                Console.Write("\t");
                for(var j = 0; j < Map.Columns; j++)
                {
                    if(Map.Grid[ i, j ] != null && Map.Grid[ i, j ].IsSpawnPoint)
                        Console.Write("S ");
                    else
                        Console.Write(". ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("Transition tiles:");
            for(var i = 0; i < Map.Rows; i++)
            {
                Console.Write("\t");
                for(var j = 0; j < Map.Columns; j++)
                {
                    if(Map.Grid[ i, j ] != null && Map.Grid[ i, j ].IsTransitionSpot)
                        Console.Write("T ");
                    else
                        Console.Write(". ");
                }

                Console.WriteLine();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Console.WriteLine($"{propertyName} modified.");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public new event PropertyChangedEventHandler PropertyChanged;
    }
}
