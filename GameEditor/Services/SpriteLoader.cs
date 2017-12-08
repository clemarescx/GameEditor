using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace GameEditor.Services
{
    /// <summary>
    /// Singleton to access sprite resources through the whole program
    /// </summary>
    public static class SpriteLoader
    {
        public static Dictionary<string, BitmapImage> TerrainSprites{ get; }
        public static Dictionary<string, BitmapImage> LogicSprites{ get; }
        public static BitmapImage ErrorSprite{ get; set; }
        public static BitmapImage DefaultSprite{ get; set; }

        static SpriteLoader()
        {
            TerrainSprites = LoadSprites("Resources/tiles/terrain");
            LogicSprites = LoadSprites("Resources/tiles/logic");
            ErrorSprite = LoadSprites("Resources/tiles")[ "error.png" ];
            DefaultSprite = TerrainSprites[ "sand_1.png" ];
        }

        
        private static Dictionary<string, BitmapImage> LoadSprites(string path)
        {
            var sprites = new Dictionary<string, BitmapImage>();
            try
            {
                var dir = new DirectoryInfo(path);
                var fileList = dir.GetFiles("*.png").ToList();
                Console.Write($@"Loading sprites from {path}... ");

                foreach(var file in fileList)
                {
                    var img = new BitmapImage(new Uri(file.FullName));
                    var fileName = file.Name;
                    sprites[ fileName ] = img;
                }

                Console.WriteLine(@"Done.");
            }
            catch(Exception e)
            {
                Console.WriteLine($@"Could not load sprites: {e.Message}");
            }

            return sprites;
        }
    }
}
