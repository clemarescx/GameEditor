using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameEditor.Services
{
    /// <summary>
    ///     Loads the tile graphics per category (folder) and stores them in distinct
    ///     collections as Tile objects.
    /// </summary>
    public class MapEditorService : IMapEditorService
    {
        private readonly Dictionary<string, BitmapImage> LogicTiles;
        private readonly Dictionary<string, BitmapImage> TerrainTiles;


        /// <summary>
        ///     Acts as a flyweight for tile sprites.
        /// </summary>
        public MapEditorService()
        {
            TerrainTiles = SpriteLoader.TerrainSprites;
            LogicTiles = SpriteLoader.LogicSprites;
        }

        public ImageSource GetTerrainSprite(string tilename)
        {
            return TerrainTiles[ tilename ];
        }

        public void GetDefaultTerrainSpriteName(Action<string, Exception> callback)
        {
            callback("sand_1.png", null);
        }

        public void GetTerrainTiles(Action<Dictionary<string, BitmapImage>, Exception> callback)
        {
            callback(TerrainTiles, null);
        }

        public void GetLogicTiles(Action<Dictionary<string, BitmapImage>, Exception> callback)
        {
            callback(LogicTiles, null);
        }
    }
}
