using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameEditor.Models;

namespace GameEditor.Views
{
    /// <summary>
    /// Interaction logic for TileView.xaml
    /// </summary>
    public partial class TileView : UserControl
    {
        private string terrainSpritesPath = "../Resources/tiles/terrain/";
        private Tile _tile;

        public string TerrainFilePath
        {
            get
            {
                string path = terrainSpritesPath + _tile.SpriteName;
                try
                {
                    var uri = new Uri(path);
                    return uri.AbsoluteUri;
                }
                catch(Exception e)
                {
                    Console.WriteLine(@"Could not find a file at {0}",path);
                    Console.WriteLine(e.Message);
                }
                return "../Resources/tiles/error.png";
            }
        }
        
        public TileView(Tile tile )
        {
            _tile = tile;
            
            _tile.SpriteName = "error.png";
            InitializeComponent();
        }
    }
}
