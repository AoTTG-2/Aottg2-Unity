# Locale
Inherits from [Object](../objects/Object.md)

Internationalization (Locale) utility for managing localized strings.
Supports single-level (non-recursive) language fallbacks and automatic UI language detection.

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
|DefaultLanguage|string|False|The default language to use when a string is not found in the current language pack. English by default. Refer to [LanguageEnum](../Enums/LanguageEnum.md)|


### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function Get(key: string) -> string</code></pre>
> Get the localized string for the given key. Searches the current UI language, then any registered fallbacks,
and finally the default language. Throws an exception if the key is not found in any language pack.
> 
> **Parameters**:
> - `key`: The key of the localized string to get.
> 
> **Returns**: The localized string.
<pre class="language-typescript"><code class="lang-typescript">function Set(language: string, key: string, value: string)</code></pre>
> Set or override a localized string for the specified language and key.
> 
> **Parameters**:
> - `language`: The language code. Refer to [LanguageEnum](../Enums/LanguageEnum.md)
> - `key`: The key of the localized string.
> - `value`: The localized string value.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterLanguage(language: string, strings: <a data-footnote-ref href="#user-content-fn-1">Dict</a><string,string>)</code></pre>
> Register a single-level (non-recursive) fallback: if a string is not found in 'fromLanguage',
the system will search only in 'toLanguage', without chaining further.
> 
> **Parameters**:
> - `language`: The language code to register. Refer to [LanguageEnum](../Enums/LanguageEnum.md)
> - `strings`: The dictionary containing key-value pairs of localized strings.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterLanguages(pattern: string)</code></pre>
> Register all localized strings from JSON files for a specific category across all available languages.
Use 'internal://' prefix for internal files (e.g., 'internal://BasicTutorialMap') or no prefix for external files (e.g., 'MyCustomMod').
> 
> **Parameters**:
> - `pattern`: The category pattern. Use 'internal://' prefix for internal files or no prefix for external files.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterFallback(fromLanguage: string, toLanguage: string)</code></pre>
> Register a fallback language. When a string is not found in 'fromLanguage', it will try 'toLanguage'.
> 
> **Parameters**:
> - `fromLanguage`: The language code that will fallback to another language. Refer to [LanguageEnum](../Enums/LanguageEnum.md)
> - `toLanguage`: The language code to fallback to. Refer to [LanguageEnum](../Enums/LanguageEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveFallback(fromLanguage: string)</code></pre>
> Remove a language fallback.
> 
> **Parameters**:
> - `fromLanguage`: The language code to remove the fallback for. Refer to [LanguageEnum](../Enums/LanguageEnum.md)
> 

[^0]: [Color](../Collections/Color.md)
[^1]: [Dict](../Collections/Dict.md)
[^2]: [LightBuiltin](../Collections/LightBuiltin.md)
[^3]: [LineCastHitResult](../Collections/LineCastHitResult.md)
[^4]: [List](../Collections/List.md)
[^5]: [Quaternion](../Collections/Quaternion.md)
[^6]: [Range](../Collections/Range.md)
[^7]: [Set](../Collections/Set.md)
[^8]: [Vector2](../Collections/Vector2.md)
[^9]: [Vector3](../Collections/Vector3.md)
[^10]: [Animation](../Component/Animation.md)
[^11]: [Animator](../Component/Animator.md)
[^12]: [AudioSource](../Component/AudioSource.md)
[^13]: [Collider](../Component/Collider.md)
[^14]: [Collision](../Component/Collision.md)
[^15]: [LineRenderer](../Component/LineRenderer.md)
[^16]: [LodBuiltin](../Component/LodBuiltin.md)
[^17]: [MapTargetable](../Component/MapTargetable.md)
[^18]: [NavmeshObstacleBuiltin](../Component/NavmeshObstacleBuiltin.md)
[^19]: [PhysicsMaterialBuiltin](../Component/PhysicsMaterialBuiltin.md)
[^20]: [RigidbodyBuiltin](../Component/RigidbodyBuiltin.md)
[^21]: [Character](../Entities/Character.md)
[^22]: [Human](../Entities/Human.md)
[^23]: [MapObject](../Entities/MapObject.md)
[^24]: [NetworkView](../Entities/NetworkView.md)
[^25]: [Player](../Entities/Player.md)
[^26]: [Prefab](../Entities/Prefab.md)
[^27]: [Shifter](../Entities/Shifter.md)
[^28]: [Titan](../Entities/Titan.md)
[^29]: [Transform](../Entities/Transform.md)
[^30]: [WallColossal](../Entities/WallColossal.md)
[^31]: [AlignEnum](../Enums/AlignEnum.md)
[^32]: [AngleUnitEnum](../Enums/AngleUnitEnum.md)
[^33]: [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md)
[^34]: [CameraModeEnum](../Enums/CameraModeEnum.md)
[^35]: [CharacterTypeEnum](../Enums/CharacterTypeEnum.md)
[^36]: [CollideModeEnum](../Enums/CollideModeEnum.md)
[^37]: [CollideWithEnum](../Enums/CollideWithEnum.md)
[^38]: [CollisionDetectionModeEnum](../Enums/CollisionDetectionModeEnum.md)
[^39]: [DummyAnimationEnum](../Enums/DummyAnimationEnum.md)
[^40]: [EffectNameEnum](../Enums/EffectNameEnum.md)
[^41]: [ErenAnimationEnum](../Enums/ErenAnimationEnum.md)
[^42]: [FlexDirectionEnum](../Enums/FlexDirectionEnum.md)
[^43]: [FontStyleEnum](../Enums/FontStyleEnum.md)
[^44]: [ForceModeEnum](../Enums/ForceModeEnum.md)
[^45]: [GradientModeEnum](../Enums/GradientModeEnum.md)
[^46]: [HandStateEnum](../Enums/HandStateEnum.md)
[^47]: [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
[^48]: [HumanAnimationEnum](../Enums/HumanAnimationEnum.md)
[^49]: [HumanParticleEffectEnum](../Enums/HumanParticleEffectEnum.md)
[^50]: [HumanSoundEnum](../Enums/HumanSoundEnum.md)
[^51]: [HumanStateEnum](../Enums/HumanStateEnum.md)
[^52]: [InputAnnieShifterEnum](../Enums/InputAnnieShifterEnum.md)
[^53]: [InputCategoryEnum](../Enums/InputCategoryEnum.md)
[^54]: [InputErenShifterEnum](../Enums/InputErenShifterEnum.md)
[^55]: [InputGeneralEnum](../Enums/InputGeneralEnum.md)
[^56]: [InputHumanEnum](../Enums/InputHumanEnum.md)
[^57]: [InputInteractionEnum](../Enums/InputInteractionEnum.md)
[^58]: [InputTitanEnum](../Enums/InputTitanEnum.md)
[^59]: [JustifyEnum](../Enums/JustifyEnum.md)
[^60]: [LanguageEnum](../Enums/LanguageEnum.md)
[^61]: [LineAlignmentEnum](../Enums/LineAlignmentEnum.md)
[^62]: [LineTextureModeEnum](../Enums/LineTextureModeEnum.md)
[^63]: [LoadoutEnum](../Enums/LoadoutEnum.md)
[^64]: [OutlineModeEnum](../Enums/OutlineModeEnum.md)
[^65]: [OverflowEnum](../Enums/OverflowEnum.md)
[^66]: [PhysicMaterialCombineEnum](../Enums/PhysicMaterialCombineEnum.md)
[^67]: [PlayerStatusEnum](../Enums/PlayerStatusEnum.md)
[^68]: [ProfileIconEnum](../Enums/ProfileIconEnum.md)
[^69]: [ProjectileNameEnum](../Enums/ProjectileNameEnum.md)
[^70]: [ScaleModeEnum](../Enums/ScaleModeEnum.md)
[^71]: [ScrollElasticityEnum](../Enums/ScrollElasticityEnum.md)
[^72]: [ShadowCastingModeEnum](../Enums/ShadowCastingModeEnum.md)
[^73]: [ShifterSoundEnum](../Enums/ShifterSoundEnum.md)
[^74]: [ShifterTypeEnum](../Enums/ShifterTypeEnum.md)
[^75]: [SliderDirectionEnum](../Enums/SliderDirectionEnum.md)
[^76]: [SpecialEnum](../Enums/SpecialEnum.md)
[^77]: [SteamStateEnum](../Enums/SteamStateEnum.md)
[^78]: [TeamEnum](../Enums/TeamEnum.md)
[^79]: [TextAlignEnum](../Enums/TextAlignEnum.md)
[^80]: [TextOverflowEnum](../Enums/TextOverflowEnum.md)
[^81]: [TitanAnimationEnum](../Enums/TitanAnimationEnum.md)
[^82]: [TitanSoundEnum](../Enums/TitanSoundEnum.md)
[^83]: [TitanTypeEnum](../Enums/TitanTypeEnum.md)
[^84]: [TSKillSoundEnum](../Enums/TSKillSoundEnum.md)
[^85]: [UILabelEnum](../Enums/UILabelEnum.md)
[^86]: [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md)
[^87]: [WeaponEnum](../Enums/WeaponEnum.md)
[^88]: [Camera](../Game/Camera.md)
[^89]: [Cutscene](../Game/Cutscene.md)
[^90]: [Game](../Game/Game.md)
[^91]: [Input](../Game/Input.md)
[^92]: [Locale](../Game/Locale.md)
[^93]: [Map](../Game/Map.md)
[^94]: [Network](../Game/Network.md)
[^95]: [PersistentData](../Game/PersistentData.md)
[^96]: [Physics](../Game/Physics.md)
[^97]: [RoomData](../Game/RoomData.md)
[^98]: [Time](../Game/Time.md)
[^99]: [Button](../UIElements/Button.md)
[^100]: [Dropdown](../UIElements/Dropdown.md)
[^101]: [Icon](../UIElements/Icon.md)
[^102]: [Image](../UIElements/Image.md)
[^103]: [Label](../UIElements/Label.md)
[^104]: [ProgressBar](../UIElements/ProgressBar.md)
[^105]: [ScrollView](../UIElements/ScrollView.md)
[^106]: [Slider](../UIElements/Slider.md)
[^107]: [TextField](../UIElements/TextField.md)
[^108]: [Toggle](../UIElements/Toggle.md)
[^109]: [UI](../UIElements/UI.md)
[^110]: [VisualElement](../UIElements/VisualElement.md)
[^111]: [Convert](../Utility/Convert.md)
[^112]: [Json](../Utility/Json.md)
[^113]: [Math](../Utility/Math.md)
[^114]: [Random](../Utility/Random.md)
[^115]: [String](../Utility/String.md)
[^116]: [Object](../objects/Object.md)
[^117]: [Component](../objects/Component.md)
