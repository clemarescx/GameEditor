using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameEditor
{
	public class Map
	{
		public int Rows => TerrainGrid.GetLength(0);
		public int Columns => TerrainGrid.GetLength(1);

		public List<string> TerrainTileNamesInUse{ get; set; }

		public int[,] TerrainGrid{ get; set; }

		public List<LogicTile> LogicTiles{ get; set; }
		public string Name{ get; set; }

		public Map(int rows, int columns, string defaultTile)
		{
			TerrainTileNamesInUse = new List<string>{ defaultTile };
			TerrainGrid = new int[rows, columns];
			LogicTiles = new List<LogicTile>();
		}

		[JsonConstructor]
		public Map(int size, string defaultTile) : this(size, size, defaultTile)
		{ }

		public void Fill(int value)
		{
			for(var i = 0; i < Rows; i++)
			for(var j = 0; j < Columns; j++)
				TerrainGrid[ i, j ] = value;
		}
	}
}
