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
//        public Dictionary<int, string> CharacterNameTable{ get; set; }
//        public Dictionary<int, Point> CharacterSpawnTable{ get; set; }
//        public int[,] Grid{ get; set; }
        public Tile[,] Grid{ get; set; }

        public string Name{ get; set; }

        [JsonConstructor]
        public Map(string mapName= "newMap", string defaultSpriteName= "sand_1.png", int size=8 )
        {
            Name = mapName;
            Grid = new Tile[size, size];
            var defaultTile = new Tile{ SpriteName = defaultSpriteName };
            Fill(defaultTile);
        }

        public void Fill(Tile value)
        {
            for (var i = 0; i < Rows; i++)
            for (var j = 0; j < Columns; j++)
                Grid[i, j] = value;
        }
    }
}
