using GalaSoft.MvvmLight.Messaging;
using GameEditor.Models;

namespace GameEditor.Messages
{
    public class SaveMapMessage : MessageBase
    {
        public SaveMapMessage(AreaMap savedMap)
        {
            SavedMap = savedMap;

        }

        public AreaMap SavedMap{ get; set; }
    }
}
