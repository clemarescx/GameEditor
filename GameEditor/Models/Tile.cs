using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace GameEditor.Models
{
    public class Tile
    {
        public string SpriteName{ get; set; }
        public bool IsWalkable{ get; set; }
        public bool IsSpawnPoint{ get; set; }
        public string CharacterToSpawn{ get; set; }
        public bool IsTransitionSpot{ get; set; }
        public string DestinationAreaName{ get; set; }
    }
}
