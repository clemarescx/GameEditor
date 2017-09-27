using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GameEditor.Resources;
using Color = System.Windows.Media.Color;
using Image = System.Windows.Controls.Image;

namespace GameEditor{
	


	/// <summary>
	/// Interaction logic for MapEditorControl.xaml
	/// </summary>
	public partial class MapEditorControl : UserControl{
		public bool IsDirty{ get; set; }
		private GameMap Map{ get; set; }

		private NodeGrid _mapGrid;

		public ObservableCollection<Tile> TerrainTiles{ get; set; }
		public ObservableCollection<Tile> LogicTiles{ get; set; }

		public MapEditorControl(){
			InitializeComponent();
			DataContext = this;

			var tileManager = new TileManager();

			TerrainTiles = new ObservableCollection<Tile>(tileManager.TerrainTiles.Values);
			LogicTiles = new ObservableCollection<Tile>(tileManager.LogicTiles.Values);

			Map = new GameMap(16, 16);
			ViewMapGrid.Rows = ViewMapGrid.Columns = 16;
			ViewLogicGrid.Rows = ViewLogicGrid.Columns = 16;

			LoadDefaultMapData();
		}
		


		private void LoadDefaultMapData(){
			// when the canvas is loaded in the app, throw an event to build the map grid
			CvsMapGrid.AddHandler(LoadedEvent, new RoutedEventHandler(cvsMapGrid_loaded));
		}

		private void cvsMapGrid_loaded(object sender, RoutedEventArgs e){
			_mapGrid = new NodeGrid(16, 16);
			DrawGrid(_mapGrid, Colors.Red);
		}


		private void DrawMap(GameMap map){
			
		}

		/// <summary>
		/// Dynamically draw grid to canvas
		/// TODO: consider using an XAML Grid filled with buttons
		/// </summary>
		/// <param name="mapGrid"></param>
		/// <param name="lineColor"></param>
		private void DrawGrid(NodeGrid mapGrid, Color lineColor){
			// draw rows
			int rowStep = (int)(CvsMapGrid.ActualHeight / mapGrid.Rows);


			for(int i = 0; i <= mapGrid.Rows; i++){
				var line = new Line{
					StrokeThickness = 1,
					Stroke = new SolidColorBrush(lineColor),
					X1 = 0,
					X2 = CvsMapGrid.ActualWidth,
					Y1 = i * rowStep,
					Y2 = i * rowStep
				};
				CvsMapGrid.Children.Add(line);
			}

			// draw columns
			var columnStep = (int)(CvsMapGrid.ActualWidth / mapGrid.Columns);

			for(int i = 0; i <= mapGrid.Columns; i++){
				var line = new Line{
					StrokeThickness = 1,
					Stroke = new SolidColorBrush(lineColor),
					X1 = i * columnStep,
					X2 = i * columnStep,
					Y1 = 0,
					Y2 = CvsMapGrid.ActualHeight
				};

				CvsMapGrid.Children.Add(line);
			}
		}


		private void BtnLoadMap(object sender, RoutedEventArgs e){ }

		private void BtnSaveMap(object sender, RoutedEventArgs e){ }

		private void BtnClearMap(object sender, RoutedEventArgs e){
			_mapGrid.Reset();
			CvsMapGrid.Children.Clear();
			DrawGrid(_mapGrid, Colors.Blue);
		}


		private void TilesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e){
			var lb = sender as ListBox;
			Tile selected = (Tile)lb?.SelectedItem;
			ImgSelectedTile.Source = selected?.TileImage;
//			SelectedTile = selected;
//			Console.WriteLine($@"Selected: {SelectedTile?.Name}");
			Console.WriteLine($@"Selected: {selected?.Name}");
		}
	}
}