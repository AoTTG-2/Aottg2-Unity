using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicListBuiltin: CustomLogicBaseBuiltin
    {
        public List<object> List = new List<object>();

        public CustomLogicListBuiltin(string type): base(type)
        {
        }

        public CustomLogicListBuiltin(): base("List")
        {
        }

        public override object CallMethod(string methodName, List<object> parameters)
        {
            if (methodName == "Clear")
            {
                List.Clear();
                return null;
            }
            if (methodName == "Get")
            {
                int index = (int)parameters[0];
                return List[index];
            }
            if (methodName == "Set")
            {
                int index = (int)parameters[0];
                List[index] = parameters[1];
                return null;
            }
            if (methodName == "Add")
            {
                List.Add(parameters[0]);
                return null;
            }
            if (methodName == "InsertAt")
            {
                int index = (int)parameters[0];
                List.Insert(index, parameters[1]);
                return null;
            }
            if (methodName == "RemoveAt")
            {
                List.RemoveAt((int)parameters[0]);
                return null;
            }
            if (methodName == "Remove")
            {
                List.Remove(parameters[0]);
                return null;
            }
            if (methodName == "Contains")
            {
                return List.Any(e => e == parameters[0]);
            }
            if (methodName == "Sort")
            {
                List.Sort();
                return null;
            }
            return base.CallMethod(methodName, parameters);
        }

        public override object GetField(string name)
        {
            if (name == "Count")
                return List.Count;
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            base.SetField(name, value);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[");

            for (var i = 0; i < List.Count; i++)
            {
                if (List[i] is string str)
                    builder.Append($"\"{str}\"");
                else
                    builder.Append(List[i]);

                if (i != List.Count - 1)
                    builder.Append(", ");
            }

            builder.Append("]");
            return builder.ToString();
        }
    }
}
