using System.Collections.Generic;
using UnityEngine;
using UI;
using Utility;
using ApplicationManagers;
using Map;
using GameManagers;

namespace MapEditor
{
    class TransformPositionCommand: BaseCommand
    {
        private List<Vector3> _oldPositions = new List<Vector3>();
        private List<Vector3> _newPositions = new List<Vector3>();
        private List<int> _ids = new List<int>();

        public TransformPositionCommand(List<MapObject> mapObjects)
        {
            foreach (var mapObject in mapObjects)
            {
                _ids.Add(mapObject.ScriptObject.Id);
                _oldPositions.Add(mapObject.ScriptObject.GetPosition());
                _newPositions.Add(mapObject.GameObject.transform.localPosition);
            }
        }

        public override void Execute()
        {
            for (int i = 0; i < _ids.Count; i++)
            {
                MapLoader.IdToMapObject[_ids[i]].GameObject.transform.localPosition = _newPositions[i];
                MapLoader.IdToMapObject[_ids[i]].ScriptObject.SetPosition(_newPositions[i]);
            }
            
        }

        public override void Unexecute()
        {
            for (int i = 0; i < _ids.Count; i++)
            {
                MapLoader.IdToMapObject[_ids[i]].GameObject.transform.localPosition = _oldPositions[i];
                MapLoader.IdToMapObject[_ids[i]].ScriptObject.SetPosition(_oldPositions[i]);
            }
            
        }
    }
}
