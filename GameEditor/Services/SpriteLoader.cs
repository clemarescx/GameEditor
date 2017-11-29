using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace GameEditor.Services
{
    public class SpriteLoader
    {
        public Dictionary<string, BitmapImage> Sprites{ get; }

        public SpriteLoader(string path)
        {
            Sprites = new Dictionary<string, BitmapImage>();
            try
            {
                var dir = new DirectoryInfo(path);
                var fileList = dir.GetFiles("*.png").ToList();
                Console.Write($@"Loading sprites from {path}... ");

                foreach(var file in fileList)
                {
                    var img = new BitmapImage(new Uri(file.FullName));
                    var fileName = file.Name;
                    Sprites[ fileName ] = img;
                }

                Console.WriteLine(@"Done.");
            }
            catch(Exception e)
            {
                Console.WriteLine($@"Could not load sprites: {e.Message}");
            }
        }
    }
}
