using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GameEditor.Resources{
	/// <summary>
	/// Loads the tile graphics per category (folder) and stores them in distinct
	/// collections as Tile objects. 
	/// </summary>
	public class TileManager{
		readonly DirectoryInfo _resourcesDirectory = new DirectoryInfo("Resources/tiles");
		private const string TerrainTilesDirectoryPath = "/terrain";
		private const string LogicTilesDirectoryPath = "/logic";
		public TerrainTile DefaultTile{ get; private set; }
		public Tile ErrorTile{ get; private set; }

		public Dictionary<string, TerrainTile> TerrainTiles = new Dictionary<string, TerrainTile>();
		public Dictionary<string, LogicTile> LogicTiles = new Dictionary<string, LogicTile>();

		public TileManager(){
			LoadTerrainTiles();
			LoadLogicTiles();
			SetDefaultTile();
			SetErrorTile();

			Console.WriteLine(Directory.GetCurrentDirectory());
		}

		private void SetErrorTile(){
			// "error.png" is located directly in the Resources folder.
			var filepath = _resourcesDirectory.GetFiles("*.png");
			ErrorTile = new Tile{
				TileImage = new BitmapImage(new Uri(filepath[0].FullName)),
				Name = Path.GetFileNameWithoutExtension(filepath[0].Name)
			};
		}

		private void SetDefaultTile(){
			DefaultTile = GetTerrainTile("sand_1.png");
		}

		private void LoadTerrainTiles(){
			//			LoadTiles(TerrainTilesDirectoryPath, ref TerrainTiles);
			var loader = new TileLoader<TerrainTile>(_resourcesDirectory + TerrainTilesDirectoryPath);
			TerrainTiles = loader.Tiles;
		}

		private void LoadLogicTiles(){
			//			LoadTiles(LogicTilesDirectoryPath, ref LogicTiles);
			var loader = new TileLoader<LogicTile>(_resourcesDirectory + LogicTilesDirectoryPath);
			LogicTiles = loader.Tiles;
		}

		/*
		private void LoadTiles(string tileSubDirectory, ref Dictionary<string, Tile> dict){
			try{
				var dir = new DirectoryInfo(_resourcesDirectory + tileSubDirectory);
				var fileList = dir.GetFiles("*.png").ToList();
				Console.Write($@"Loading files from {tileSubDirectory}... ");

				foreach(var file in fileList){
					var img = new BitmapImage(new Uri(file.FullName));
					var fileName = file.Name;
					dict[fileName] = new Tile{ TileImage = img, Name = file.Name };
				}
				Console.WriteLine("Done.");
			}
			catch(Exception e){
				Console.WriteLine($@"Could not load terrain tiles: {e.Message}");
			}
		}
		*/
		public TerrainTile GetTerrainTile(string tileName){
			return TerrainTiles.Keys.Contains(tileName) ? TerrainTiles[tileName] : (TerrainTile) ErrorTile;
		}
	}
}