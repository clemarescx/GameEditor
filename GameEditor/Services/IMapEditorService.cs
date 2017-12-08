using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using GameEditor.Models;

namespace GameEditor.Services
{
    public interface IMapEditorService
    {

        void LoadAreaMap(Action<AreaMap, Exception> callback);
        void SaveAreaMap(AreaMap map);
    }
}
