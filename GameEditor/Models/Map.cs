using System.Collections.Generic;
using System.Windows;
using Newtonsoft.Json;

namespace GameEditor.Models
{
    public class Map
    {
        public int Rows => TerrainSpriteGrid.GetLength(0);
        public int Columns => TerrainSpriteGrid.GetLength(1);

        public Dictionary<int, string> TerrainSpriteNameTable{ get; set; }
        public Dictionary<int, string> CreatureNameTable{ get; set; }
        public Dictionary<int, Point> CreatureSpawnTable{ get; set; }
        public int[,] TerrainSpriteGrid{ get; set; }
        public int[,] WalkableGrid{ get; set; }


        public string Name{ get; set; }

        public Map(int rows, int columns)
        {
            TerrainSpriteNameTable = new Dictionary<int, string>();
            TerrainSpriteGrid = new int[rows, columns];
            //			LogicTiles = new List<LogicTile>();
        }

        [JsonConstructor]
        public Map(int size) : this(size, size)
        { }

        public void Fill(int value)
        {
            for(var i = 0; i < Rows; i++)
            for(var j = 0; j < Columns; j++)
                TerrainSpriteGrid[ i, j ] = value;
        }
    }
}
