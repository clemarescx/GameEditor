using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace GameEditor{
	/// <summary>
	/// Interaction logic for TestPanel.xaml
	/// </summary>
	public partial class TestPanel : UserControl{
		private const int GRID_SIZE = 16;

		public TestPanel(){
			InitializeComponent();

			CreateGrid();
			
			var dir = new DirectoryInfo("Resources/tiles/terrain");
			var imgFileList = dir.GetFiles("*.png");
			var imgList = imgFileList.ToList().Select(x => new BitmapImage(new Uri(x.FullName)));
			var imgListIterator = imgList.GetEnumerator();

			bool hasNextTile = true;
			imgListIterator.MoveNext();
			for(int i = 0; i < GRID_SIZE && hasNextTile; i++){
				for(int j = 0; j < GRID_SIZE && hasNextTile; j++){
					var currImg = new Image{
						Stretch = Stretch.None,
						Source = imgListIterator.Current
					};
					Grid.SetRow(currImg, i);
					Grid.SetColumn(currImg, j);
					TestGrid.Children.Add(currImg);
					hasNextTile = imgListIterator.MoveNext();
				}
			}
			imgListIterator.Dispose();
		}

		private void CreateGrid(){
			TestGrid.ShowGridLines = true;
			for(int i = 0; i < GRID_SIZE; i++){
				var spacing = new GridLength(3, GridUnitType.Star);
				TestGrid.ColumnDefinitions.Add(new ColumnDefinition{ Width = spacing });
				TestGrid.RowDefinitions.Add(new RowDefinition{ Height = spacing });
			}
		}
	}
}