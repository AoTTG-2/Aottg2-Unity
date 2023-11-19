using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicDictBuiltin: CustomLogicBaseBuiltin
    {
        public Dictionary<object, object> Dict = new Dictionary<object, object>();

        public CustomLogicDictBuiltin(): base("Dict")
        {
        }

        public override object CallMethod(string methodName, List<object> parameters)
        {
            if (methodName == "Clear")
            {
                Dict.Clear();
            }
            else if (methodName == "Get")
            {
                return Dict[parameters[0]];
            }
            else if (methodName == "Set")
            {
                object key = parameters[0];
                if (Dict.ContainsKey(key))
                    Dict[key] = parameters[1];
                else
                    Dict.Add(key, parameters[1]);
            }
            else if (methodName == "Remove")
            {
                Dict.Remove(parameters[0]);
            }
            else if (methodName == "Contains")
            {
                return Dict.ContainsKey(parameters[0]);
            }
            return null;
        }

        public override object GetField(string name)
        {
            if (name == "Keys")
            {
                CustomLogicListBuiltin list = new CustomLogicListBuiltin();
                list.List = new List<object>(Dict.Keys);
                return list;
            }
            else if (name == "Values")
            {
                CustomLogicListBuiltin list = new CustomLogicListBuiltin();
                list.List = new List<object>(Dict.Values);
                return list;
            }
            else if (name == "Count")
                return Dict.Count;
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            base.SetField(name, value);
        }
    }
}
