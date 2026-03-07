using System;
using System.Collections.Generic;
using Settings;
using UI;

namespace CustomLogic
{
    /// <summary>
    /// Internationalization (Locale) utility for managing localized strings.
    /// Supports single-level (non-recursive) language fallbacks and automatic UI language detection.
    /// </summary>
    /// <code>
    /// # Register individual strings for different languages
    /// Locale.Set(LanguageEnum.English, "welcome", "Welcome to the game!");
    /// Locale.Set(LanguageEnum.Russian, "welcome", "Добро пожаловать в игру!");
    /// Locale.Set(LanguageEnum.Chinese, "welcome", "你好");
    ///
    /// Game.Print("welcome: " + Locale.Get("welcome"));
    ///
    /// # Register multiple strings at once using a dictionary
    /// englishStrings = Dict();
    /// englishStrings.Set("hello", "Hello");
    /// englishStrings.Set("goodbye", "Goodbye");
    /// englishStrings.Set("score", "Score: {0}");
    /// Locale.RegisterLanguage(LanguageEnum.English, englishStrings);
    ///
    /// russianStrings = Dict();
    /// russianStrings.Set("hello", "Привет");
    /// russianStrings.Set("goodbye", "Пока");
    /// russianStrings.Set("score", "Счет: {0}");
    /// Locale.RegisterLanguage(LanguageEnum.Russian, russianStrings);
    ///
    /// chineseStrings = Dict();
    /// chineseStrings.Set("hello", "你好");
    /// chineseStrings.Set("goodbye", "再见");
    /// chineseStrings.Set("score", "分数: {0}");
    /// Locale.RegisterLanguage(LanguageEnum.Chinese, chineseStrings);
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
    /// Locale.DefaultLanguage = LanguageEnum.Russian;
    ///
    /// # Fallback to default for missing key in current language
    /// Locale.Set(LanguageEnum.Russian, "russian_key", "Сообщение");
    /// Game.Print("russian_key: " + Locale.Get("russian_key"));
    ///
    /// # Single-level (non-recursive) fallback: English -> German
    /// Locale.RegisterFallback(LanguageEnum.English, LanguageEnum.German);
    /// Locale.Set(LanguageEnum.German, "german_string", "Hallo");
    /// Game.Print("german_string: " + Locale.Get("german_string"));
    ///
    /// # By default Traditional Chinese falls back to Simplified Chinese
    /// Locale.Set(LanguageEnum.Chinese, "chinese_string", "你好");
    /// Game.Print("chinese_string: " + Locale.Get("chinese_string"));
    ///
    /// # Clean up fallbacks
    /// Locale.RemoveFallback(LanguageEnum.Chinese);
    /// Locale.RemoveFallback(LanguageEnum.TraditionalChinese);
    ///
    /// # Missing key throws an exception
    /// Game.Print("missing_key: " + Locale.Get("missing_key"));
    /// </code>
    [CLType(Name = "Locale", Abstract = true, Static = true)]
    partial class CustomLogicLocaleBuiltin : BuiltinClassInstance
    {
        private static readonly Dictionary<string, Dictionary<string, string>> _languages = new Dictionary<string, Dictionary<string, string>>();
        private static readonly Dictionary<string, string> _languageFallbacks = new Dictionary<string, string>();

        [CLConstructor]
        public CustomLogicLocaleBuiltin(){
            DefaultLanguage = CustomLogicLanguageEnum.English;

            RegisterFallback(CustomLogicLanguageEnum.TraditionalChinese, CustomLogicLanguageEnum.Chinese);
            RegisterFallback(CustomLogicLanguageEnum.Chinese, CustomLogicLanguageEnum.TraditionalChinese);
        }

        /// <summary>
        /// Returns the current language (e.g. "English" or "简体中文").
        /// </summary>
        [CLProperty]
        public static string CurrentLanguage => SettingsManager.GeneralSettings.Language.Value;

        /// <summary>
        /// The default language to use when a string is not found in the current language pack. English by default.
        /// </summary>
        [CLProperty(Enum = new Type[] { typeof(CustomLogicLanguageEnum) })]
        public static string DefaultLanguage { get; set; }

        /// <summary>
        /// Get the localized string for the given key. Searches the current UI language, then any registered fallbacks,
        /// and finally the default language. Throws an exception if the key is not found in any language pack.
        /// </summary>
        /// <param name="key">The key of the localized string to get.</param>
        /// <returns>The localized string.</returns>
        [CLMethod]
        public static string Get(string key)
        {
            var currentLang = SettingsManager.GeneralSettings.Language.Value;

            var value = ResolveString(key, currentLang);
            if (value != null)
                return value;

            throw new Exception("Localized string not found: " + key);
        }

        /// <summary>
        /// Set or override a localized string for the specified language and key.
        /// </summary>
        /// <param name="language">The language code.</param>
        /// <param name="key">The key of the localized string.</param>
        /// <param name="value">The localized string value.</param>
        [CLMethod]
        public static void Set(
            [CLParam(Enum = new Type[] { typeof(CustomLogicLanguageEnum) })] string language,
            string key,
            string value)
        {
            if (!_languages.TryGetValue(language, out var languagePack))
            {
                languagePack = new Dictionary<string, string>();
                _languages[language] = languagePack;
            }

            languagePack[key] = value;
        }

        /// <summary>
        /// Register a single-level (non-recursive) fallback: if a string is not found in 'fromLanguage',
        /// the system will search only in 'toLanguage', without chaining further.
        /// </summary>
        /// <param name="language">The language code to register.</param>
        /// <param name="strings">The dictionary containing key-value pairs of localized strings.</param>
        [CLMethod]
        public static void RegisterLanguage(
            [CLParam(Enum = new Type[] { typeof(CustomLogicLanguageEnum) })] string language,
            [CLParam(Type = "Dict<string, string>")] CustomLogicDictBuiltin strings)
        {
            var dictionary = new Dictionary<string, string>(strings.Count);
            foreach (var key in strings.Keys.List)
                dictionary[key.ToString()] = strings.Get(key).ToString();
            _languages[language] = dictionary;
        }

        /// <summary>
        /// Register all localized strings from JSON files for a specific category across all available languages.
        /// Use 'internal://' prefix for internal files (e.g., 'internal://BasicTutorialMap') or no prefix for external files (e.g., 'MyCustomMod').
        /// </summary>
        /// <param name="pattern">The category pattern. Use 'internal://' prefix for internal files or no prefix for external files.</param>
        [CLMethod]
        public static void RegisterLanguages(string pattern)
        {
            foreach (var languagePair in UIManager.GetLocaleCategoryStrings(pattern))
                _languages[languagePair.Key] = languagePair.Value;
        }

        /// <summary>
        /// Register a fallback language. When a string is not found in 'fromLanguage', it will try 'toLanguage'.
        /// </summary>
        /// <param name="fromLanguage">The language code that will fallback to another language.</param>
        /// <param name="toLanguage">The language code to fallback to.</param>
        [CLMethod]
        public static void RegisterFallback(
            [CLParam(Enum = new Type[] { typeof(CustomLogicLanguageEnum) })] string fromLanguage,
            [CLParam(Enum = new Type[] { typeof(CustomLogicLanguageEnum) })] string toLanguage)
        {
            _languageFallbacks[fromLanguage] = toLanguage;
        }

        /// <summary>
        /// Remove a language fallback.
        /// </summary>
        /// <param name="fromLanguage">The language code to remove the fallback for.</param>
        [CLMethod]
        public static void RemoveFallback([CLParam(Enum = new Type[] { typeof(CustomLogicLanguageEnum) })] string fromLanguage)
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
