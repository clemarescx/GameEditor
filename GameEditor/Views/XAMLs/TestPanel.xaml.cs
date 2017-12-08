using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameEditor
{
    /// <summary>
    ///     Used as WPF testing grounds
    /// </summary>
    public partial class TestPanel : UserControl
    {
        private const int GridSize = 16;

        public TestPanel()
        {
            InitializeComponent();

            CreateGrid();

            var dir = new DirectoryInfo("Resources/tiles/terrain");
            var imgFileList = dir.GetFiles("*.png");
            var imgList = imgFileList.ToList().Select(x => new BitmapImage(new Uri(x.FullName)));
            var imgListIterator = imgList.GetEnumerator();

            var hasNextTile = true;
            imgListIterator.MoveNext();
            for(var i = 0; i < GridSize && hasNextTile; i++)
            {
                for(var j = 0; j < GridSize && hasNextTile; j++)
                {
                    var currImg = new Image{ Stretch = Stretch.None, Source = imgListIterator.Current };
                    Grid.SetRow(currImg, i);
                    Grid.SetColumn(currImg, j);
                    TestGrid.Children.Add(currImg);
                    hasNextTile = imgListIterator.MoveNext();
                }
            }

            imgListIterator.Dispose();
        }

        private void CreateGrid()
        {
            TestGrid.ShowGridLines = true;
            for(var i = 0; i < GridSize; i++)
            {
                var spacing = new GridLength(3, GridUnitType.Star);
                TestGrid.ColumnDefinitions.Add(new ColumnDefinition{ Width = spacing });
                TestGrid.RowDefinitions.Add(new RowDefinition{ Height = spacing });
            }
        }
    }
}
