# Locale
Inherits from [Object](../objects/Object.md)

Internationalization (Locale) utility for managing localized strings.
Supports single-level (non-recursive) language fallbacks and automatic UI language detection.

### Example
```csharp
# Register individual strings for different languages
Locale.Set(Locale.EnglishLanguage, "welcome", "Welcome to the game!");
Locale.Set(Locale.RussianLanguage, "welcome", "Добро пожаловать в игру!");
Locale.Set(Locale.ChineseLanguage, "welcome", "你好");

Game.Print("welcome: " + Locale.Get("welcome"));

# Register multiple strings at once using a dictionary
englishStrings = Dict();
englishStrings.Set("hello", "Hello");
englishStrings.Set("goodbye", "Goodbye");
englishStrings.Set("score", "Score: {0}");
Locale.RegisterLanguage(Locale.EnglishLanguage, englishStrings);

russianStrings = Dict();
russianStrings.Set("hello", "Привет");
russianStrings.Set("goodbye", "Пока");
russianStrings.Set("score", "Счет: {0}");
Locale.RegisterLanguage(Locale.RussianLanguage, russianStrings);

chineseStrings = Dict();
chineseStrings.Set("hello", "你好");
chineseStrings.Set("goodbye", "再见");
chineseStrings.Set("score", "分数: {0}");
Locale.RegisterLanguage(Locale.ChineseLanguage, chineseStrings);

# Get localized strings (automatically uses current UI language)
Game.Print("hello: " + Locale.Get("hello"));
Game.Print("goodbye: " + Locale.Get("goodbye"));

params = List();
params.Add(83);
Game.Print("score: " + String.FormatFromList(Locale.Get("score"), params));

# Register all strings from internal JSON files for a specific category
# Use internal:// prefix to specify internal localization files
Locale.RegisterLanguages("internal://BasicTutorialMap");

Game.Print("Name.Game: " + Locale.Get("Name.Game"));
Game.Print("Name.Tutorial: " + Locale.Get("Name.Tutorial"));
Game.Print("Stage.Introduction: " + Locale.Get("Stage.Introduction"));

# Register all strings from external JSON files
# Without prefix, treats as external localization files
# Path: Documents/Aottg2/CustomLocale/MyGameMode/English.json
# JSON file structure:
{
    "Name": "English",  // Required header
    "Hello": "World!",  // Localization values
    "Foo.Bar": "Baz"    // Localization values
}

Locale.RegisterLanguages("MyGameMode");

Game.Print("Hello: " + Locale.Get("Hello"));
Game.Print("Foo.Bar: " + Locale.Get("Foo.Bar"));

# Registering non-existing or empty localization files directory will throw an exception
Locale.RegisterLanguages("NonExistingGameMode");

# Set default fallback language (Russian instead of English)
Locale.DefaultLanguage = Locale.RussianLanguage;

# Fallback to default for missing key in current language
Locale.Set(Locale.RussianLanguage, "russian_key", "Сообщение");
Game.Print("russian_key: " + Locale.Get("russian_key"));

# Single-level (non-recursive) fallback: English -> German
Locale.RegisterFallback(Locale.EnglishLanguage, Locale.GermanLanguage);
Locale.Set(Locale.GermanLanguage, "german_string", "Hallo");
Game.Print("german_string: " + Locale.Get("german_string"));

# By default Traditional Chinese falls back to Simplified Chinese
Locale.Set(Locale.ChineseLanguage, "chinese_string", "你好");
Game.Print("chinese_string: " + Locale.Get("chinese_string"));

# Clean up fallbacks
Locale.RemoveFallback(Locale.ChineseLanguage);
Locale.RemoveFallback(Locale.TraditionalChineseLanguage);

# Missing key throws an exception
Game.Print("missing_key: " + Locale.Get("missing_key"));
```
### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|ArabicLanguage|string|True|Arabic language code|
|BrazilianPortugueseLanguage|string|True|Brazilian Portuguese language code|
|ChineseLanguage|string|True|Chinese language code|
|CzechLanguage|string|True|Czech language code|
|DutchLanguage|string|True|Dutch language code|
|EnglishLanguage|string|True|English language code|
|FrenchLanguage|string|True|French language code|
|GermanLanguage|string|True|German language code|
|GreekLanguage|string|True|Greek language code|
|IndonesianLanguage|string|True|Indonesian language code|
|ItalianLanguage|string|True|Italian language code|
|JapaneseLanguage|string|True|Japanese language code|
|KoreanLanguage|string|True|Korean language code|
|PolishLanguage|string|True|Polish language code|
|RussianLanguage|string|True|Russian language code|
|SpanishLanguage|string|True|Spanish language code|
|TraditionalChineseLanguage|string|True|Traditional Chinese language code|
|TurkishLanguage|string|True|Turkish language code|
|UkrainianLanguage|string|True|Ukrainian language code|
|CurrentLanguage|string|True|Returns the current language (e.g. "English" or "简体中文").|
|DefaultLanguage|string|False|The default language to use when a string is not found in the current language pack. English by default.|


### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function Get(key: string) -> string</code></pre>
> Get the localized string for the given key. Searches the current UI language, then any registered fallbacks, and finally the default language. Throws an exception if the key is not found in any language pack.
> 
<pre class="language-typescript"><code class="lang-typescript">function Set(language: string, key: string, value: string)</code></pre>
> Set or override a localized string for the specified language and key.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterLanguage(language: string, strings: <a data-footnote-ref href="#user-content-fn-7">Dict</a>)</code></pre>
> Register a single-level (non-recursive) fallback: if a string is not found in 'fromLanguage', the system will search only in 'toLanguage', without chaining further.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterLanguages(pattern: string)</code></pre>
> Register all localized strings from JSON files for a specific category across all available languages. Use 'internal://' prefix for internal files (e.g., 'internal://BasicTutorialMap') or no prefix for external files (e.g., 'MyCustomMod').
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterFallback(fromLanguage: string, toLanguage: string)</code></pre>
> Register a fallback language. When a string is not found in 'fromLanguage', it will try 'toLanguage'.
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveFallback(fromLanguage: string)</code></pre>
> Remove a language fallback.
> 

[^0]: [Camera](../static/Camera.md)
[^1]: [Character](../objects/Character.md)
[^2]: [Collider](../objects/Collider.md)
[^3]: [Collision](../objects/Collision.md)
[^4]: [Color](../objects/Color.md)
[^5]: [Convert](../static/Convert.md)
[^6]: [Cutscene](../static/Cutscene.md)
[^7]: [Dict](../objects/Dict.md)
[^8]: [Game](../static/Game.md)
[^9]: [Human](../objects/Human.md)
[^10]: [Input](../static/Input.md)
[^11]: [Json](../static/Json.md)
[^12]: [LightBuiltin](../static/LightBuiltin.md)
[^13]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^14]: [LineRenderer](../objects/LineRenderer.md)
[^15]: [List](../objects/List.md)
[^16]: [Locale](../static/Locale.md)
[^17]: [LodBuiltin](../static/LodBuiltin.md)
[^18]: [Map](../static/Map.md)
[^19]: [MapObject](../objects/MapObject.md)
[^20]: [MapTargetable](../objects/MapTargetable.md)
[^21]: [Math](../static/Math.md)
[^22]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^23]: [Network](../static/Network.md)
[^24]: [NetworkView](../objects/NetworkView.md)
[^25]: [PersistentData](../static/PersistentData.md)
[^26]: [Physics](../static/Physics.md)
[^27]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^28]: [Player](../objects/Player.md)
[^29]: [Prefab](../objects/Prefab.md)
[^30]: [Quaternion](../objects/Quaternion.md)
[^31]: [Random](../objects/Random.md)
[^32]: [Range](../objects/Range.md)
[^33]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^34]: [RoomData](../static/RoomData.md)
[^35]: [Set](../objects/Set.md)
[^36]: [Shifter](../objects/Shifter.md)
[^37]: [String](../static/String.md)
[^38]: [Time](../static/Time.md)
[^39]: [Titan](../objects/Titan.md)
[^40]: [Transform](../objects/Transform.md)
[^41]: [UI](../static/UI.md)
[^42]: [Vector2](../objects/Vector2.md)
[^43]: [Vector3](../objects/Vector3.md)
[^44]: [WallColossal](../objects/WallColossal.md)
[^45]: [Object](../objects/Object.md)
[^46]: [Component](../objects/Component.md)
