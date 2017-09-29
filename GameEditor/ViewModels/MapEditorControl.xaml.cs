using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameEditor.Annotations;
using GameEditor.Resources;
using Microsoft.Win32;
using Newtonsoft.Json;
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


		private Map Map{ get; set; }
		private Tile _imgSelectedtile;

		public Tile BrushTile
		{
			get => _imgSelectedtile;
			set {
				_imgSelectedtile = value;
				OnPropertyChanged();
			}
		}

		private string _fileDialogDir;

		private string FileDialogDir
		{
			get => _fileDialogDir ?? Directory.GetCurrentDirectory();
			set => _fileDialogDir = value;
		}

		private readonly TileManager _tileManager;
		public ObservableCollection<TerrainTile> TerrainTiles{ get; set; }
		public ObservableCollection<Tile> LogicTiles{ get; set; }

		public MapEditorControl(){
			InitializeComponent();
			DataContext = this;

			_tileManager = new TileManager();

			TerrainTiles = new ObservableCollection<TerrainTile>(_tileManager.TerrainTiles.Values);
			LogicTiles = new ObservableCollection<Tile>(_tileManager.LogicTiles.Values);
			BrushTile = _tileManager.DefaultTile;

			Map = new Map(8, _tileManager.DefaultTile.Name);

			TerrainMapGrid.ShowGridLines = true;

			TerrainMapGrid.Background = Brushes.YellowGreen;

			InitTileGrid(TerrainMapGrid, Map.Rows, Map.Columns);
			InitTileGrid(LogicMapGrid, Map.Rows, Map.Columns);
			DrawMap(Map);
		}

		private void DrawMap(Map map){
			for(int i = 0; i < map.Rows; i++){
				for(int j = 0; j < map.Columns; j++){
					var img = new Image();

					var tilevalue = map.TerrainGrid[i, j];
					var tilename = map.TileNamesInUse[tilevalue];
					var tile = _tileManager.GetTerrainTile(tilename);
					img.Source = tile.TileImage;
					img.PreviewMouseDown += TerrainMapGrid_OnMouseDown;
					Grid.SetRow(img, i);
					Grid.SetColumn(img, j);
					TerrainMapGrid.Children.Add(img);
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


		private void BtnLoadMap(object sender, RoutedEventArgs e){
			var openFileDialog = new OpenFileDialog{
				InitialDirectory = Directory.GetCurrentDirectory()
			};

			if(openFileDialog.ShowDialog() != true)
				return;

			try{
				var jsonZone = File.ReadAllText(openFileDialog.FileName);
				Map = JsonConvert.DeserializeObject<Map>(jsonZone);

				TxtMapName.Text = Map.Name;
				DrawMap(Map);
			}
			catch(Exception ex){
				MessageBox.Show("Error: \n" + ex.Message);
				throw;
			}
		}

		private void BtnSaveMap(object sender, RoutedEventArgs e){
			Map.Name = TxtMapName.Text;

			var jsonConvertZone = JsonConvert.SerializeObject(Map);

			var filename = String.Empty.Equals(Map.Name) || null == Map.Name ? "newMap" : Map.Name;
			filename += ".json";

			SaveFileDialog saveFileDialog = new SaveFileDialog{
				FileName = filename,
				InitialDirectory = FileDialogDir,
				Filter = "JSON file (*.json)|*.json",
				RestoreDirectory = true
			};

			if(saveFileDialog.ShowDialog() == true){
				try{
					File.WriteAllText(saveFileDialog.FileName, jsonConvertZone);
				}
				catch(Exception ex){
					MessageBox.Show("Error: \n" + ex.Message);
				}
			}
		}

		private void BtnClearMap(object sender, RoutedEventArgs e){
			Map.Fill(0);
			DrawMap(Map);
		}


		private void TilesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e){
			var lb = sender as ListBox;
			Tile selected = (Tile)lb?.SelectedItem;
			BrushTile = selected;

			Console.WriteLine($@"Selected: {selected?.Name}");
		}

		private void TerrainMapGrid_OnMouseDown(object sender, MouseButtonEventArgs e){
			if(sender != null){
				var image = sender as Image;
				int row = (int)image.GetValue(Grid.RowProperty);
				int col = (int)image.GetValue(Grid.ColumnProperty);
				Console.WriteLine($@"TerrainGrid clicked in cell {row},{col}");

				string paintbrushTileName = BrushTile.Name;

				if(!Map.TileNamesInUse.Contains(paintbrushTileName)){
					Console.WriteLine($@"{paintbrushTileName} added to Map's tile index");
					Map.TileNamesInUse.Add(paintbrushTileName);
				}
				Map.TerrainGrid[row, col] = Map.TileNamesInUse.IndexOf(paintbrushTileName);
				image.Source = BrushTile.TileImage;
			}
		}

		private void BtnPrintMap(object sender, RoutedEventArgs e){
			Console.WriteLine($@"Content of map '{Map.Name}':");
			Console.WriteLine("{ ");
			for(int i = 0; i < Map.Rows; i++){
				Console.Write("\t");
				for(int j = 0; j < Map.Columns; j++){
					Console.Write($@"{Map.TerrainGrid[i, j]}, ");
				}
				Console.WriteLine();
			}
			Console.WriteLine(@"}");
		}

		private void BtnPrintView(object sender, RoutedEventArgs e){
			Console.WriteLine(@"Drawing map to grid...");
			DrawMap(Map);
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