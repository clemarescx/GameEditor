using System;
using System.Collections.Generic;
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


        private AreaMap _areaMap;
        private string _brushTile;
        private List<ObservableCollection<Tile>> _flattenedAreaMap = new List<ObservableCollection<Tile>>();
        private ObservableCollection<AreaMap> _otherAreaMaps;
        private Tile _selectedTile;

        public ObservableCollection<string> TerrainSpriteNames{ get; set; }

        //////
        /// Commands, initialised in the constructor
        public RelayCommand BtnPrintMapCommand{ get; }
        public RelayCommand BtnImportMapCommand{ get; }
        public RelayCommand BtnExportMapCommand{ get; }
        public RelayCommand BtnClearMapCommand{ get; }
        public RelayCommand BtnDebugCommand{ get; }
        public RelayCommand BtnSaveMapCommand{ get; }


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

        /// <summary>
        ///     Bound to the editor grid
        /// </summary>
        public List<ObservableCollection<Tile>> FlattenedAreaMap
        {
            get => _flattenedAreaMap;
            set
            {
                _flattenedAreaMap = value;
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
        public ObservableCollection<AreaMap> OtherAreaMaps
        {
            get => _otherAreaMaps;
            set
            {
                _otherAreaMaps = value;
                _otherAreaMaps.Remove(_areaMap);
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

            // When selecting a map in the WorldEditor view, it is sent to this view
            // as the working AreaMap.
            Messenger.Default.Register<MapSelectedMessage>(
                this,
                msg => {
                    AreaMap = msg.SelectedMap;
                    if(AreaMap != null)
                        AreamapToBindableGrid();
                });

            Messenger.Default.Register<AreamapsAvailableMessage>(
                this,
                msg => { OtherAreaMaps = new ObservableCollection<AreaMap>(msg.AllMaps); });

            // For the list of sprites used in the sprite selector
            TerrainSpriteNames = new ObservableCollection<string>(SpriteLoader.TerrainSprites.Keys);

            BtnPrintMapCommand = new RelayCommand(PrintMap, () => AreaMap != null);
            BtnImportMapCommand = new RelayCommand(ImportMap);
            BtnExportMapCommand = new RelayCommand(ExportMap, () => AreaMap != null);
            BtnClearMapCommand = new RelayCommand(ClearMap);
            BtnDebugCommand = new RelayCommand(() => AreaMap = new AreaMap("CreatedfromDebug"));
            BtnSaveMapCommand = new RelayCommand(SaveMap);
        }

        /// <summary>
        ///     Transform the Tile[,] to be bindable.
        ///     The idea to bind the data to a list of horizontal StackPanels
        ///     was taken from:
        ///     http://www.thinkbottomup.com.au/site/blog/Game_of_Life_in_XAML_WPF_using_embedded_Python
        /// </summary>
        private void AreamapToBindableGrid()
        {
            for(var i = 0; i < AreaMap.Grid.GetLength(0); i++)
            {
                FlattenedAreaMap.Add(new ObservableCollection<Tile>());

                for(var j = 0; j < AreaMap.Grid.GetLength(1); j++)
                    FlattenedAreaMap[ i ].Add(AreaMap.Grid[ i, j ]);
            }
        }

        /// <summary>
        ///     Return the current map state of the editor to a 2D array of Tiles
        /// </summary>
        /// <param name="flattenedAreaMap"></param>
        /// <returns>The updated grid in its DTO form</returns>
        private Tile[,] BindableGridToAreaMap(List<ObservableCollection<Tile>> flattenedAreaMap)
        {
            Console.WriteLine("Grid serialisation not yet implemented!");
            return AreaMap.Grid;
        }

        /// <summary>
        ///     Save the map state by event
        /// </summary>
        private void SaveMap()
        {
            AreaMap.Grid = BindableGridToAreaMap(_flattenedAreaMap);
            Messenger.Default.Send(new SaveMapMessage(AreaMap));
        }

        /// <summary>
        ///     Print the content of the current loaded AreaMap
        ///     for debugging. Only prints the sprite name for now.
        /// </summary>
        private void PrintMap()
        {
            Console.WriteLine($@"Content of map '{AreaMap.Name}':");
            for(var i = 0; i < AreaMap.Rows; i++)
            {
                Console.Write("\t");
                for(var j = 0; j < AreaMap.Columns; j++)
                    Console.Write($@"{AreaMap.Grid[ i, j ]?.SpriteName}, ");

                Console.WriteLine();
            }
        }

        /// <summary>
        ///     Import data for a single area
        /// </summary>
        private void ImportMap()
        {
            _mapEditorService.LoadAreaMap(
                (areaMap, error) => {
                    if(error != null)
                        MessageBox.Show("Could not load world from service: " + error.Message);
                    else
                    {
                        if(areaMap != null)
                        {
                            Console.WriteLine("Loaded!");
                            AreaMap = areaMap;
                            AreamapToBindableGrid();
                        }
                    }
                });
        }

        /// <summary>
        ///     Export the current area on its own
        /// </summary>
        private void ExportMap()
        {
            AreaMap.Grid = BindableGridToAreaMap(_flattenedAreaMap);
            _mapEditorService.SaveAreaMap(AreaMap);
        }


        /// <summary>
        ///     Invalidates the map.
        ///     TODO: Make ClearMap produce a valid AreaMap
        /// </summary>
        private void ClearMap()
        {
            AreaMap.Fill(null);
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
