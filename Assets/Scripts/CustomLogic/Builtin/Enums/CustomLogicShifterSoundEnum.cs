using Characters;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of shifter sounds.
    /// </summary>
    [CLType(Name = "ShifterSoundEnum", Static = true, Abstract = true)]
    partial class CustomLogicShifterSoundEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicShifterSoundEnum() { }

        /// <summary>
        /// Thunder sound.
        /// </summary>
        [CLProperty]
        public static string Thunder => ShifterSounds.Thunder;
        /// <summary>
        /// ErenRoar sound.
        /// </summary>
        [CLProperty]
        public static string ErenRoar => ShifterSounds.ErenRoar;
        /// <summary>
        /// AnnieRoar sound.
        /// </summary>
        [CLProperty]
        public static string AnnieRoar => ShifterSounds.AnnieRoar;
        /// <summary>
        /// AnnieHurt sound.
        /// </summary>
        [CLProperty]
        public static string AnnieHurt => ShifterSounds.AnnieHurt;
        /// <summary>
        /// ColossalSteam1 sound.
        /// </summary>
        [CLProperty]
        public static string ColossalSteam1 => ShifterSounds.ColossalSteam1;
        /// <summary>
        /// ColossalSteam2 sound.
        /// </summary>
        [CLProperty]
        public static string ColossalSteam2 => ShifterSounds.ColossalSteam2;
    }
}
