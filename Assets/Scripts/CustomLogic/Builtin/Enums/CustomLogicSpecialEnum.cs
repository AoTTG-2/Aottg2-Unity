using GameManagers;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of special abilities for humans.
    /// </summary>
    [CLType(Name = "SpecialEnum", Static = true, Abstract = true)]
    partial class CustomLogicSpecialEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicSpecialEnum() { }

        /// <summary>
        /// Potato special ability.
        /// </summary>
        [CLProperty]
        public static string Potato => HumanSpecial.Potato;

        /// <summary>
        /// Escape special ability.
        /// </summary>
        [CLProperty]
        public static string Escape => HumanSpecial.Escape;

        /// <summary>
        /// Dance special ability.
        /// </summary>
        [CLProperty]
        public static string Dance => HumanSpecial.Dance;

        /// <summary>
        /// Distract special ability.
        /// </summary>
        [CLProperty]
        public static string Distract => HumanSpecial.Distract;

        /// <summary>
        /// Smell special ability.
        /// </summary>
        [CLProperty]
        public static string Smell => HumanSpecial.Smell;

        /// <summary>
        /// Supply special ability.
        /// </summary>
        [CLProperty]
        public static string Supply => HumanSpecial.Supply;

        /// <summary>
        /// SmokeBomb special ability.
        /// </summary>
        [CLProperty]
        public static string SmokeBomb => HumanSpecial.SmokeBomb;

        /// <summary>
        /// Carry special ability.
        /// </summary>
        [CLProperty]
        public static string Carry => HumanSpecial.Carry;

        /// <summary>
        /// Switchback special ability.
        /// </summary>
        [CLProperty]
        public static string Switchback => HumanSpecial.Switchback;

        /// <summary>
        /// Confuse special ability.
        /// </summary>
        [CLProperty]
        public static string Confuse => HumanSpecial.Confuse;

        /// <summary>
        /// DownStrike special ability (Blade only).
        /// </summary>
        [CLProperty]
        public static string DownStrike => HumanSpecial.DownStrike;

        /// <summary>
        /// Spin1 special ability (Blade only).
        /// </summary>
        [CLProperty]
        public static string Spin1 => HumanSpecial.Spin1;

        /// <summary>
        /// Spin2 special ability (Blade only).
        /// </summary>
        [CLProperty]
        public static string Spin2 => HumanSpecial.Spin2;

        /// <summary>
        /// Spin3 special ability (Blade only).
        /// </summary>
        [CLProperty]
        public static string Spin3 => HumanSpecial.Spin3;

        /// <summary>
        /// BladeThrow special ability (Blade only).
        /// </summary>
        [CLProperty]
        public static string BladeThrow => HumanSpecial.BladeThrow;

        /// <summary>
        /// AHSSTwinShot special ability (AHSS only).
        /// </summary>
        [CLProperty]
        public static string AHSSTwinShot => HumanSpecial.AHSSTwinShot;

        /// <summary>
        /// Stock special ability (Thunderspear only).
        /// </summary>
        [CLProperty]
        public static string Stock => HumanSpecial.Stock;

        /// <summary>
        /// None special ability (no special).
        /// </summary>
        [CLProperty]
        public static string None => HumanSpecial.None;

        /// <summary>
        /// Eren shifter transformation special ability.
        /// </summary>
        [CLProperty]
        public static string Eren => HumanSpecial.Eren;

        /// <summary>
        /// Annie shifter transformation special ability.
        /// </summary>
        [CLProperty]
        public static string Annie => HumanSpecial.Annie;

        /// <summary>
        /// Armored shifter transformation special ability.
        /// </summary>
        [CLProperty]
        public static string Armored => HumanSpecial.Armored;
    }
}
