using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GameEditor.Resources{
	public class TileManager{
		private const string TerrainTilesDirectoryPath = "Resources/tiles/terrain";
		private const string LogicTilesDirectoryPath = "Resources/tiles/logic";
		public Dictionary<string, Tile> TerrainTiles = new Dictionary<string, Tile>();
		public Dictionary<string, Tile> LogicTiles = new Dictionary<string, Tile>();

		public TileManager(){
			LoadTerrainTiles();
			LoadLogicTiles();
		}

		private void LoadTerrainTiles(){
			LoadTiles(TerrainTilesDirectoryPath, ref TerrainTiles);
		}

		private void LoadLogicTiles() {
			LoadTiles(LogicTilesDirectoryPath, ref LogicTiles);
		}

		private void LoadTiles(string path, ref Dictionary<string, Tile> dict){
			try {
				var directory = new DirectoryInfo(path);
				var fileList = directory.GetFiles("*.png").ToList();
				foreach(var file in fileList){
					Console.Write($@"Loading {file.Name}...");
					var img = new BitmapImage(new Uri(file.FullName));
					var fileName = Path.GetFileNameWithoutExtension(file.Name);
					Console.WriteLine($@"{fileName}");
					dict[fileName] = new Tile { TileImage = img, Name = file.Name };
				}
				         
			}
			catch(Exception e) {
				Console.WriteLine($@"Could not load terrain tiles: {e.Message}");
			}
		}

		public Tile GetTerrainTile(string tileName){ return TerrainTiles[tileName]; }
	}
}