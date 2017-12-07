using System;
using System.Windows.Controls;
using GameEditor.Models;

namespace GameEditor.Views
{
    /// <summary>
    ///     Interaction logic for TileView.xaml
    /// </summary>
    public partial class TileView : UserControl
    {
        private readonly Tile _tile;
        private readonly string terrainSpritesPath = "../Resources/tiles/terrain/";

        public string TerrainFilePath
        {
            get
            {
                var path = terrainSpritesPath + _tile.SpriteName;
                try
                {
                    var uri = new Uri(path);
                    return uri.AbsoluteUri;
                }
                catch(Exception e)
                {
                    Console.WriteLine(@"Selected:{0}", path);
                    Console.WriteLine(e.Message);
                }

                return "../Resources/tiles/error.png";
            }
        }

        public bool IsWalkable { get; set; }
        public bool IsSpawnPoint { get; set; }
        public bool IsTransitionSpot{ get; set; }

        public TileView(Tile tile)
        {
            _tile = tile;

            _tile.SpriteName = "error.png";
            InitializeComponent();
        }
    }
}
