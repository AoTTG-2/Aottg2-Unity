using UnityEngine;
using Utility;
using SimpleJSONFixed;

namespace CustomLogic
{
    /// <summary>
    /// Serializes and deserializes primitive and struct values from and to json strings.
    /// Supports float, int, string, bool, Vector3, Quaternion, Color, Dict, and List.
    /// Dict and List must contain only the supported types, and can be nested.
    /// </summary>
    [CLType(Name = "Json", Static = true, Abstract = true)]
    partial class CustomLogicJsonBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicJsonBuiltin()
        {
        }

        [CLMethod(description: "Loads a json string into a custom logic object")]
        public static object LoadFromString(string json)
        {
            string jsonTrim = json.Trim();
            JSONNode jsonNode;
            try
            {
                jsonNode = JSON.Parse(json);
            }
            catch
            {
                jsonNode = new JSONString(jsonTrim);
            }
            try
            {
                return LoadJSON(jsonNode);
            }
            catch
            {
                return null;
            }

        }

        [CLMethod(description: "Saves a custom logic object into a json string")]
        public static string SaveToString(object obj)
        {
            JSONNode json = SaveJSON(obj);
            return json.ToString(aIndent: 4);
        }

        protected static object LoadJSON(JSONNode json)
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
                else if (type == "null")
                    return null;
                else if (type == "vector3")
                {
                    string[] raw = value.Substring(8).Split(',');
                    return new CustomLogicVector3Builtin(new Vector3(float.Parse(raw[0]), float.Parse(raw[1]), float.Parse(raw[2])));
                }
                else if (type == "quaternion")
                {
                    string[] raw = value.Substring(11).Split(',');
                    return new CustomLogicQuaternionBuiltin(new Quaternion(float.Parse(raw[0]), float.Parse(raw[1]), float.Parse(raw[2]), float.Parse(raw[3])));
                }
                else if (type == "color")
                {
                    string[] raw = value.Substring(6).Split(',');
                    return new CustomLogicColorBuiltin(new Color255(int.Parse(raw[0]), int.Parse(raw[1]), int.Parse(raw[2]), int.Parse(raw[3])));
                }
            }
            throw new System.Exception("Loading invalid json format.");
        }

        protected static JSONNode SaveJSON(object obj)
        {
            if (obj == null)
                return new JSONString("null:null");
            else if (obj is CustomLogicDictBuiltin)
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
            else if (obj is CustomLogicVector3Builtin)
            {
                var vector = ((CustomLogicVector3Builtin)obj).Value;
                return new JSONString("vector3:" + vector.x.ToString() + "," + vector.y.ToString() + "," + vector.z.ToString());
            }
            else if (obj is CustomLogicColorBuiltin)
            {
                var color = ((CustomLogicColorBuiltin)obj).Value;
                return new JSONString("color:" + color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString() + "," + color.A.ToString());
            }
            else if (obj is CustomLogicQuaternionBuiltin)
            {
                var quat = ((CustomLogicQuaternionBuiltin)obj).Value;
                return new JSONString("quaternion:" + quat.x.ToString() + "," + quat.y.ToString() + "," + quat.z.ToString() + "," + quat.w.ToString());
            }
            else
                throw new System.Exception("Saving invalid json type: only list, dict, structs, and primitives allowed, got " + obj.GetType().ToString());
        }
    }
}
