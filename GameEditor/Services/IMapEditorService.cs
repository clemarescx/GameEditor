using System;
using System.Collections.Generic;
using GameEditor.Models;

namespace GameEditor.Services {
	public interface IMapEditorService
	{
		void GetTerrainTiles(Action<IEnumerable<Tile>, Exception> callback);
		void GetLogicTiles(Action<IEnumerable<Tile>, Exception> callback);
	}
}


