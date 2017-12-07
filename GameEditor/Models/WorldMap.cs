using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameEditor.Models
{
    public class WorldMap
    {
        public string Name{ get; set; }
        public List<Map> Maps{ get; set; }

        [JsonConstructor]
        public WorldMap(string name)
        {
            Name = name;
        }
    }
}
