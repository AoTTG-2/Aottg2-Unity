using System.Collections.Generic;
using UnityEngine;
using Utility;
using SimpleJSONFixed;

namespace CustomLogic
{
    class CustomLogicJsonBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicJsonBuiltin() : base("Json")
        {
        }

        public override object CallMethod(string methodName, List<object> parameters)
        {
            if (methodName == "LoadFromString")
            {
                var json = JSON.Parse((string)parameters[0]);
                if (json.IsArray || json.IsObject)
                    return LoadJSON(json);
                throw new System.Exception("Loading invalid json string.");
            }
            if (methodName == "SaveToString")
            {
                var json = SaveJSON(parameters[0]);
                return json.ToString(aIndent: 4);
            }
            return base.CallMethod(methodName, parameters);
        }

        protected object LoadJSON(JSONNode json)
        {
            if (json.IsArray)
            {
                var list = new CustomLogicListBuiltin();
                foreach (var node in json.Values)
                    list.List.Add(LoadJSON(node));
                return list;
            }
            else if (json.IsObject)
            {
                var dict = new CustomLogicDictBuiltin();
                foreach (string key in json.Keys)
                {
                    var node = json[key];
                    dict.Dict.Add(key, LoadJSON(node));
                }
                return dict;
            }
            else if (json.IsString)
            {
                string value = json.Value;
                string type = value.Split(':')[0];
                if (type == "float")
                    return float.Parse(value.Substring(6));
                else if (type == "int")
                    return int.Parse(value.Substring(4));
                else if (type == "string")
                    return value.Substring(7);
                else if (type == "bool")
                    return value.Substring(5) == "1";
            }
            throw new System.Exception("Loading invalid json format.");
        }

        protected JSONNode SaveJSON(object obj)
        {
            if (obj is CustomLogicDictBuiltin)
            {
                var node = new JSONObject();
                var dict = (CustomLogicDictBuiltin)obj;
                foreach (object key in dict.Dict.Keys)
                {
                    if (!(key is string))
                        throw new System.Exception("Saving invalid json type: dict must have string keys.");
                    node.Add((string)key, SaveJSON(dict.Dict[key]));
                }
                return node;
            }
            else if (obj is CustomLogicListBuiltin)
            {
                var node = new JSONArray();
                var list = (CustomLogicListBuiltin)obj;
                foreach (object item in list.List)
                    node.Add(SaveJSON(item));
                return node;
            }
            else if (obj is string)
                return new JSONString("string:" + ((string)obj));
            else if (obj is float)
                return new JSONString("float:" + ((float)obj).ToString());
            else if (obj is int)
                return new JSONString("int:" + ((int)obj).ToString());
            else if (obj is bool)
                return new JSONString("bool:" + (((bool)(obj)) == true ? "1" : "0"));
            else
                throw new System.Exception("Saving invalid json type: only list, dict, and primitives allowed.");
        }
    }
}
