using System.Collections.Generic;
using System.Windows;
using Newtonsoft.Json;

namespace GameEditor.Models
{
    public class Map
    {
        public int Rows => Grid.GetLength(0);
        public int Columns => Grid.GetLength(1);

//        public Dictionary<int, string> TerrainSpriteNameTable{ get; set; }
//        public Dictionary<int, string> CreatureNameTable{ get; set; }
//        public Dictionary<int, Point> CreatureSpawnTable{ get; set; }
//        public int[,] Grid{ get; set; }
        public Tile[,] Grid{ get; set; }

        public string Name{ get; set; }

        public Map(int rows, int columns, string name)
        {
//            TerrainSpriteNameTable = new Dictionary<int, string>();
//            Grid = new int[rows, columns];
            Grid = new Tile[rows, columns];
        }

        [JsonConstructor]
        public Map(int size, string name) : this(size, size, name)
        {
            Name = name;
        }

//        public void Fill(int value)
//        {
//            for(var i = 0; i < Rows; i++)
//            for(var j = 0; j < Columns; j++)
//                Grid[ i, j ] = value;
//        }

        public void Fill(Tile value)
        {
            for (var i = 0; i < Rows; i++)
            for (var j = 0; j < Columns; j++)
                Grid[i, j] = value;
        }
    }
}
