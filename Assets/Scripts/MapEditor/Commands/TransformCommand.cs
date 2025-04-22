using System.Collections.Generic;
using UnityEngine;
using UI;
using Utility;
using ApplicationManagers;
using Map;
using GameManagers;

namespace MapEditor
{
    struct STransform
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        public STransform(MapObject obj)
        {
            Position = obj.GameObject.transform.localPosition;
            Rotation = obj.GameObject.transform.localRotation.eulerAngles;
            Scale = Util.DivideVectors(obj.GameObject.transform.localScale, obj.BaseScale);

        }

        public STransform(MapScriptBaseObject so)
        {
            Position = so.GetPosition();
            Rotation = so.GetRotation();
            Scale = so.GetScale();
        }
    }

    class TransformCommand : InspectorCommand
    {
        private List<STransform> _oldTransforms = new List<STransform>();
        private List<STransform> _newTransforms = new List<STransform>();
        private List<int> _ids = new List<int>();

        public TransformCommand(List<MapObject> mapObjects)
        {
            foreach (var mapObject in mapObjects)
            {
                _ids.Add(mapObject.ScriptObject.Id);
                _oldTransforms.Add(new STransform(mapObject.ScriptObject));
                _newTransforms.Add(new STransform(mapObject));
            }
        }

        public void SetTransform(MapObject obj, STransform transform)
        {
            obj.GameObject.transform.localPosition = transform.Position;
            obj.ScriptObject.SetPosition(transform.Position);

            obj.GameObject.transform.localRotation = Quaternion.Euler(transform.Rotation);
            obj.ScriptObject.SetRotation(transform.Rotation);

            obj.GameObject.transform.localScale = Util.MultiplyVectors(obj.BaseScale, transform.Scale);
            obj.ScriptObject.SetScale(transform.Scale);
        }

        public override void Execute()
        {
            for (int i = 0; i < _ids.Count; i++)
            {
                SetTransform(MapLoader.IdToMapObject[_ids[i]], _newTransforms[i]);
            }

        }

        public override void Unexecute()
        {
            for (int i = 0; i < _ids.Count; i++)
            {
                SetTransform(MapLoader.IdToMapObject[_ids[i]], _oldTransforms[i]);
            }

        }
    }
}
