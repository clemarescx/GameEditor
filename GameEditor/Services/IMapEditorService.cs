using System;
using GameEditor.Models;

namespace GameEditor.Services
{
    public interface IMapEditorService
    {
        void LoadMap(Action<Map, Exception> callback);
        void SaveMap(Map map);
    }
}
