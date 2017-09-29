using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameEditor.Annotations;
using GameEditor.Resources;
using Image = System.Windows.Controls.Image;

namespace GameEditor{
	/// <summary>
	/// Interaction logic for MapEditorControl.xaml
	/// </summary>
	public partial class MapEditorControl : UserControl, INotifyPropertyChanged{
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null){
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public bool IsDirty{ get; set; }
		private Zone Map{ get; set; }

		private Tile _imgSelectedtile;

		public Tile BrushTile
		{
			get => _imgSelectedtile;
			set {
				_imgSelectedtile = value;
				OnPropertyChanged();
			}
		}

		private readonly TileManager _tileManager;
		public ObservableCollection<Tile> TerrainTiles{ get; set; }
		public ObservableCollection<Tile> LogicTiles{ get; set; }

		public MapEditorControl(){
			InitializeComponent();
			DataContext = this;

			_tileManager = new TileManager();
			BrushTile = _tileManager.DefaultTile;

			TerrainTiles = new ObservableCollection<Tile>(_tileManager.TerrainTiles.Values);
			LogicTiles = new ObservableCollection<Tile>(_tileManager.LogicTiles.Values);

			Map = new Zone(8, _tileManager.DefaultTile.Name);

			ViewMapGrid.ShowGridLines = true;

			ViewMapGrid.Background = Brushes.YellowGreen;
			DrawMap(Map);
		}

		private void DrawMap(Zone map){
			InitTileGrid(ViewMapGrid, map.Rows, map.Columns);

			

			for(int i = 0; i < map.Rows; i++){
				for(int j = 0; j < map.Columns; j++){
					var img = new Image();

					var tilevalue = map.TerrainGrid[i, j];
					var tilename = map.TileNamesInUse[tilevalue];
					var tile = _tileManager.GetTerrainTile(tilename);
					img.Source = tile.TileImage;
					img.PreviewMouseDown += ViewMapGrid_OnMouseDown;
					Grid.SetRow(img, i);
					Grid.SetColumn(img, j);
					ViewMapGrid.Children.Add(img);
				}
			}
		}

		private void InitTileGrid(Grid tileGrid, int rows, int cols){
			tileGrid.ShowGridLines = true;
			var spacing = new GridLength(3, GridUnitType.Star);

			for(int i = 0; i < rows; i++){
				tileGrid.RowDefinitions.Add(new RowDefinition{ Height = spacing });
			}

			for(int j = 0; j < cols; j++){
				tileGrid.ColumnDefinitions.Add(new ColumnDefinition{ Width = spacing });
			}
		}


		private void BtnLoadMap(object sender, RoutedEventArgs e){ }

		private void BtnSaveMap(object sender, RoutedEventArgs e){ }

		private void BtnClearMap(object sender, RoutedEventArgs e){ Map.Fill(0); }


		private void TilesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e){
			var lb = sender as ListBox;
			Tile selected = (Tile)lb?.SelectedItem;
			BrushTile = selected;

			Console.WriteLine($@"Selected: {selected?.Name}");
		}

		private void ViewMapGrid_OnMouseDown(object sender, MouseButtonEventArgs e){
			if(sender != null){
				if(BrushTile.Name != "default"){
					var image = sender as Image;
					int row = (int)image.GetValue(Grid.RowProperty);
					int col = (int)image.GetValue(Grid.ColumnProperty);
					Console.WriteLine($@"Grid clicked in cell {row},{col}");

					string paintbrushTileName = BrushTile.Name;

					if(!Map.TileNamesInUse.Contains(paintbrushTileName)){
						Console.WriteLine($@"{paintbrushTileName} added to Map's tile index");
						Map.TileNamesInUse.Add(paintbrushTileName);
					}
					Map.TerrainGrid[row, col] = Map.TileNamesInUse.IndexOf(paintbrushTileName);
				}
				else{
					Console.WriteLine("No tile selected");
				}
			}
		}

		private void BtnPrintMap(object sender, RoutedEventArgs e){
			Console.WriteLine($"Content of map '{Map.Name}':");
			
			Console.WriteLine("{ ");
			for(int i = 0; i < Map.Rows; i++){
				Console.Write("\t");
				for(int j = 0; j < Map.Columns; j++){
					Console.Write($"{Map.TerrainGrid[i,j]}, ");
				}
				Console.WriteLine();
			}
			Console.WriteLine("}");
		}
	}

	/// <summary>
	/// Converter to avoid getting 
	/// </summary>
	[ValueConversion(typeof(Tile), typeof(BitmapImage))]
	public class TileToBitmapImageConverter : IValueConverter{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture){
			Tile selectedTile = (Tile)value;
			return selectedTile?.TileImage;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture){
			return null;
		}
	}
}