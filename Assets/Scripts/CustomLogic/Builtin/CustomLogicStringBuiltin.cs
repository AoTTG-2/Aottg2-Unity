using ApplicationManagers;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicStringBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicStringBuiltin(): base("String")
        {
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            if (name == "FormatFloat")
                return Util.FormatFloat(parameters[0].UnboxToFloat(), (int)parameters[1]);
            else if (name == "Split")
            {
                string toSplit = (string)parameters[0];
                string splitStr = (string)parameters[1];
                char splitChar = splitStr[0];
                CustomLogicListBuiltin list = new CustomLogicListBuiltin();
                foreach (string str in toSplit.Split(splitChar))
                    list.List.Add(str);
                return list;
            }
            else if (name == "Join")
            {
                var list = (CustomLogicListBuiltin)parameters[0];
                string separator = (string)parameters[1];
                List<string> strList = new List<string>();
                foreach (var obj in list.List)
                {
                    strList.Add((string)obj);
                }
                return string.Join(separator, strList.ToArray());
            }
            else if (name == "Substring")
            {
                string str = (string)parameters[0];
                int startIndex = (int)parameters[1];
                return str.Substring(startIndex);
            }
            else if (name == "SubstringWithLength")
            {
                string str = (string)parameters[0];
                int startIndex = (int)parameters[1];
                int length = (int)parameters[2];
                return str.Substring(startIndex, length);
            }
            else if (name == "Length")
            {
                string str = (string)parameters[0];
                return str.Length;
            }
            else if (name == "Replace")
            {
                string str = (string)parameters[0];
                string replace = (string)parameters[1];
                string with = (string)parameters[2];
                return str.Replace(replace, with);
            }
            else if (name == "Contains")
            {
                string str = (string)parameters[0];
                string contains = (string)parameters[1];
                return str.Contains(contains);
            }
            else if (name == "StartsWith")
                return ((string)parameters[0]).StartsWith((string)parameters[1]);
            else if (name == "EndsWith")
                return ((string)parameters[0]).EndsWith((string)parameters[1]);
            else if (name == "Trim")
                return ((string)parameters[0]).Trim();
            else if (name == "Insert")
                return ((string)parameters[0]).Insert((int)parameters[2], (string)parameters[1]);
            return base.CallMethod(name, parameters);
        }

        public override object GetField(string name)
        {
            if (name == "Newline")
                return "\n";
            return null;
        }
    }
}
