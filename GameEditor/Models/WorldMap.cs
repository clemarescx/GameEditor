using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameEditor.Models
{
    public class WorldMap
    {
        public string Name{ get; set; }
        public List<AreaMap> Areas{ get; set; }

        [JsonConstructor]
        public WorldMap(string name)
        {
            Name = name;
            Areas = new List<AreaMap>();
        }
    }
}
