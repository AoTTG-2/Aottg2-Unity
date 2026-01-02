using System;
using System.Collections.Generic;
using System.Diagnostics;
using Settings;
using UI;

namespace CustomLogic
{
    /// <code>
    /// # Register individual strings for different languages
    /// Locale.Set(Locale.EnglishLanguage, "welcome", "Welcome to the game!");
    /// Locale.Set(Locale.RussianLanguage, "welcome", "Добро пожаловать в игру!");
    /// Locale.Set(Locale.ChineseLanguage, "welcome", "你好");
    ///
    /// Game.Print("welcome: " + Locale.Get("welcome"));
    ///
    /// # Register multiple strings at once using a dictionary
    /// englishStrings = Dict();
    /// englishStrings.Set("hello", "Hello");
    /// englishStrings.Set("goodbye", "Goodbye");
    /// englishStrings.Set("score", "Score: {0}");
    /// Locale.RegisterLanguage(Locale.EnglishLanguage, englishStrings);
    ///
    /// russianStrings = Dict();
    /// russianStrings.Set("hello", "Привет");
    /// russianStrings.Set("goodbye", "Пока");
    /// russianStrings.Set("score", "Счет: {0}");
    /// Locale.RegisterLanguage(Locale.RussianLanguage, russianStrings);
    ///
    /// chineseStrings = Dict();
    /// chineseStrings.Set("hello", "你好");
    /// chineseStrings.Set("goodbye", "再见");
    /// chineseStrings.Set("score", "分数: {0}");
    /// Locale.RegisterLanguage(Locale.ChineseLanguage, chineseStrings);
    ///
    /// # Get localized strings (automatically uses current UI language)
    /// Game.Print("hello: " + Locale.Get("hello"));
    /// Game.Print("goodbye: " + Locale.Get("goodbye"));
    /// 
    /// params = List();
    /// params.Add(83);
    /// Game.Print("score: " + String.FormatFromList(Locale.Get("score"), params));
    ///
    /// # Register all strings from internal JSON files for a specific category
    /// # Use internal:// prefix to specify internal localization files
    /// Locale.RegisterLanguages("internal://BasicTutorialMap");
    /// 
    /// Game.Print("Name.Game: " + Locale.Get("Name.Game"));
    /// Game.Print("Name.Tutorial: " + Locale.Get("Name.Tutorial"));
    /// Game.Print("Stage.Introduction: " + Locale.Get("Stage.Introduction"));
    /// 
    /// # Register all strings from external JSON files
    /// # Without prefix, treats as external localization files
    /// # Path: Documents/Aottg2/CustomLocale/MyGameMode/English.json
    /// # JSON file structure:
    /// {
    ///     "Name": "English",  // Required header
    ///     "Hello": "World!",  // Localization values
    ///     "Foo.Bar": "Baz"    // Localization values
    /// }
    /// 
    /// Locale.RegisterLanguages("MyGameMode");
    /// 
    /// Game.Print("Hello: " + Locale.Get("Hello"));
    /// Game.Print("Foo.Bar: " + Locale.Get("Foo.Bar"));
    /// 
    /// # Registering non-existing or empty localization files directory will throw an exception
    /// Locale.RegisterLanguages("NonExistingGameMode");
    ///
    /// # Set default fallback language (Russian instead of English)
    /// Locale.DefaultLanguage = Locale.RussianLanguage;
    ///
    /// # Fallback to default for missing key in current language
    /// Locale.Set(Locale.RussianLanguage, "russian_key", "Сообщение");
    /// Game.Print("russian_key: " + Locale.Get("russian_key"));
    ///
    /// # Single-level (non-recursive) fallback: English -> German
    /// Locale.RegisterFallback(Locale.EnglishLanguage, Locale.GermanLanguage);
    /// Locale.Set(Locale.GermanLanguage, "german_string", "Hallo");
    /// Game.Print("german_string: " + Locale.Get("german_string"));
    ///
    /// # By default Traditional Chinese falls back to Simplified Chinese
    /// Locale.Set(Locale.ChineseLanguage, "chinese_string", "你好");
    /// Game.Print("chinese_string: " + Locale.Get("chinese_string"));
    ///
    /// # Clean up fallbacks
    /// Locale.RemoveFallback(Locale.ChineseLanguage);
    /// Locale.RemoveFallback(Locale.TraditionalChineseLanguage);
    ///
    /// # Missing key throws an exception
    /// Game.Print("missing_key: " + Locale.Get("missing_key"));
    /// </code>
    [CLType(Name = "Locale", Abstract = true, Static = true, Description = "Internationalization (Locale) utility for managing localized strings. Supports single-level (non-recursive) language fallbacks and automatic UI language detection.")]
    partial class CustomLogicLocaleBuiltin : BuiltinClassInstance
    {
        private static readonly Dictionary<string, Dictionary<string, string>> _languages = new Dictionary<string, Dictionary<string, string>>();
        private static readonly Dictionary<string, string> _languageFallbacks = new Dictionary<string, string>();

        [CLConstructor("Creates a new Locale instance.")]
        public CustomLogicLocaleBuiltin()
        {
            DefaultLanguage = EnglishLanguage;

            RegisterFallback(TraditionalChineseLanguage, ChineseLanguage);
            RegisterFallback(ChineseLanguage, TraditionalChineseLanguage);
        }

        [CLProperty("Arabic language code.")]
        public static string ArabicLanguage => UILanguages.Arabic;

        [CLProperty(Description = "Brazilian Portuguese language code")]
        public static string BrazilianPortugueseLanguage => UILanguages.BrazilianPortuguese;

        [CLProperty(Description = "Chinese language code")]
        public static string ChineseLanguage => UILanguages.Chinese;

        [CLProperty(Description = "Czech language code")]
        public static string CzechLanguage => UILanguages.Czech;

        [CLProperty(Description = "Dutch language code")]
        public static string DutchLanguage => UILanguages.Dutch;

        [CLProperty(Description = "English language code")]
        public static string EnglishLanguage => UILanguages.English;

        [CLProperty(Description = "French language code")]
        public static string FrenchLanguage => UILanguages.French;

        [CLProperty(Description = "German language code")]
        public static string GermanLanguage => UILanguages.German;

        [CLProperty(Description = "Greek language code")]
        public static string GreekLanguage => UILanguages.Greek;

        [CLProperty(Description = "Indonesian language code")]
        public static string IndonesianLanguage => UILanguages.Indonesian;

        [CLProperty(Description = "Italian language code")]
        public static string ItalianLanguage => UILanguages.Italian;

        [CLProperty(Description = "Japanese language code")]
        public static string JapaneseLanguage => UILanguages.Japanese;

        [CLProperty(Description = "Korean language code")]
        public static string KoreanLanguage => UILanguages.Korean;

        [CLProperty(Description = "Polish language code")]
        public static string PolishLanguage => UILanguages.Polish;

        [CLProperty(Description = "Russian language code")]
        public static string RussianLanguage => UILanguages.Russian;

        [CLProperty(Description = "Spanish language code")]
        public static string SpanishLanguage => UILanguages.Spanish;

        [CLProperty(Description = "Traditional Chinese language code")]
        public static string TraditionalChineseLanguage => UILanguages.TraditionalChinese;

        [CLProperty(Description = "Turkish language code")]
        public static string TurkishLanguage => UILanguages.Turkish;

        [CLProperty(Description = "Ukrainian language code")]
        public static string UkrainianLanguage => UILanguages.Ukrainian;

        [CLProperty("Returns the current language (e.g. \"English\" or \"简体中文\").")]
        public static string CurrentLanguage => SettingsManager.GeneralSettings.Language.Value;

        [CLProperty("The default language to use when a string is not found in the current language pack. English by default.")]
        public static string DefaultLanguage { get; set; }

        [CLMethod("Get the localized string for the given key. Searches the current UI language, then any registered fallbacks, and finally the default language. Throws an exception if the key is not found in any language pack.")]
        public static string Get(string key)
        {
            var currentLang = SettingsManager.GeneralSettings.Language.Value;

            var value = ResolveString(key, currentLang);
            if (value != null)
                return value;

            throw new Exception("Localized string not found: " + key);
        }

        [CLMethod("Set or override a localized string for the specified language and key.")]
        public static void Set(string language, string key, string value)
        {
            if (!_languages.TryGetValue(language, out var languagePack))
            {
                languagePack = new Dictionary<string, string>();
                _languages[language] = languagePack;
            }

            languagePack[key] = value;
        }

        [CLMethod("Register a single-level (non-recursive) fallback: if a string is not found in 'fromLanguage', the system will search only in 'toLanguage', without chaining further.")]
        public static void RegisterLanguage(string language, CustomLogicDictBuiltin strings)
        {
            var dictionary = new Dictionary<string, string>(strings.Count);
            foreach (var key in strings.Keys.List)
                dictionary[key.ToString()] = strings.Get(key).ToString();
            _languages[language] = dictionary;
        }

        [CLMethod("Register all localized strings from JSON files for a specific category across all available languages. Use 'internal://' prefix for internal files (e.g., 'internal://BasicTutorialMap') or no prefix for external files (e.g., 'MyCustomMod').")]
        public static void RegisterLanguages(string pattern)
        {
            foreach (var languagePair in UIManager.GetLocaleCategoryStrings(pattern))
                _languages[languagePair.Key] = languagePair.Value;
        }

        [CLMethod("Register a fallback language. When a string is not found in 'fromLanguage', it will try 'toLanguage'.")]
        public static void RegisterFallback(string fromLanguage, string toLanguage)
        {
            _languageFallbacks[fromLanguage] = toLanguage;
        }

        [CLMethod("Remove a language fallback.")]
        public static void RemoveFallback(string fromLanguage)
        {
            _languageFallbacks.Remove(fromLanguage);
        }

        private static string ResolveString(string key, string requestedLang)
        {
            if (_languages.TryGetValue(requestedLang, out var exactPack) && exactPack.TryGetValue(key, out var exactValue))
                return exactValue;

            if (_languageFallbacks.TryGetValue(requestedLang, out var fallbackLang)
                && _languages.TryGetValue(fallbackLang, out var fallbackPack)
                && fallbackPack.TryGetValue(key, out var fallbackValue))
                return fallbackValue;

            if (_languages.TryGetValue(DefaultLanguage, out var defaultPack)
                && defaultPack.TryGetValue(key, out var defaultValue))
                return defaultValue;

            throw new Exception("Locale string not found: " + key);
        }
    }
}
