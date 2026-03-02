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
[^34]: [AspectRatioEnum](../Enums/AspectRatioEnum.md)
[^35]: [CameraModeEnum](../Enums/CameraModeEnum.md)
[^36]: [CharacterTypeEnum](../Enums/CharacterTypeEnum.md)
[^37]: [CollideModeEnum](../Enums/CollideModeEnum.md)
[^38]: [CollideWithEnum](../Enums/CollideWithEnum.md)
[^39]: [CollisionDetectionModeEnum](../Enums/CollisionDetectionModeEnum.md)
[^40]: [DummyAnimationEnum](../Enums/DummyAnimationEnum.md)
[^41]: [EffectNameEnum](../Enums/EffectNameEnum.md)
[^42]: [ErenAnimationEnum](../Enums/ErenAnimationEnum.md)
[^43]: [FlexDirectionEnum](../Enums/FlexDirectionEnum.md)
[^44]: [FontScaleModeEnum](../Enums/FontScaleModeEnum.md)
[^45]: [FontStyleEnum](../Enums/FontStyleEnum.md)
[^46]: [ForceModeEnum](../Enums/ForceModeEnum.md)
[^47]: [GradientModeEnum](../Enums/GradientModeEnum.md)
[^48]: [HandStateEnum](../Enums/HandStateEnum.md)
[^49]: [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
[^50]: [HumanAnimationEnum](../Enums/HumanAnimationEnum.md)
[^51]: [HumanParticleEffectEnum](../Enums/HumanParticleEffectEnum.md)
[^52]: [HumanSoundEnum](../Enums/HumanSoundEnum.md)
[^53]: [HumanStateEnum](../Enums/HumanStateEnum.md)
[^54]: [InputAnnieShifterEnum](../Enums/InputAnnieShifterEnum.md)
[^55]: [InputCategoryEnum](../Enums/InputCategoryEnum.md)
[^56]: [InputErenShifterEnum](../Enums/InputErenShifterEnum.md)
[^57]: [InputGeneralEnum](../Enums/InputGeneralEnum.md)
[^58]: [InputHumanEnum](../Enums/InputHumanEnum.md)
[^59]: [InputInteractionEnum](../Enums/InputInteractionEnum.md)
[^60]: [InputTitanEnum](../Enums/InputTitanEnum.md)
[^61]: [JustifyEnum](../Enums/JustifyEnum.md)
[^62]: [LanguageEnum](../Enums/LanguageEnum.md)
[^63]: [LineAlignmentEnum](../Enums/LineAlignmentEnum.md)
[^64]: [LineTextureModeEnum](../Enums/LineTextureModeEnum.md)
[^65]: [LoadoutEnum](../Enums/LoadoutEnum.md)
[^66]: [OutlineModeEnum](../Enums/OutlineModeEnum.md)
[^67]: [OverflowEnum](../Enums/OverflowEnum.md)
[^68]: [PhysicMaterialCombineEnum](../Enums/PhysicMaterialCombineEnum.md)
[^69]: [PlayerStatusEnum](../Enums/PlayerStatusEnum.md)
[^70]: [ProfileIconEnum](../Enums/ProfileIconEnum.md)
[^71]: [ProjectileNameEnum](../Enums/ProjectileNameEnum.md)
[^72]: [ScaleModeEnum](../Enums/ScaleModeEnum.md)
[^73]: [ScrollElasticityEnum](../Enums/ScrollElasticityEnum.md)
[^74]: [ShadowCastingModeEnum](../Enums/ShadowCastingModeEnum.md)
[^75]: [ShifterSoundEnum](../Enums/ShifterSoundEnum.md)
[^76]: [ShifterTypeEnum](../Enums/ShifterTypeEnum.md)
[^77]: [SliderDirectionEnum](../Enums/SliderDirectionEnum.md)
[^78]: [SpecialEnum](../Enums/SpecialEnum.md)
[^79]: [SteamStateEnum](../Enums/SteamStateEnum.md)
[^80]: [StunStateEnum](../Enums/StunStateEnum.md)
[^81]: [TeamEnum](../Enums/TeamEnum.md)
[^82]: [TextAlignEnum](../Enums/TextAlignEnum.md)
[^83]: [TextOverflowEnum](../Enums/TextOverflowEnum.md)
[^84]: [TitanAnimationEnum](../Enums/TitanAnimationEnum.md)
[^85]: [TitanSoundEnum](../Enums/TitanSoundEnum.md)
[^86]: [TitanTypeEnum](../Enums/TitanTypeEnum.md)
[^87]: [TSKillSoundEnum](../Enums/TSKillSoundEnum.md)
[^88]: [UILabelEnum](../Enums/UILabelEnum.md)
[^89]: [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md)
[^90]: [WeaponEnum](../Enums/WeaponEnum.md)
[^91]: [Camera](../Game/Camera.md)
[^92]: [Cutscene](../Game/Cutscene.md)
[^93]: [Game](../Game/Game.md)
[^94]: [Input](../Game/Input.md)
[^95]: [Locale](../Game/Locale.md)
[^96]: [Map](../Game/Map.md)
[^97]: [Network](../Game/Network.md)
[^98]: [PersistentData](../Game/PersistentData.md)
[^99]: [Physics](../Game/Physics.md)
[^100]: [RoomData](../Game/RoomData.md)
[^101]: [Time](../Game/Time.md)
[^102]: [Button](../UIElements/Button.md)
[^103]: [Dropdown](../UIElements/Dropdown.md)
[^104]: [Icon](../UIElements/Icon.md)
[^105]: [Image](../UIElements/Image.md)
[^106]: [Label](../UIElements/Label.md)
[^107]: [ProgressBar](../UIElements/ProgressBar.md)
[^108]: [ScrollView](../UIElements/ScrollView.md)
[^109]: [Slider](../UIElements/Slider.md)
[^110]: [TextField](../UIElements/TextField.md)
[^111]: [Toggle](../UIElements/Toggle.md)
[^112]: [UI](../UIElements/UI.md)
[^113]: [VisualElement](../UIElements/VisualElement.md)
[^114]: [Convert](../Utility/Convert.md)
[^115]: [Json](../Utility/Json.md)
[^116]: [Math](../Utility/Math.md)
[^117]: [Random](../Utility/Random.md)
[^118]: [String](../Utility/String.md)
[^119]: [Object](../objects/Object.md)
[^120]: [Component](../objects/Component.md)
