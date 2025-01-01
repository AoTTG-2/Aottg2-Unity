using System.Collections.Generic;

namespace CustomLogic
{
    [CLType(Name = "RoomData", Static = true, Abstract = true)]
    partial class CustomLogicRoomDataBuiltin : CustomLogicClassInstanceBuiltin
    {
        public CustomLogicRoomDataBuiltin() : base("RoomData") { }

        [CLMethod("Sets the property with given name to the object value. Valid value types are float, string, bool, and int.")]
        public static void SetProperty(string property, object value)
        {
            if (value is not (null or float or int or string or bool))
                throw new System.Exception("RoomData.SetProperty only supports null, float, int, string, or bool values.");

            CustomLogicManager.RoomData[property] = value;
        }

        [CLMethod("Gets the property with given name. If property does not exist, returns defaultValue.")]
        public static object GetProperty(string property, object defaultValue)
        {
            return CustomLogicManager.RoomData.GetValueOrDefault(property, defaultValue);
        }

        [CLMethod("Clears all room data.")]
        public static void Clear()
        {
            CustomLogicManager.RoomData.Clear();
        }
    }
}