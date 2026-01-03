namespace CustomLogic
{
    [CLType(Name = "TSKillSoundEnum", Static = true, Abstract = true, Description = "Enumeration of Thunderspear kill sound types for effect spawning.")]
    partial class CustomLogicTSKillSoundEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicTSKillSoundEnum() { }

        [CLProperty("Kill sound type.")]
        public static string Kill => "Kill";

        [CLProperty("Air sound type.")]
        public static string Air => "Air";

        [CLProperty("Ground sound type.")]
        public static string Ground => "Ground";

        [CLProperty("ArmorHit sound type.")]
        public static string ArmorHit => "ArmorHit";

        [CLProperty("CloseShot sound type.")]
        public static string CloseShot => "CloseShot";

        [CLProperty("MaxRangeShot sound type.")]
        public static string MaxRangeShot => "MaxRangeShot";
    }
}
