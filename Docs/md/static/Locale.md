# Locale
Inherits from [Object](../objects/Object.md)

Internationalization (Locale) utility for managing localized strings. Supports single-level (non-recursive) language fallbacks and automatic UI language detection.

### Example
```csharp
# Register individual strings for different languages
Locale.Set(LanguageEnum.English, "welcome", "Welcome to the game!");
Locale.Set(LanguageEnum.Russian, "welcome", "Добро пожаловать в игру!");
Locale.Set(LanguageEnum.Chinese, "welcome", "你好");

Game.Print("welcome: " + Locale.Get("welcome"));

# Register multiple strings at once using a dictionary
englishStrings = Dict();
englishStrings.Set("hello", "Hello");
englishStrings.Set("goodbye", "Goodbye");
englishStrings.Set("score", "Score: {0}");
Locale.RegisterLanguage(LanguageEnum.English, englishStrings);

russianStrings = Dict();
russianStrings.Set("hello", "Привет");
russianStrings.Set("goodbye", "Пока");
russianStrings.Set("score", "Счет: {0}");
Locale.RegisterLanguage(LanguageEnum.Russian, russianStrings);

chineseStrings = Dict();
chineseStrings.Set("hello", "你好");
chineseStrings.Set("goodbye", "再见");
chineseStrings.Set("score", "分数: {0}");
Locale.RegisterLanguage(LanguageEnum.Chinese, chineseStrings);

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
Locale.DefaultLanguage = LanguageEnum.Russian;

# Fallback to default for missing key in current language
Locale.Set(LanguageEnum.Russian, "russian_key", "Сообщение");
Game.Print("russian_key: " + Locale.Get("russian_key"));

# Single-level (non-recursive) fallback: English -> German
Locale.RegisterFallback(LanguageEnum.English, LanguageEnum.German);
Locale.Set(LanguageEnum.German, "german_string", "Hallo");
Game.Print("german_string: " + Locale.Get("german_string"));

# By default Traditional Chinese falls back to Simplified Chinese
Locale.Set(LanguageEnum.Chinese, "chinese_string", "你好");
Game.Print("chinese_string: " + Locale.Get("chinese_string"));

# Clean up fallbacks
Locale.RemoveFallback(LanguageEnum.Chinese);
Locale.RemoveFallback(LanguageEnum.TraditionalChinese);

# Missing key throws an exception
Game.Print("missing_key: " + Locale.Get("missing_key"));
```
### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|CurrentLanguage|string|True|Returns the current language (e.g. "English" or "简体中文").|
|DefaultLanguage|string|False|The default language to use when a string is not found in the current language pack. English by default. Refer to [LanguageEnum](../static/LanguageEnum.md)|


### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function Get(key: string) -> string</code></pre>
> Get the localized string for the given key. Searches the current UI language, then any registered fallbacks, and finally the default language. Throws an exception if the key is not found in any language pack.
> 
> **Parameters**:
> - `key`: The key of the localized string to get.
> 
<pre class="language-typescript"><code class="lang-typescript">function Set(language: string, key: string, value: string)</code></pre>
> Set or override a localized string for the specified language and key.
> 
> **Parameters**:
> - `language`: The language code. Refer to [LanguageEnum](../static/LanguageEnum.md)
> - `key`: The key of the localized string.
> - `value`: The localized string value.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterLanguage(language: string, strings: <a data-footnote-ref href="#user-content-fn-1">Dict</a><string,string>)</code></pre>
> Register a single-level (non-recursive) fallback: if a string is not found in 'fromLanguage', the system will search only in 'toLanguage', without chaining further.
> 
> **Parameters**:
> - `language`: The language code to register. Refer to [LanguageEnum](../static/LanguageEnum.md)
> - `strings`: The dictionary containing key-value pairs of localized strings.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterLanguages(pattern: string)</code></pre>
> Register all localized strings from JSON files for a specific category across all available languages. Use 'internal://' prefix for internal files (e.g., 'internal://BasicTutorialMap') or no prefix for external files (e.g., 'MyCustomMod').
> 
> **Parameters**:
> - `pattern`: The category pattern. Use 'internal://' prefix for internal files or no prefix for external files.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterFallback(fromLanguage: string, toLanguage: string)</code></pre>
> Register a fallback language. When a string is not found in 'fromLanguage', it will try 'toLanguage'.
> 
> **Parameters**:
> - `fromLanguage`: The language code that will fallback to another language. Refer to [LanguageEnum](../static/LanguageEnum.md)
> - `toLanguage`: The language code to fallback to. Refer to [LanguageEnum](../static/LanguageEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveFallback(fromLanguage: string)</code></pre>
> Remove a language fallback.
> 
> **Parameters**:
> - `fromLanguage`: The language code to remove the fallback for. Refer to [LanguageEnum](../static/LanguageEnum.md)
> 

[^0]: [Color](../objects/Color.md)
[^1]: [Dict](../objects/Dict.md)
[^2]: [LightBuiltin](../static/LightBuiltin.md)
[^3]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^4]: [List](../objects/List.md)
[^5]: [Quaternion](../objects/Quaternion.md)
[^6]: [Range](../objects/Range.md)
[^7]: [Set](../objects/Set.md)
[^8]: [Vector2](../objects/Vector2.md)
[^9]: [Vector3](../objects/Vector3.md)
[^10]: [Animation](../objects/Animation.md)
[^11]: [Animator](../objects/Animator.md)
[^12]: [AudioSource](../objects/AudioSource.md)
[^13]: [Collider](../objects/Collider.md)
[^14]: [Collision](../objects/Collision.md)
[^15]: [LineRenderer](../objects/LineRenderer.md)
[^16]: [LodBuiltin](../static/LodBuiltin.md)
[^17]: [MapTargetable](../objects/MapTargetable.md)
[^18]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^19]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^20]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^21]: [Character](../objects/Character.md)
[^22]: [Human](../objects/Human.md)
[^23]: [MapObject](../objects/MapObject.md)
[^24]: [NetworkView](../objects/NetworkView.md)
[^25]: [Player](../objects/Player.md)
[^26]: [Prefab](../objects/Prefab.md)
[^27]: [Shifter](../objects/Shifter.md)
[^28]: [Titan](../objects/Titan.md)
[^29]: [Transform](../objects/Transform.md)
[^30]: [WallColossal](../objects/WallColossal.md)
[^31]: [CharacterTypeEnum](../static/CharacterTypeEnum.md)
[^32]: [CollideModeEnum](../static/CollideModeEnum.md)
[^33]: [CollideWithEnum](../static/CollideWithEnum.md)
[^34]: [CollisionDetectionModeEnum](../static/CollisionDetectionModeEnum.md)
[^35]: [EffectNameEnum](../static/EffectNameEnum.md)
[^36]: [ForceModeEnum](../static/ForceModeEnum.md)
[^37]: [HandStateEnum](../static/HandStateEnum.md)
[^38]: [HumanParticleEffectEnum](../static/HumanParticleEffectEnum.md)
[^39]: [InputCategoryEnum](../static/InputCategoryEnum.md)
[^40]: [LanguageEnum](../static/LanguageEnum.md)
[^41]: [LoadoutEnum](../static/LoadoutEnum.md)
[^42]: [OutlineModeEnum](../static/OutlineModeEnum.md)
[^43]: [PhysicMaterialCombineEnum](../static/PhysicMaterialCombineEnum.md)
[^44]: [PlayerStatusEnum](../static/PlayerStatusEnum.md)
[^45]: [ProjectileNameEnum](../static/ProjectileNameEnum.md)
[^46]: [ScaleModeEnum](../static/ScaleModeEnum.md)
[^47]: [ShifterTypeEnum](../static/ShifterTypeEnum.md)
[^48]: [SliderDirectionEnum](../static/SliderDirectionEnum.md)
[^49]: [SteamStateEnum](../static/SteamStateEnum.md)
[^50]: [TeamEnum](../static/TeamEnum.md)
[^51]: [TitanTypeEnum](../static/TitanTypeEnum.md)
[^52]: [TSKillSoundEnum](../static/TSKillSoundEnum.md)
[^53]: [WeaponEnum](../static/WeaponEnum.md)
[^54]: [Camera](../static/Camera.md)
[^55]: [Cutscene](../static/Cutscene.md)
[^56]: [Game](../static/Game.md)
[^57]: [Input](../static/Input.md)
[^58]: [Locale](../static/Locale.md)
[^59]: [Map](../static/Map.md)
[^60]: [Network](../static/Network.md)
[^61]: [PersistentData](../static/PersistentData.md)
[^62]: [Physics](../static/Physics.md)
[^63]: [RoomData](../static/RoomData.md)
[^64]: [Time](../static/Time.md)
[^65]: [Button](../objects/Button.md)
[^66]: [Dropdown](../objects/Dropdown.md)
[^67]: [Icon](../objects/Icon.md)
[^68]: [Image](../objects/Image.md)
[^69]: [Label](../objects/Label.md)
[^70]: [ProgressBar](../objects/ProgressBar.md)
[^71]: [ScrollView](../objects/ScrollView.md)
[^72]: [Slider](../objects/Slider.md)
[^73]: [TextField](../objects/TextField.md)
[^74]: [Toggle](../objects/Toggle.md)
[^75]: [UI](../static/UI.md)
[^76]: [VisualElement](../objects/VisualElement.md)
[^77]: [Convert](../static/Convert.md)
[^78]: [Json](../static/Json.md)
[^79]: [Math](../static/Math.md)
[^80]: [Random](../objects/Random.md)
[^81]: [String](../static/String.md)
[^82]: [Object](../objects/Object.md)
[^83]: [Component](../objects/Component.md)
