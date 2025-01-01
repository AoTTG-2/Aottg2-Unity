using Map;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicComponentInstance : CustomLogicClassInstance
    {
        public CustomLogicMapObjectBuiltin MapObject;
        public CustomLogicNetworkViewBuiltin NetworkView;
        private MapScriptComponent _script;

        public CustomLogicComponentInstance(string name, MapObject obj, MapScriptComponent script,
            CustomLogicNetworkViewBuiltin networkView) : base(name)
        {
            ClassName = name;
            MapObject = new CustomLogicMapObjectBuiltin(obj);
            _script = script;
            NetworkView = networkView;
        }

        public void LoadVariables()
        {
            Variables.Add("MapObject", MapObject);
            if (NetworkView != null)
                Variables.Add("NetworkView", NetworkView);
            foreach (string param in _script.Parameters)
            {
                var arr = param.Split(':');
                string name = arr[0];
                string value = arr[1];
                if (Variables.ContainsKey(name))
                {
                    Variables[name] = DeserializeValue(Variables[name], value);
                }
            }
        }

        public bool UsesCollider()
        {
            var eval = CustomLogicManager.Evaluator;
            return eval.HasMethod(this, "OnCollisionStay") || eval.HasMethod(this, "OnCollisionEnter") || eval.HasMethod(this, "OnCollisionExit") || eval.HasMethod(this, "OnGetHit");
        }

        public void OnCollisionStay(CustomLogicClassInstanceBuiltin other)
        {
            if (!Enabled)
                return;

            CustomLogicManager.Evaluator?.EvaluateMethod(this, "OnCollisionStay", new object[] { other });
        }

        public void OnCollisionEnter(CustomLogicClassInstanceBuiltin other)
        {
            if (!Enabled)
                return;

            CustomLogicManager.Evaluator.EvaluateMethod(this, "OnCollisionEnter", new object[] { other });
        }

        public void OnCollisionExit(CustomLogicClassInstanceBuiltin other)
        {
            if (!Enabled)
                return;

            CustomLogicManager.Evaluator.EvaluateMethod(this, "OnCollisionExit", new object[] { other });
        }

        public void OnGetHit(CustomLogicCharacterBuiltin character, string name, int damage, string type, Vector3 position)
        {
            if (!Enabled)
                return;

            CustomLogicManager.Evaluator.EvaluateMethod(this, "OnGetHit", new object[] { character, name, damage, type, position });
        }

        public void OnGetHooked(CustomLogicHumanBuiltin human, CustomLogicVector3Builtin position, bool left)
        {
            if (!Enabled)
                return;

            CustomLogicManager.Evaluator.EvaluateMethod(this, "OnGetHooked", new object[] { human, position, left });
        }

        public static object DeserializeValue(object obj, string value)
        {
            if (obj is not string && value == "null")
                return null;
            if (obj is int)
                return int.Parse(value);
            if (obj is float)
                return float.Parse(value);
            if (obj is string)
                return value;
            if (obj is bool)
                return value == "true";
            if (obj is CustomLogicColorBuiltin)
            {
                string[] strArr = value.Split('/');
                return new CustomLogicColorBuiltin(new Color255(int.Parse(strArr[0]), int.Parse(strArr[1]),
                    int.Parse(strArr[2]), int.Parse(strArr[3])));
            }
            if (obj is CustomLogicVector3Builtin)
            {
                string[] strArr = value.Split('/');
                return new CustomLogicVector3Builtin(new Vector3(float.Parse(strArr[0]), float.Parse(strArr[1]),
                    float.Parse(strArr[2])));
            }
            else if (obj is CustomLogicDictBuiltin)
            {
                return new CustomLogicDictBuiltin();
            }
            else if (obj is CustomLogicListBuiltin)
            {
                return new CustomLogicListBuiltin();
            }
            return null;
        }
    }
}
