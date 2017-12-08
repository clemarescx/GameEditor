using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace GameEditor
{
    /// <summary>
    ///     Interaction logic for MapEditorControl.xaml
    /// </summary>
    public partial class MapEditorControl : UserControl, INotifyPropertyChanged
    {
        public MapEditorControl()
        {
            InitializeComponent();
        }

        private void AddToGrid(Grid grid, int row, int col, Image img)
        { /*
            Grid.SetRow(img, row);
            Grid.SetColumn(img, col);
            grid.Children.Add(img);*/
        }

        private void DrawMap()
        { /*
            for(var i = 0; i < Map.Rows; i++)
            {
                for(var j = 0; j < Map.Columns; j++)
                    UpdateTileAt(i, j);
            }*/
        }

        private void UpdateTileAt(int row, int column)
        { /*
            var terrainImg = new Image();
            var tilevalue = Map.Grid[ row, column ];
            var tilename = Map.TerrainSpriteNameTable[ tilevalue ];
            var tilename = Map.Grid[ row, column ].SpriteName;
            terrainImg.Source = _mapEditorService.GetTerrainSprite(tilename);
            terrainImg.PreviewMouseDown += TerrainMapGrid_OnMouseDown;
            AddToGrid(TerrainMapGrid, row, column, terrainImg);
            */
        }

        // Prepare the grid space
        private void InitTileGrid(Grid tileGrid, int rows, int cols)
        {
            /* tileGrid.ShowGridLines = true;
             var spacing = new GridLength(3, GridUnitType.Star);
 
             for(var i = 0; i < rows; i++) tileGrid.RowDefinitions.Add(new RowDefinition{ Height = spacing });
             for(var j = 0; j < cols; j++) tileGrid.ColumnDefinitions.Add(new ColumnDefinition{ Width = spacing });
             */
        }


        // the "paint" function - applies the current selected tile's sprite to the
        // terrain grid
        private void TerrainMapGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
        { /*
            if(sender != null)
            {
                var image = sender as Image;
                var row = (int)image.GetValue(Grid.RowProperty);
                var col = (int)image.GetValue(Grid.ColumnProperty);
                Console.WriteLine($@"Grid clicked in cell {row},{col}");
                var paintbrushTileName = BrushTile.SpriteName;

                if(!Map.TerrainSpriteNameTable.Contains(paintbrushTileName))
                {
                    if the sprite has not been used for this

                    map before, add it to the index table
                    Console.WriteLine($@"{paintbrushTileName} added to Map's tile index");
                    Map.TerrainSpriteNameTable.Add(paintbrushTileName);
                }

                update both the model and the view
                Map.Grid[ row, col ] = Map.TerrainSpriteNameTable.IndexOf(paintbrushTileName);
                image.Source = BrushTile.TileImage;
            }*/
        }

        //////////////
        /// Boilerplate event listener for WPF to update when some property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
