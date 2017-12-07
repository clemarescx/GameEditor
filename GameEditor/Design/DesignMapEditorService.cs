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
				SpriteName = Path.GetFileNameWithoutExtension(filepath[ 0 ].Name)
			};
		}


	    public void GetTerrainTiles(Action<Dictionary<string, BitmapImage>, Exception> callback)
	    {
	        throw new NotImplementedException();
	    }

	    public void GetLogicTiles(Action<Dictionary<string, BitmapImage>, Exception> callback)
		{
		    throw new NotImplementedException();
        }
    }
}
