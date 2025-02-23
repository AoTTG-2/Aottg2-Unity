using System.Collections.Generic;
using UnityEngine;
using UI;
using Utility;
using ApplicationManagers;
using Map;
using GameManagers;

namespace MapEditor
{
    class DeleteObjectCommand: BaseCommand
    {
        private string _script;

        public DeleteObjectCommand(List<MapObject> objs)
        {
            var scriptObjects = new MapScriptObjects();
            foreach (MapObject obj in objs)
            {
                scriptObjects.Objects.Add(obj.ScriptObject);
                if (MapLoader.IdToChildren.ContainsKey(obj.ScriptObject.Id))
                {
                    foreach (int child in MapLoader.IdToChildren[obj.ScriptObject.Id])
                        scriptObjects.Objects.Add(MapLoader.IdToMapObject[child].ScriptObject);
                }
            }
            _script = scriptObjects.Serialize();
        }

        public override void Execute()
        {
            var scriptObjects = new MapScriptObjects();
            scriptObjects.Deserialize(_script);
            foreach (MapScriptBaseObject obj in scriptObjects.Objects)
                MapLoader.DeleteObject(obj.Id);
        }

        public override void Unexecute()
        {
            var scriptObjects = new MapScriptObjects();
            scriptObjects.Deserialize(_script);
            foreach (MapScriptBaseObject obj in scriptObjects.Objects)
                MapLoader.LoadObject(obj, true);
        }
    }
}
