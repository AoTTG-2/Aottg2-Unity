using System.Collections.Generic;
using UnityEngine;
using UI;
using Utility;
using ApplicationManagers;
using Map;
using GameManagers;

namespace MapEditor
{
    class TransformPositionRotationCommand: BaseCommand
    {
        private List<Vector3> _oldPositions = new List<Vector3>();
        private List<Vector3> _newPositions = new List<Vector3>();
        private List<Vector3> _oldRotations = new List<Vector3>();
        private List<Vector3> _newRotations = new List<Vector3>();
        private List<int> _ids = new List<int>();

        public TransformPositionRotationCommand(List<MapObject> mapObjects)
        {
            foreach (var mapObject in mapObjects)
            {
                _ids.Add(mapObject.ScriptObject.Id);
                _oldPositions.Add(mapObject.ScriptObject.GetPosition());
                _newPositions.Add(mapObject.GameObject.transform.localPosition);
                _oldRotations.Add(mapObject.ScriptObject.GetRotation());
                _newRotations.Add(mapObject.GameObject.transform.localRotation.eulerAngles);
            }
        }

        public override void Execute()
        {
            for (int i = 0; i < _ids.Count; i++)
            {
                MapLoader.IdToMapObject[_ids[i]].GameObject.transform.localPosition = _newPositions[i];
                MapLoader.IdToMapObject[_ids[i]].ScriptObject.SetPosition(_newPositions[i]);
                MapLoader.IdToMapObject[_ids[i]].GameObject.transform.localRotation = Quaternion.Euler(_newRotations[i]);
                MapLoader.IdToMapObject[_ids[i]].ScriptObject.SetRotation(_newRotations[i]);
            }
            
        }

        public override void Unexecute()
        {
            for (int i = 0; i < _ids.Count; i++)
            {
                MapLoader.IdToMapObject[_ids[i]].GameObject.transform.localPosition = _oldPositions[i];
                MapLoader.IdToMapObject[_ids[i]].ScriptObject.SetPosition(_oldPositions[i]);
                MapLoader.IdToMapObject[_ids[i]].GameObject.transform.localRotation = Quaternion.Euler(_oldRotations[i]);
                MapLoader.IdToMapObject[_ids[i]].ScriptObject.SetRotation(_oldRotations[i]);
            }
        }
    }
}
