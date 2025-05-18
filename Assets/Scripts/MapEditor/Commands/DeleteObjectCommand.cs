using System.Collections.Generic;
using UnityEngine;
using UI;
using Utility;
using ApplicationManagers;
using Map;
using GameManagers;
using System.Linq;

namespace MapEditor
{
    class DeleteObjectCommand: BaseCommand
    {
        private string _script;

        public DeleteObjectCommand(List<MapObject> objs)
        {
            HashSet<MapObject> uniqueObjects = new HashSet<MapObject>();
            foreach (MapObject obj in objs)
            {
                uniqueObjects.Add(obj);
                if (MapLoader.IdToChildren.ContainsKey(obj.ScriptObject.Id))
                {
                    foreach (int child in MapLoader.IdToChildren[obj.ScriptObject.Id])
                        uniqueObjects.Add(MapLoader.IdToMapObject[child]);
                }
            }

            var scriptObjects = new MapScriptObjects();
            scriptObjects.Objects = uniqueObjects.OrderBy(x => x.Level).Select(x => x.ScriptObject).ToList(); // Need to delete from bottom up.
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
