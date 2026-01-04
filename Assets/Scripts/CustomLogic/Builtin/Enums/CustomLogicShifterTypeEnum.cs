namespace CustomLogic
{
    /// <summary>
    /// Enumeration of shifter types that can be spawned.
    /// </summary>
    [CLType(Name = "ShifterTypeEnum", Static = true, Abstract = true)]
    partial class CustomLogicShifterTypeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicShifterTypeEnum() { }

        /// <summary>
        /// Annie shifter type.
        /// </summary>
        [CLProperty]
        public static string Annie => "Annie";

        /// <summary>
        /// Armored shifter type.
        /// </summary>
        [CLProperty]
        public static string Armored => "Armored";

        /// <summary>
        /// Eren shifter type.
        /// </summary>
        [CLProperty]
        public static string Eren => "Eren";

        /// <summary>
        /// WallColossal shifter type.
        /// </summary>
        [CLProperty]
        public static string WallColossal => "WallColossal";
    }
}
