using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Documents;
using System.Xml.Serialization.Advanced;
using Newtonsoft.Json;

namespace GameEditor{
	public class Map{
		public int Rows => TerrainGrid.GetLength(0);
		public int Columns => TerrainGrid.GetLength(1);

		public List<string> TileNamesInUse{ get; set; }

		public int[,] TerrainGrid{ get; set; }

		public List<int> Triggers{ get; set; }
		public string Name{ get; set; }

		public Map(int rows, int columns, string defaultTile){
			TileNamesInUse = new List<string>{ defaultTile };
			TerrainGrid = new int[rows, columns];
			Triggers = new List<int>();

		}
		[JsonConstructor]
		public Map(int size, string defaultTile) : this(size, size, defaultTile){ }

//		public Map() : this(8, "sand_1.png") { }

		public void Fill(int value){
			for(int i = 0; i < Rows; i++)
			for(int j = 0; j < Columns; j++)
				TerrainGrid[i, j] = value;
		}
	}
}