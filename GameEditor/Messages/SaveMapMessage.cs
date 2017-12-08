using GalaSoft.MvvmLight.Messaging;
using GameEditor.Models;

namespace GameEditor.Messages
{
    public class SaveMapMessage : MessageBase
    {
        /// <summary>
        /// Container for transferring areamap data through MvvmLight messenging
        /// </summary>
        /// <param name="savedMap"></param>
        public SaveMapMessage(Map savedMap)
        {
            SavedMap = savedMap;

        }

        public Map SavedMap{ get; set; }
    }
}
