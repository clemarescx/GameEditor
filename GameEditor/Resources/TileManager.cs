using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GameEditor.Resources{
	public class TileManager{
		private const string TerrainTilesDirectoryPath = "Resources/tiles";
		public Dictionary<string, Tile> TerrainTiles{ get; }

		public TileManager(){
			TerrainTiles = new Dictionary<string, Tile>();
			LoadTerrainTiles();
		}

		private void LoadTerrainTiles(){
			try{
				var terrainTilesDir = new DirectoryInfo(TerrainTilesDirectoryPath);
				terrainTilesDir.GetFiles("*.png")
				               .ToList()
				               .ForEach(
				               file => {
					               Console.WriteLine($@"Loading {file.Name}...");
					               var img = new BitmapImage(new Uri(file.FullName));
					               TerrainTiles[file.Name] = new Tile{ TileImage = img, Name = file.Name };
				               });
			}
			catch(Exception e){
				Console.WriteLine($@"Could not load terrain tiles: {e.Message}");
			}
		}

		public Tile GetTerrainTile(string tileName){ return TerrainTiles[tileName]; }
	}
}