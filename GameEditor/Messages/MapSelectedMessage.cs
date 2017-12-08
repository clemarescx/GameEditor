using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using GameEditor.Models;

namespace GameEditor.Messages
{
    public class MapSelectedMessage : MessageBase
    {
        public Map SelectedMap{ get; }

        public MapSelectedMessage(Map selectedMap)
        {
            SelectedMap = selectedMap;
        }
    }
}
