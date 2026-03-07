using UI;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of available language codes. Duplicates Locale language constants for consistency.
    /// </summary>
    [CLType(Name = "LanguageEnum", Static = true, Abstract = true)]
    partial class CustomLogicLanguageEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicLanguageEnum() { }

        /// <summary>
        /// Arabic language code.
        /// </summary>
        [CLProperty]
        public static string Arabic => UILanguages.Arabic;

        /// <summary>
        /// Brazilian Portuguese language code.
        /// </summary>
        [CLProperty]
        public static string BrazilianPortuguese => UILanguages.BrazilianPortuguese;

        /// <summary>
        /// Chinese language code.
        /// </summary>
        [CLProperty]
        public static string Chinese => UILanguages.Chinese;

        /// <summary>
        /// Czech language code.
        /// </summary>
        [CLProperty]
        public static string Czech => UILanguages.Czech;

        /// <summary>
        /// Dutch language code.
        /// </summary>
        [CLProperty]
        public static string Dutch => UILanguages.Dutch;

        /// <summary>
        /// English language code.
        /// </summary>
        [CLProperty]
        public static string English => UILanguages.English;

        /// <summary>
        /// French language code.
        /// </summary>
        [CLProperty]
        public static string French => UILanguages.French;

        /// <summary>
        /// German language code.
        /// </summary>
        [CLProperty]
        public static string German => UILanguages.German;

        /// <summary>
        /// Greek language code.
        /// </summary>
        [CLProperty]
        public static string Greek => UILanguages.Greek;

        /// <summary>
        /// Indonesian language code.
        /// </summary>
        [CLProperty]
        public static string Indonesian => UILanguages.Indonesian;

        /// <summary>
        /// Italian language code.
        /// </summary>
        [CLProperty]
        public static string Italian => UILanguages.Italian;

        /// <summary>
        /// Japanese language code.
        /// </summary>
        [CLProperty]
        public static string Japanese => UILanguages.Japanese;

        /// <summary>
        /// Korean language code.
        /// </summary>
        [CLProperty]
        public static string Korean => UILanguages.Korean;

        /// <summary>
        /// Polish language code.
        /// </summary>
        [CLProperty]
        public static string Polish => UILanguages.Polish;

        /// <summary>
        /// Russian language code.
        /// </summary>
        [CLProperty]
        public static string Russian => UILanguages.Russian;

        /// <summary>
        /// Spanish language code.
        /// </summary>
        [CLProperty]
        public static string Spanish => UILanguages.Spanish;

        /// <summary>
        /// Traditional Chinese language code.
        /// </summary>
        [CLProperty]
        public static string TraditionalChinese => UILanguages.TraditionalChinese;

        /// <summary>
        /// Turkish language code.
        /// </summary>
        [CLProperty]
        public static string Turkish => UILanguages.Turkish;

        /// <summary>
        /// Ukrainian language code.
        /// </summary>
        [CLProperty]
        public static string Ukrainian => UILanguages.Ukrainian;
    }
}
