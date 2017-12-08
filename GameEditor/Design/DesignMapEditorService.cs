using System;
using GameEditor.Models;
using GameEditor.Services;

namespace GameEditor.Design
{
    /// <summary>
    ///     Design-time implementation of the MapEditorService.
    ///     Unused.
    /// </summary>
    public class DesignMapEditorService : IMapEditorService
    {
        public void LoadMap(Action<Map, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void SaveMap(Map map)
        {
            throw new NotImplementedException();
        }
    }
}
