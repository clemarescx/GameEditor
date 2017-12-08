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
        public void LoadAreaMap(Action<AreaMap, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void SaveAreaMap(AreaMap map)
        {
            throw new NotImplementedException();
        }
    }
}
