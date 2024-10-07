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

        public AddObjectCommand(List<MapScriptBaseObject> objs)
        {
            var scriptObjects = new MapScriptObjects();
            foreach (var obj in objs)
            {
                obj.Id = ((MapEditorGameManager)SceneLoader.CurrentGameManager).GetNextObjectId();
                scriptObjects.Objects.Add(obj);
            }
            _script = scriptObjects.Serialize();
        }

        public override void Execute()
        {
            var scriptObjects = new MapScriptObjects();
            scriptObjects.Deserialize(_script);
            foreach (MapScriptBaseObject obj in scriptObjects.Objects)
                MapLoader.LoadObject(obj, true);
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
