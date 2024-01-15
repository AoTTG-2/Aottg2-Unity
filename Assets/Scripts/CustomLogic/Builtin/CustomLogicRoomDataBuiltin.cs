using ApplicationManagers;
using GameManagers;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicRoomDataBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicRoomDataBuiltin(): base("RoomData")
        {
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            if (name == "SetProperty")
            {
                string property = (string)parameters[0];
                object value = parameters[1];
                if (!(value == null || value is float || value is int || value is string || value is bool))
                    throw new System.Exception("RoomData.SetProperty only supports null, float, int, string, or bool values.");
                CustomLogicManager.RoomData[property] = value;
                return null;
            }
            if (name == "GetProperty")
            {
                string property = (string)parameters[0];
                object value = parameters[1];
                if (CustomLogicManager.RoomData.ContainsKey(property))
                    return CustomLogicManager.RoomData[property];
                return value;
            }
            if (name == "Clear")
            {
                CustomLogicManager.RoomData.Clear();
                return null;
            }
            return base.CallMethod(name, parameters);
        }
    }
}
