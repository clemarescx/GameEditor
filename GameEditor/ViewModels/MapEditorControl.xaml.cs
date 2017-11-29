﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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

namespace GameEditor
{
	/// <summary>
	///     Interaction logic for MapEditorControl.xaml
	/// </summary>
	public partial class MapEditorControl : UserControl, INotifyPropertyChanged
	{
		private readonly TileManager _tileManager;
		private Tile _brushTile;
		/// 
		//////////////


		private Map Map{ get; set; }
		public ObservableCollection<TerrainTile> TerrainTiles{ get; set; }
		public ObservableCollection<LogicTile> LogicTiles{ get; set; }
		public Tile BrushTile
		{
			get => _brushTile;
			set
			{
				_brushTile = value;
				OnPropertyChanged();
			}
		}

		public MapEditorControl()
		{
			InitializeComponent();
			DataContext = this;

			_tileManager = new TileManager();

			TerrainTiles = new ObservableCollection<TerrainTile>(_tileManager.TerrainTiles.Values);
			LogicTiles = new ObservableCollection<LogicTile>(_tileManager.LogicTiles.Values);
			BrushTile = _tileManager.DefaultTile;

			Map = new Map(8, _tileManager.DefaultTile.Name);

			TerrainMapGrid.ShowGridLines = true;
			TerrainMapGrid.Background = Brushes.YellowGreen;

			InitTileGrid(TerrainMapGrid, Map.Rows, Map.Columns);
			//			InitTileGrid(LogicMapGrid, Map.Rows, Map.Columns);
			//			InitTileGrid(MouseEventGrid, Map.Rows, Map.Columns);

			DrawMap(Map);
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void AddToGrid(Grid grid, int row, int col, Image img)
		{
			Grid.SetRow(img, row);
			Grid.SetColumn(img, col);
			grid.Children.Add(img);
		}

		private void DrawMap(Map map)
		{
			for(var i = 0; i < map.Rows; i++)
			{
				for(var j = 0; j < map.Columns; j++)
				{
					var terrainImg = new Image();
					var tilevalue = map.TerrainGrid[ i, j ];
					var tilename = map.TerrainTileNamesInUse[ tilevalue ];
					var tile = _tileManager.GetTerrainTile(tilename);
					terrainImg.Source = tile.TileImage;
					terrainImg.PreviewMouseDown += TerrainMapGrid_OnMouseDown;
					AddToGrid(TerrainMapGrid, i, j, terrainImg);
				}
			}
			//TODO: render logic tiles
		}

		private void LogicMapGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Console.WriteLine(@"Done.");
		}

		// Prepare the grid space
		private void InitTileGrid(Grid tileGrid, int rows, int cols)
		{
			tileGrid.ShowGridLines = true;
			var spacing = new GridLength(3, GridUnitType.Star);

			for(var i = 0; i < rows; i++) tileGrid.RowDefinitions.Add(new RowDefinition{ Height = spacing });
			for(var j = 0; j < cols; j++) tileGrid.ColumnDefinitions.Add(new ColumnDefinition{ Width = spacing });
		}

		// Load from JSON
		private void BtnLoadMap(object sender, RoutedEventArgs e)
		{
			var openFileDialog = new OpenFileDialog{ InitialDirectory = Directory.GetCurrentDirectory() };

			if(openFileDialog.ShowDialog() != true)
				return;

			try
			{
				var jsonZone = File.ReadAllText(openFileDialog.FileName);
				Map = JsonConvert.DeserializeObject<Map>(jsonZone);

				TxtMapName.Text = Map.Name;
				DrawMap(Map);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error: \n" + ex.Message);
				throw;
			}
		}

		// Save to JSON
		private void BtnSaveMap(object sender, RoutedEventArgs e)
		{
			Map.Name = TxtMapName.Text;

			var jsonConvertZone = JsonConvert.SerializeObject(Map);

			var filename = string.Empty.Equals(Map.Name) || null == Map.Name ? "newMap" : Map.Name;
			filename += ".json";

			var saveFileDialog = new SaveFileDialog{
				FileName = filename,
				InitialDirectory = Directory.GetCurrentDirectory(),
				Filter = "JSON file (*.json)|*.json"
			};
			Map.Name = saveFileDialog.SafeFileName;

			if(saveFileDialog.ShowDialog() == true)
			{
				try
				{
					File.WriteAllText(saveFileDialog.FileName, jsonConvertZone);
				}
				catch(Exception ex)
				{
					MessageBox.Show("Error: \n" + ex.Message);
				}
			}
		}

		// Reset map to tile "0", (sand_1.png by default)
		private void BtnClearMap(object sender, RoutedEventArgs e)
		{
			Map.Fill(0);
			DrawMap(Map);
		}

		// Change the tile brush according to selected tile in the lists
		private void TilesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var lb = sender as ListBox;
			var selected = (Tile)lb?.SelectedItem;
			BrushTile = selected;

			Console.WriteLine($@"Selected: {selected?.Name}");
		}


		// the "paint" function - applies the current selected tile's sprite to the
		// terrain grid
		private void TerrainMapGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			//			TODO: handle mouse events on the drawing grid through a superposed grid 
			//				  then delegate according to "brushTile"'s subtype
			//			
			if(sender != null)
			{
				var image = sender as Image;
				var row = (int)image.GetValue(Grid.RowProperty);
				var col = (int)image.GetValue(Grid.ColumnProperty);
				Console.WriteLine($@"TerrainGrid clicked in cell {row},{col}");
				var paintbrushTileName = BrushTile.Name;

				if(!Map.TerrainTileNamesInUse.Contains(paintbrushTileName))
				{
					// if the sprite has not been used for this map before, add it to the index table
					Console.WriteLine($@"{paintbrushTileName} added to Map's tile index");
					Map.TerrainTileNamesInUse.Add(paintbrushTileName);
				}

				// update both the model and the view
				Map.TerrainGrid[ row, col ] = Map.TerrainTileNamesInUse.IndexOf(paintbrushTileName);
				image.Source = BrushTile.TileImage;
			}
		}


		// For debugging
		private void BtnPrintMap(object sender, RoutedEventArgs e)
		{
			Console.WriteLine($@"Content of map '{Map.Name}':");
			Console.WriteLine(@"Done.");
			for(var i = 0; i < Map.Rows; i++)
			{
				Console.Write("\t");
				for(var j = 0; j < Map.Columns; j++) Console.Write($@"{Map.TerrainGrid[ i, j ]}, ");

				Console.WriteLine();
			}

			Console.WriteLine(@"Done.");
		}

		//////////////
		/// Boilerplate event listener for WPF to update when some property changes
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
	}

	// Converter to unpack a Tile's sprite from within XAML view file
	[ValueConversion(typeof(Tile), typeof(BitmapImage))]
	public class TileToBitmapImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var selectedTile = (Tile)value;
			return selectedTile?.TileImage;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
