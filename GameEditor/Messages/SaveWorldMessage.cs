using GalaSoft.MvvmLight.Messaging;
using GameEditor.Models;

namespace GameEditor.Messages
{
    public class SaveWorldMessage : MessageBase
    {
        public SaveWorldMessage(WorldMap savedWorldMap)
        {
            SavedWorldMap = savedWorldMap;

        }

        public WorldMap SavedWorldMap{ get; set; }
    }
}
