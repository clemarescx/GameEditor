using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameEditor.Models
{
    public class Campaign
    {
        public string Name{ get; set; }
        public List<Map> Maps{ get; set; }
        public List<Character> Characters { get; set; }

        [JsonConstructor]
        public Campaign(string name)
        {
            Name = name;
            Maps = new List<Map>();
        }
    }
}
