using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using GameEditor.Models;

namespace GameEditor.Services
{
    /// <summary>
    ///     Loads the tile graphics per category (folder) and stores them in distinct
    ///     collections as Tile objects.
    /// </summary>
    public class MapEditorService : IMapEditorService
    {
        private const string TerrainTilesDirectoryPath = "/terrain";
        private const string LogicTilesDirectoryPath = "/logic";

        private readonly DirectoryInfo _resourcesDirectory = new DirectoryInfo("Resources/tiles");
        private Dictionary<string, BitmapImage> LogicTiles = new Dictionary<string, BitmapImage>();
        private Dictionary<string, BitmapImage> TerrainTiles = new Dictionary<string, BitmapImage>();

        //		public TerrainTile DefaultTile{ get; private set; }
        //		public Tile ErrorTile{ get; private set; }

        /// <summary>
        ///     Acts as a flyweight for tile sprites.
        /// </summary>
        public MapEditorService()
        {
            LoadTerrainTiles();
            LoadLogicTiles();
            //			SetDefaultTile();
            //			SetErrorTile();
        }

        //		private void SetErrorTile()
        //		{
        //			// "error.png" is located directly in the Resources folder.
        //			var filepath = _resourcesDirectory.GetFiles("*.png");
        //			ErrorTile = new Tile{
        //				TileImage = new BitmapImage(new Uri(filepath[ 0 ].FullName)),
        //				Name = Path.GetFileNameWithoutExtension(filepath[ 0 ].Name)
        //			};
        //		}
        //
        //		private void SetDefaultTile()
        //		{
        //			DefaultTile = GetTerrainTile("sand_1.png");
        //		}

        private void LoadTerrainTiles()
        {
            var loader = new SpriteLoader(_resourcesDirectory + TerrainTilesDirectoryPath);
            TerrainTiles = loader.Sprites;
        }

        private void LoadLogicTiles()
        {
            var loader = new SpriteLoader(_resourcesDirectory + LogicTilesDirectoryPath);
            LogicTiles = loader.Sprites;
        }

        public TerrainTile GetTerrainTile(string tileName)
        {
            return TerrainTiles[ tileName ] ?? TerrainTiles[ "sand_1.png" ];
        }

        public void GetTerrainTiles(Action<IEnumerable<Tile>, Exception> callback)
        {
            //TODO: 
            callback(new List<Tile>(TerrainTiles.Values), null);
        }

        public void GetLogicTiles(Action<IEnumerable<Tile>, Exception> callback)
        {
            callback(new List<Tile>(LogicTiles.Values), null);
        }
    }
}
