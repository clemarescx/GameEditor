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
		public Tile DefaultTile{ get; private set; }

		public Dictionary<string, Tile> TerrainTiles = new Dictionary<string, Tile>();
		public Dictionary<string, Tile> LogicTiles = new Dictionary<string, Tile>();

		public TileManager(){
			SetDefaultTile();

			LoadTerrainTiles();
			LoadLogicTiles();
			Console.WriteLine(Directory.GetCurrentDirectory());
		}

		private void SetDefaultTile(){
			var filepath = _resourcesDirectory.GetFiles("*.png");
			DefaultTile = new Tile{
				TileImage = new BitmapImage(new Uri(filepath[0].FullName))
			};
		}


		private void LoadTerrainTiles(){ LoadTiles(TerrainTilesDirectoryPath, ref TerrainTiles); }

		private void LoadLogicTiles(){ LoadTiles(LogicTilesDirectoryPath, ref LogicTiles); }


		private void LoadTiles(string tileSubDirectory, ref Dictionary<string, Tile> dict){
			try{
				var dir = new DirectoryInfo(_resourcesDirectory + tileSubDirectory);
				var fileList = dir.GetFiles("*.png").ToList();
				Console.WriteLine($@"Loading files from {tileSubDirectory}... ");

				foreach(var file in fileList){
					Console.Write(@".");
					var img = new BitmapImage(new Uri(file.FullName));
					var fileName = Path.GetFileNameWithoutExtension(file.Name);
					dict[fileName] = new Tile{ TileImage = img, Name = file.Name };
				}
				Console.WriteLine(" Done.");
			}
			catch(Exception e){
				Console.WriteLine($@"Could not load terrain tiles: {e.Message}");
			}
		}

		public Tile GetTerrainTile(string tileName){
			return TerrainTiles.Keys.Contains(tileName) ? TerrainTiles[tileName] : DefaultTile; 
		}
	}
}