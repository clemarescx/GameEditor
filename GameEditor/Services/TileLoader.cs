using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using GameEditor.Models;

namespace GameEditor.Services
{
	/// <summary>
	///     Builds tiles with their respective
	///     sprite and name, and cast them to their subtype T
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class TileLoader<T> where T : Tile, new()
	{
		public Dictionary<string, T> Tiles{ get; }

		public TileLoader(string path)
		{
			Tiles = new Dictionary<string, T>();
			try
			{
				var dir = new DirectoryInfo(path);
				var fileList = dir.GetFiles("*.png").ToList();
				Console.Write($@"Loading files from {path}... ");

				foreach(var file in fileList)
				{
					var img = new BitmapImage(new Uri(file.FullName));
					var fileName = file.Name;
					Tiles[ fileName ] = new T{ TileImage = img, Name = file.Name };
				}

				Console.WriteLine(@"Done.");
			}
			catch(Exception e)
			{
				Console.WriteLine($@"Could not load terrain tiles: {e.Message}");
			}
		}
	}
}
