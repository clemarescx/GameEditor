using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace GameEditor{
	/// <summary>
	/// Interaction logic for TestPanel.xaml
	/// </summary>
	public partial class TestPanel : UserControl{
		public TestPanel(){
			InitializeComponent();

			createGrid();

			Canvas cvs = new Canvas{
				Height = 50,
				Width = 50,
				HorizontalAlignment = HorizontalAlignment.Left,

			};

			var tilesDirectory = new DirectoryInfo("Resources/tiles");
			
			var files = tilesDirectory.GetFiles("*.png");
			files.ToList().ForEach(Console.WriteLine);
			
			BitmapImage imgSrc = new BitmapImage( new Uri("/Resources/default.png", UriKind.Relative));
			BitmapImage imgSrc2 = new BitmapImage( new Uri("/Resources/bush.png", UriKind.Relative));

			Image img = new Image{
				//				Width = 50,
				//				Height = 50,
				Stretch = Stretch.Fill,
				Source = imgSrc
			};

			Grid.SetRow(img, 0);
			Grid.SetColumn(img, 0);

			TestGrid.Children.Add(img);

			Image img2 = new Image{
				//				Width = 50,
				//				Height = 50,
				Stretch = Stretch.Fill,
				Source = imgSrc2
			};

			Grid.SetRow(img2, 0);
			Grid.SetColumn(img2, 1);
			TestGrid.Children.Add(img2);


		}

		private void createGrid(){
			TestGrid.ShowGridLines = true;
			var coldef1 = new ColumnDefinition{ Width = new GridLength(3, GridUnitType.Star) };
			var coldef2 = new ColumnDefinition{ Width = new GridLength(3, GridUnitType.Star) };
			var coldef3 = new ColumnDefinition{ Width = new GridLength(3, GridUnitType.Star) };
			var coldef4 = new ColumnDefinition{ Width = new GridLength(3, GridUnitType.Star) };
			TestGrid.ColumnDefinitions.Add(coldef1);
			TestGrid.ColumnDefinitions.Add(coldef2);
			TestGrid.ColumnDefinitions.Add(coldef3);
			TestGrid.ColumnDefinitions.Add(coldef4);

			var rowdef1 = new RowDefinition{ Height = new GridLength(3, GridUnitType.Star) };
			var rowdef2 = new RowDefinition{ Height = new GridLength(3, GridUnitType.Star) };
			var rowdef3 = new RowDefinition{ Height = new GridLength(3, GridUnitType.Star) };
			var rowdef4 = new RowDefinition{ Height = new GridLength(3, GridUnitType.Star) };

			TestGrid.RowDefinitions.Add(rowdef1);
			TestGrid.RowDefinitions.Add(rowdef2);
			TestGrid.RowDefinitions.Add(rowdef3);
			TestGrid.RowDefinitions.Add(rowdef4);
		}
	}
}