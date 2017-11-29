using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using GameEditor.Models;
using GameEditor.Services;

namespace GameEditor.Design
{
	public class DesignMapEditorService : IMapEditorService
	{
		private readonly DirectoryInfo _resourcesDirectory = new DirectoryInfo("Resources/tiles");
		private Tile ErrorTile{ get; }

		public DesignMapEditorService()
		{
			var filepath = _resourcesDirectory.GetFiles("*.png");
			ErrorTile = new Tile{
				TileImage = new BitmapImage(new Uri(filepath[ 0 ].FullName)),
				Name = Path.GetFileNameWithoutExtension(filepath[ 0 ].Name)
			};
		}


		public void GetTerrainTiles(Action<IEnumerable<Tile>, Exception> callback)
		{
			var tiles = new List<Tile>{ ErrorTile };
			callback(tiles, null);
		}

		public void GetLogicTiles(Action<IEnumerable<Tile>, Exception> callback)
		{
			var tiles = new List<Tile>{ ErrorTile, ErrorTile, ErrorTile };
			callback(tiles, null);
		}
	}
}
