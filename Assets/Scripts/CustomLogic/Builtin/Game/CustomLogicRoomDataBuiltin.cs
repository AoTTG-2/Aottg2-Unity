using System.Collections.Generic;

namespace CustomLogic
{
    [CLType(Name = "RoomData", Static = true, Abstract = true, Description = "Store and retrieve room variables. Room data is cleared upon joining or creating a new lobby and does not reset between game rounds. Supports float, string, bool, and int types. Note that RoomData is local only and does not sync. You must use network messages to sync room variables.")]
    partial class CustomLogicRoomDataBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicRoomDataBuiltin(){}

        [CLMethod("Sets the property with given name to the object value. Valid value types are float, string, bool, and int.")]
        public static void SetProperty(
            [CLParam("The name of the property.")]
            string property,
            [CLParam("The value to set (must be float, string, bool, int, or null).", Type = "float|int|string|bool|null")]
            object value)
        {
            if (value is not (null or float or int or string or bool))
                throw new System.Exception("RoomData.SetProperty only supports null, float, int, string, or bool values.");

            CustomLogicManager.RoomData[property] = value;
        }

        [CLMethod("Gets the property with given name. If property does not exist, returns defaultValue.")]
        public static object GetProperty(
            [CLParam("The name of the property.")]
            string property,
            [CLParam("The default value to return if the property does not exist.")]
            object defaultValue)
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
