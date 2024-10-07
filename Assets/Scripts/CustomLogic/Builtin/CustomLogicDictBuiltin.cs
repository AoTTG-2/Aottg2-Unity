using System.Collections.Generic;
using System.Text;
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
                return null;
            }
            if (methodName == "Get")
            {
                var key = GetDictKey(parameters[0]);
                if (key != null)
                    return Dict[key];
                if (parameters.Count > 1)
                    return parameters[1];
                throw new System.Exception("No dict key found: " + parameters[0]);
            }
            if (methodName == "Set")
            {
                object key = parameters[0];
                var dictKey = GetDictKey(key);
                if (dictKey != null)
                    Dict[dictKey] = parameters[1];
                else
                    Dict.Add(key, parameters[1]);
                return null;
            }
            if (methodName == "Remove")
            {
                var dictKey = GetDictKey(parameters[0]);
                if (dictKey != null)
                    Dict.Remove(dictKey);
                return null;
            }
            if (methodName == "Contains")
            {
                return GetDictKey(parameters[0]) != null;
            }
            return base.CallMethod(methodName, parameters);
        }

        private object GetDictKey(object key)
        {
            foreach (var dictKey in Dict.Keys)
            {
                if (CustomLogicManager.Evaluator.CheckEquals(dictKey, key))
                    return dictKey;
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
            if (name == "Values")
            {
                CustomLogicListBuiltin list = new CustomLogicListBuiltin();
                list.List = new List<object>(Dict.Values);
                return list;
            }
            if (name == "Count")
                return Dict.Count;
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            base.SetField(name, value);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("{");

            var i = 0;
            foreach (var (key, value) in Dict)
            {
                Append(key);
                builder.Append(": ");
                Append(value);

                if (i != Dict.Count - 1)
                    builder.Append(", ");
                
                i++;
            }
            
            builder.Append("}");
            return builder.ToString();

            void Append(object obj)
            {
                if (obj is string str)
                {
                    builder.Append($"\"{str}\"");
                    return;
                }

                builder.Append(obj);
            }
        }
    }
}
