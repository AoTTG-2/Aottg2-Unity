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
            if (name == "FormatFromList")
            {
                string str = (string)parameters[0];
                var list = (CustomLogicListBuiltin)parameters[1];
                List<string> strList = new List<string>();
                foreach (var obj in list.List)
                {
                    strList.Add((string)obj);
                }
                return string.Format(str, strList.ToArray());
            }
            if (name == "Split")
            {
                if (parameters.Count < 2 || parameters.Count > 3)
                    throw new System.Exception("Invalid number of parameters for Split (string, string, [OPT]removeEmpty), (string, list, [OPT]removeEmpty)");
                System.StringSplitOptions options = System.StringSplitOptions.None;

                if (parameters.Count == 3)
                {
                    options = (bool)parameters[2] ? System.StringSplitOptions.RemoveEmptyEntries : System.StringSplitOptions.None;
                }

                string stringToSplit = (string)parameters[0];
                CustomLogicListBuiltin list = new CustomLogicListBuiltin();
                if (parameters[1] is string)
                {
                    string separator = (string)parameters[1];
                    if (separator.Length == 1)
                    {
                        foreach (string str in stringToSplit.Split(separator[0], options))
                            list.List.Add(str);
                    }
                    else
                    {
                        foreach (string str in stringToSplit.Split(separator, options))
                            list.List.Add(str);
                    }
                }
                else
                {
                    CustomLogicListBuiltin separatorList = (CustomLogicListBuiltin)parameters[1];
                    string[] separators = new string[separatorList.List.Count];
                    for (int i = 0; i < separatorList.List.Count; i++)
                        separators[i] = (string)separatorList.List[i];
                    foreach (string str in stringToSplit.Split(separators, options))
                        list.List.Add(str);
                }
                return list;
            }
            if (name == "Join")
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
            if (name == "Substring")
            {
                string str = (string)parameters[0];
                int startIndex = (int)parameters[1];
                return str.Substring(startIndex);
            }
            if (name == "SubstringWithLength")
            {
                string str = (string)parameters[0];
                int startIndex = (int)parameters[1];
                int length = (int)parameters[2];
                return str.Substring(startIndex, length);
            }
            if (name == "Length")
            {
                string str = (string)parameters[0];
                return str.Length;
            }
            if (name == "Replace")
            {
                string str = (string)parameters[0];
                string replace = (string)parameters[1];
                string with = (string)parameters[2];
                return str.Replace(replace, with);
            }
            if (name == "Contains")
            {
                string str = (string)parameters[0];
                string contains = (string)parameters[1];
                return str.Contains(contains);
            }
            if (name == "StartsWith")
                return ((string)parameters[0]).StartsWith((string)parameters[1]);
            if (name == "EndsWith")
                return ((string)parameters[0]).EndsWith((string)parameters[1]);
            if (name == "Trim")
                return ((string)parameters[0]).Trim();
            if (name == "Insert")
                return ((string)parameters[0]).Insert((int)parameters[2], (string)parameters[1]);
            if (name == "Capitalize")
                return ((string)parameters[0]).UpperFirstLetter();
            if (name == "ToUpper")
                return ((string)parameters[0]).ToUpper();
            if (name == "ToLower")
                return ((string)parameters[0]).ToLower();
            if (name == "IndexOf")
                return ((string)parameters[0]).IndexOf((string)parameters[1]);
            return base.CallMethod(name, parameters);
        }

        public override object GetField(string name)
        {
            if (name == "Newline")
                return "\n";
            return base.GetField(name);
        }
    }
}
