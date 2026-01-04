namespace CustomLogic
{
    /// <summary>
    /// Enumeration of Thunderspear kill sound types for effect spawning.
    /// </summary>
    [CLType(Name = "TSKillSoundEnum", Static = true, Abstract = true)]
    partial class CustomLogicTSKillSoundEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicTSKillSoundEnum() { }

        /// <summary>
        /// Kill sound type.
        /// </summary>
        [CLProperty]
        public static string Kill => "Kill";

        /// <summary>
        /// Air sound type.
        /// </summary>
        [CLProperty]
        public static string Air => "Air";

        /// <summary>
        /// Ground sound type.
        /// </summary>
        [CLProperty]
        public static string Ground => "Ground";

        /// <summary>
        /// ArmorHit sound type.
        /// </summary>
        [CLProperty]
        public static string ArmorHit => "ArmorHit";

        /// <summary>
        /// CloseShot sound type.
        /// </summary>
        [CLProperty]
        public static string CloseShot => "CloseShot";

        /// <summary>
        /// MaxRangeShot sound type.
        /// </summary>
        [CLProperty]
        public static string MaxRangeShot => "MaxRangeShot";
    }
}
