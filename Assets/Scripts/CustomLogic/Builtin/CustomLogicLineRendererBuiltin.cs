using ApplicationManagers;
using Map;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicLineRendererBuiltin: CustomLogicBaseBuiltin
    {
        public LineRenderer Value = null;

        public CustomLogicLineRendererBuiltin(LineRenderer value) : base("LineRenderer")
        {
            Value = value;
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            if (name == "CreateLineRenderer")
            {
                GameObject obj = new GameObject();
                var renderer = obj.AddComponent<LineRenderer>();
                renderer.material = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Map, "Materials/TransparentMaterial", true);
                renderer.material.color = Color.black;
                renderer.startWidth = 1f;
                renderer.endWidth = 1f;
                renderer.positionCount = 0;
                renderer.enabled = false;
                return new CustomLogicLineRendererBuiltin(renderer);
            }
            if (name == "GetPosition")
                return new CustomLogicVector3Builtin(Value.GetPosition((int)parameters[0]));
            if (name == "SetPosition")
            {
                Value.SetPosition((int)parameters[0], ((CustomLogicVector3Builtin)parameters[1]).Value);
                return null;
            }
            return base.CallMethod(name, parameters);
        }

        public override void SetField(string name, object value)
        {
            if (name == "StartWidth")
                Value.startWidth = (float)value;
            else if (name == "EndWidth")
                Value.endWidth = (float)value;
            else if (name == "LineColor")
                Value.material.color = ((CustomLogicColorBuiltin)value).Value.ToColor();
            else if (name == "PositionCount")
                Value.positionCount = (int)value;
            else if (name == "Enabled")
                Value.enabled = (bool)value;
            else
                base.SetField(name, value);
        }

        public override object GetField(string name)
        {
            if (name == "StartWidth")
                return Value.startWidth;
            if (name == "EndWidth")
                return Value.endWidth;
            if (name == "LineColor")
                return new CustomLogicColorBuiltin(new Color255(Value.material.color));
            if (name == "PositionCount")
                return Value.positionCount;
            if (name == "Enabled")
                return Value.enabled;
            return base.GetField(name);
        }
    }
}
