using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GameEditor.Models;
using GameEditor.Properties;
using GameEditor.Services;

namespace GameEditor.ViewModels
{
    public class MapEditorModel : ViewModelBase, INotifyPropertyChanged
    {
        public const string TerrainTilesPropertyName = "TerrainTiles";
        public const string LogicTilesPropertyName = "LogicTiles";
        public const string BrushTilePropertyName = "BrushTile";
        private Tile _brushTile;


        private ObservableCollection<Tile> _logicTiles;
        private ObservableCollection<Tile> _terrainTiles;

        public RelayCommand BtnPrintMapCommand{ get; }

        private Map Map{ get; }
        public Tile BrushTile
        {
            get => _brushTile;
            set
            {
                _brushTile = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Tile> LogicTiles
        {
            get => _logicTiles;
            set
            {
                if(_logicTiles == value) return;

                _logicTiles = value;
                RaisePropertyChanged(LogicTilesPropertyName);
            }
        }
        public ObservableCollection<Tile> TerrainTiles
        {
            get => _terrainTiles;
            set
            {
                if(_terrainTiles == value) return;

                _terrainTiles = value;
                RaisePropertyChanged(TerrainTilesPropertyName);
            }
        }

        public MapEditorModel(IMapEditorService service)
        {
            service.GetTerrainTiles(
                (tiles, error) => {
                    if(error != null)
                        MessageBox.Show(error.Message);
                    else
                        TerrainTiles = new ObservableCollection<Tile>(tiles);
                });
            service.GetLogicTiles(
                (tiles, error) => {
                    if(error != null)
                        MessageBox.Show(error.Message);
                    else
                        LogicTiles = new ObservableCollection<Tile>(tiles);
                });

            Map = new Map(8, "sand_1.png");
            BtnPrintMapCommand = new RelayCommand(BtnPrintMap);
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BtnPrintMap()
        {
            Console.WriteLine($@"Content of map '{Map.Name}':");
            Console.WriteLine(@"Done.");
            for(var i = 0; i < Map.Rows; i++)
            {
                Console.Write("\t");
                for(var j = 0; j < Map.Columns; j++) Console.Write($@"{Map.TerrainSpriteGrid[ i, j ]}, ");

                Console.WriteLine();
            }

            Console.WriteLine(@"Done.");
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
