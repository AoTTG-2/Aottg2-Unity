using System.Collections.Generic;

namespace CustomLogic
{
    /// <summary>
    /// Store and retrieve room variables. Room data is cleared upon joining or creating a new lobby
    /// and does not reset between game rounds. Supports float, string, bool, and int types.
    /// Note that RoomData is local only and does not sync. You must use network messages to sync room variables.
    /// </summary>
    [CLType(Name = "RoomData", Static = true, Abstract = true)]
    partial class CustomLogicRoomDataBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicRoomDataBuiltin(){}

        /// <summary>
        /// Sets the property with given name to the object value. Valid value types are float, string, bool, and int.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        /// <param name="value">The value to set (must be float, string, bool, int, or null).</param>
        [CLMethod]
        public static void SetProperty(string property, [CLParam(Type = "float|int|string|bool|null")] object value)
        {
            if (value is not (null or float or int or string or bool))
                throw new System.Exception("RoomData.SetProperty only supports null, float, int, string, or bool values.");

            CustomLogicManager.RoomData[property] = value;
        }

        /// <summary>
        /// Gets the property with given name. If property does not exist, returns defaultValue.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        /// <param name="defaultValue">The default value to return if the property does not exist.</param>
        /// <returns>The property value, or defaultValue if the property does not exist.</returns>
        [CLMethod]
        public static object GetProperty(string property, object defaultValue)
        {
            return CustomLogicManager.RoomData.GetValueOrDefault(property, defaultValue);
        }

        /// <summary>
        /// Clears all room data.
        /// </summary>
        [CLMethod]
        public static void Clear()
        {
            CustomLogicManager.RoomData.Clear();
        }
    }
}
