using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using GameEditor.Models;

namespace GameEditor.Messages
{
    public class AreamapsAvailableMessage : MessageBase
    {
        public List<AreaMap> AllMaps{ get; }

        public AreamapsAvailableMessage(List<AreaMap> maps)
        {
            AllMaps = maps;
        }
    }
}
