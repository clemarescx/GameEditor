using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace GameEditor.Resources{
	/// <summary>
	/// Loads the tile graphics per category (folder) and stores them in distinct
	/// collections as Tile objects. 
	/// </summary>
	public class TileManager{

		//////////////
		// file paths
		readonly DirectoryInfo _resourcesDirectory = new DirectoryInfo("Resources/tiles");
		private const string TerrainTilesDirectoryPath = "/terrain";
		private const string LogicTilesDirectoryPath = "/logic";
		//////////////
		
		public TerrainTile DefaultTile{ get; private set; }
		public Tile ErrorTile{ get; private set; }

		public Dictionary<string, TerrainTile> TerrainTiles = new Dictionary<string, TerrainTile>();
		public Dictionary<string, LogicTile> LogicTiles = new Dictionary<string, LogicTile>();

		/// <summary>
		/// Acts as a flyweight for tile sprites.
		/// </summary>
		public TileManager(){
			LoadTerrainTiles();
			LoadLogicTiles();
			SetDefaultTile();
			SetErrorTile();
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
			var loader = new TileLoader<TerrainTile>(_resourcesDirectory + TerrainTilesDirectoryPath);
			TerrainTiles = loader.Tiles;
		}

		private void LoadLogicTiles(){
			var loader = new TileLoader<LogicTile>(_resourcesDirectory + LogicTilesDirectoryPath);
			LogicTiles = loader.Tiles;
		}

		public TerrainTile GetTerrainTile(string tileName){
			return TerrainTiles.Keys.Contains(tileName) ? TerrainTiles[tileName] : (TerrainTile) ErrorTile;
		}

		public LogicTile GetLogicTile(string tileName){ return LogicTiles[tileName]; }
	}
}