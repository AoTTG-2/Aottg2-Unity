using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of angle unit values for UI element transformations.
    /// </summary>
    [CLType(Name = "AngleUnitEnum", Static = true, Abstract = true)]
    partial class CustomLogicAngleUnitEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicAngleUnitEnum() { }

        /// <summary>
        /// Degree: angle unit in degrees (360 degrees = full circle).
        /// </summary>
        [CLProperty]
        public static int Degree => (int)AngleUnit.Degree;

        /// <summary>
        /// Gradian: angle unit in gradians (400 gradians = full circle).
        /// </summary>
        [CLProperty]
        public static int Gradian => (int)AngleUnit.Gradian;

        /// <summary>
        /// Radian: angle unit in radians (2π radians = full circle).
        /// </summary>
        [CLProperty]
        public static int Radian => (int)AngleUnit.Radian;

        /// <summary>
        /// Turn: angle unit in turns (1 turn = full circle).
        /// </summary>
        [CLProperty]
        public static int Turn => (int)AngleUnit.Turn;
    }
}
