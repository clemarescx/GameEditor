using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using GameEditor.Models;

namespace GameEditor.Services
{
    public interface IWorldEditorService
    {
        void LoadWorld(Action<WorldMap, Exception> callback);
        void SaveWorld(WorldMap map);
    }
}
