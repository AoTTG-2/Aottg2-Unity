namespace CustomLogic
{
    [CLType(Name = "CollideModeEnum", Static = true, Abstract = true, Description = "Enumeration of collision modes for colliders.")]
    partial class CustomLogicCollideModeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicCollideModeEnum() { }

        [CLProperty("Region collision mode.")]
        public static string Region => "Region";

        [CLProperty("Hitboxes collision mode.")]
        public static string Hitboxes => "Hitboxes";
    }
}
