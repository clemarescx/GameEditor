using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GameEditor.Resources{
	class TileManager{
		private const string TerrainTilesDirectoryPath = "/Resources/tiles";
		public Dictionary<string, Tile> TerrainTiles{ get; }

		public TileManager(){
			TerrainTiles = new Dictionary<string, Tile>();
			LoadTerrainTiles();
		}

		private void LoadTerrainTiles(){
			try{
				new DirectoryInfo(TerrainTilesDirectoryPath)
					.GetFiles("*.png")
					.ToList()
					.ForEach(
					file => {
						var img = new BitmapImage(new Uri(file.FullName));
						TerrainTiles[file.Name] = new Tile{ Img = img, Name = file.Name };
					});
			}
			catch(Exception e){
				Console.WriteLine($@"Could not load terrain tiles: {e.Message}");
				throw;
			}
		}

		public Tile GetTerrainTile(string tileName){ return TerrainTiles[tileName]; }
	}
}