using System.Windows.Media.Imaging;

namespace GameEditor.Models
{
    public class Tile
    {
        public string SpriteName{ get; set; }
        public bool IsWalkable{ get; set; }
        public bool IsSpawnPoint{ get; set; }
        public string CreatureToSpawn{ get; set; }
        public bool IsTransitionSpot{ get; set; }
        public string DestinationAreaName{ get; set; }
    }
}
