using System.Collections.Generic;
using UnityEngine;
using UI;
using Utility;
using ApplicationManagers;
using Map;
using GameManagers;

namespace MapEditor
{
    class AddObjectCommand : BaseCommand
    {
        private string _script;
        private Dictionary<int, int> newIdToOld = new Dictionary<int, int>();
        private Dictionary<int, int> oldIdToNew = new Dictionary<int, int>();

        public AddObjectCommand(List<MapScriptBaseObject> objs)
        {
            // Need to store previous parents and map to new parents ids
            var scriptObjects = new MapScriptObjects();
            foreach (var obj in objs)
            {
                int newId = ((MapEditorGameManager)SceneLoader.CurrentGameManager).GetNextObjectId();
                newIdToOld.Add(newId, obj.Id);
                oldIdToNew.Add(obj.Id, newId);
                obj.Id = newId;
                scriptObjects.Objects.Add(obj);
            }
            _script = scriptObjects.Serialize();
        }

        public override void Execute()
        {
            var scriptObjects = new MapScriptObjects();
            scriptObjects.Deserialize(_script);
            foreach (MapScriptBaseObject obj in scriptObjects.Objects)
            {
                if (obj.Parent != -1)
                {
                    if (oldIdToNew.ContainsKey(obj.Parent))
                    {
                        obj.Parent = oldIdToNew[obj.Parent];
                    }
                }
                MapLoader.LoadObject(obj, true);
            }
        }

        public override void Unexecute()
        {
            var scriptObjects = new MapScriptObjects();
            scriptObjects.Deserialize(_script);
            foreach (MapScriptBaseObject obj in scriptObjects.Objects)
                MapLoader.DeleteObject(obj.Id);
        }
    }
}
