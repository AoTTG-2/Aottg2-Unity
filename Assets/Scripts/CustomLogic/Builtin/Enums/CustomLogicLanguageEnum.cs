using UI;

namespace CustomLogic
{
    [CLType(Name = "LanguageEnum", Static = true, Abstract = true, Description = "Enumeration of available language codes. Duplicates Locale language constants for consistency.")]
    partial class CustomLogicLanguageEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicLanguageEnum() { }

        [CLProperty("Arabic language code.")]
        public static string Arabic => UILanguages.Arabic;

        [CLProperty("Brazilian Portuguese language code.")]
        public static string BrazilianPortuguese => UILanguages.BrazilianPortuguese;

        [CLProperty("Chinese language code.")]
        public static string Chinese => UILanguages.Chinese;

        [CLProperty("Czech language code.")]
        public static string Czech => UILanguages.Czech;

        [CLProperty("Dutch language code.")]
        public static string Dutch => UILanguages.Dutch;

        [CLProperty("English language code.")]
        public static string English => UILanguages.English;

        [CLProperty("French language code.")]
        public static string French => UILanguages.French;

        [CLProperty("German language code.")]
        public static string German => UILanguages.German;

        [CLProperty("Greek language code.")]
        public static string Greek => UILanguages.Greek;

        [CLProperty("Indonesian language code.")]
        public static string Indonesian => UILanguages.Indonesian;

        [CLProperty("Italian language code.")]
        public static string Italian => UILanguages.Italian;

        [CLProperty("Japanese language code.")]
        public static string Japanese => UILanguages.Japanese;

        [CLProperty("Korean language code.")]
        public static string Korean => UILanguages.Korean;

        [CLProperty("Polish language code.")]
        public static string Polish => UILanguages.Polish;

        [CLProperty("Russian language code.")]
        public static string Russian => UILanguages.Russian;

        [CLProperty("Spanish language code.")]
        public static string Spanish => UILanguages.Spanish;

        [CLProperty("Traditional Chinese language code.")]
        public static string TraditionalChinese => UILanguages.TraditionalChinese;

        [CLProperty("Turkish language code.")]
        public static string Turkish => UILanguages.Turkish;

        [CLProperty("Ukrainian language code.")]
        public static string Ukrainian => UILanguages.Ukrainian;
    }
}
