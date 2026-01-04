namespace CustomLogic
{
    /// <summary>
    /// Enumeration of collision modes for colliders.
    /// </summary>
    [CLType(Name = "CollideModeEnum", Static = true, Abstract = true)]
    partial class CustomLogicCollideModeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicCollideModeEnum() { }

        /// <summary>
        /// Region collision mode.
        /// </summary>
        [CLProperty]
        public static string Region => "Region";

        /// <summary>
        /// Hitboxes collision mode.
        /// </summary>
        [CLProperty]
        public static string Hitboxes => "Hitboxes";
    }
}
