using ApplicationManagers;
using GameManagers;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Static = true)]
    class CustomLogicRoomDataBuiltin : CustomLogicClassInstanceBuiltin
    {
        public CustomLogicRoomDataBuiltin() : base("RoomData")
        {
        }

        [CLMethod("Sets a property in the room data.")]
        public static void SetProperty(string property, object value)
        {
            if (!(value == null || value is float || value is int || value is string || value is bool))
                throw new System.Exception("RoomData.SetProperty only supports null, float, int, string, or bool values.");
            CustomLogicManager.RoomData[property] = value;
        }

        [CLMethod("Gets a property from the room data.")]
        public static object GetProperty(string property, object defaultValue)
        {
            if (CustomLogicManager.RoomData.ContainsKey(property))
                return CustomLogicManager.RoomData[property];
            return defaultValue;
        }

        [CLMethod("Clears all properties from the room data.")]
        public static void Clear()
        {
            CustomLogicManager.RoomData.Clear();
        }
    }
}