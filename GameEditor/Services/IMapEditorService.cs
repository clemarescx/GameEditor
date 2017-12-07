using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace GameEditor.Services
{
    public interface IMapEditorService
    {
        void GetTerrainTiles(Action<Dictionary<string, BitmapImage>, Exception> callback);
        void GetLogicTiles(Action<Dictionary<string, BitmapImage>, Exception> callback);
    }
}
