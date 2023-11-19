using System.Collections.Generic;
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
            }
            else if (methodName == "Get")
            {
                int index = (int)parameters[0];
                return List[index];
            }
            else if (methodName == "Set")
            {
                int index = (int)parameters[0];
                List[index] = parameters[1];
            }
            else if (methodName == "Add")
            {
                List.Add(parameters[0]);
            }
            else if (methodName == "InsertAt")
            {
                int index = (int)parameters[0];
                List.Insert(index, parameters[1]);
            }
            else if (methodName == "RemoveAt")
            {
                List.RemoveAt((int)parameters[0]);
            }
            else if (methodName == "Remove")
            {
                List.Remove(parameters[0]);
            }
            return null;
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
    }
}
