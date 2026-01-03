namespace CustomLogic
{
    [CLType(Name = "ShifterTypeEnum", Static = true, Abstract = true, Description = "Enumeration of shifter types that can be spawned.")]
    partial class CustomLogicShifterTypeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicShifterTypeEnum() { }

        [CLProperty("Annie shifter type.")]
        public static string Annie => "Annie";

        [CLProperty("Armored shifter type.")]
        public static string Armored => "Armored";

        [CLProperty("Eren shifter type.")]
        public static string Eren => "Eren";

        [CLProperty("WallColossal shifter type.")]
        public static string WallColossal => "WallColossal";
    }
}
