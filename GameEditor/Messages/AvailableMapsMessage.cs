using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using GameEditor.Models;

namespace GameEditor.Messages
{
    public class AvailableMapsMessage : MessageBase
    {
        public List<Map> AllMaps{ get; }

        public AvailableMapsMessage(List<Map> maps)
        {
            AllMaps = maps;
        }
    }
}
