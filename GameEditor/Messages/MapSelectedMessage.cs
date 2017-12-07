using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using GameEditor.Models;

namespace GameEditor.Messages
{
    public class MapSelectedMessage : MessageBase
    {
        public AreaMap SelectedMap{ get; }

        public MapSelectedMessage(AreaMap selectedMap)
        {
            SelectedMap = selectedMap;
        }
    }
}
