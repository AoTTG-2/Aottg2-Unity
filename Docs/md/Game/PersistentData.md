# PersistentData
Inherits from [Object](../objects/Object.md)

Store and retrieve persistent data. Persistent data can be saved and loaded from file.
Supports float, int, string, and bool types. Note that any game mode may use the same file names,
so it is recommended that you choose unique file names when saving and loading.
Saved files are located in Documents/Aottg2/PersistentData.

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function SetProperty(property: string, value: float|int|string|bool|null)</code></pre>
> Sets the property with given name to the object value. Valid value types are float, string, bool, and int.
> 
> **Parameters**:
> - `property`: The name of the property.
> - `value`: The value to set (must be float, string, bool, int, or null).
> 
<pre class="language-typescript"><code class="lang-typescript">function GetProperty(property: string, defaultValue: <a data-footnote-ref href="#user-content-fn-119">Object</a>) -> <a data-footnote-ref href="#user-content-fn-119">Object</a></code></pre>
> Gets the property with given name.
> 
> **Parameters**:
> - `property`: The name of the property.
> - `defaultValue`: The default value to return if the property does not exist.
> 
> **Returns**: The property value, or defaultValue if the property does not exist.
<pre class="language-typescript"><code class="lang-typescript">function LoadFromFile(fileName: string, encrypted: bool)</code></pre>
> Loads persistent data from given file name.
> 
> **Parameters**:
> - `fileName`: The name of the file to load from.
> - `encrypted`: Whether the file is encrypted. If true, will treat the file as having been saved as encrypted.
> 
<pre class="language-typescript"><code class="lang-typescript">function SaveToFile(fileName: string, encrypted: bool)</code></pre>
> Saves current persistent data to given file name.
> 
> **Parameters**:
> - `fileName`: The name of the file to save to.
> - `encrypted`: Whether to encrypt the file. If true, will also encrypt the file instead of using plaintext.
> 
<pre class="language-typescript"><code class="lang-typescript">function Clear()</code></pre>
> Clears current persistent data.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsValidFileName(fileName: string) -> bool</code></pre>
> Determines whether or not the given fileName will be allowed for use when saving/loading a file.
> 
> **Parameters**:
> - `fileName`: The file name to validate.
> 
> **Returns**: True if the file name is valid, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function FileExists(fileName: string) -> bool</code></pre>
> Determines whether the file given already exists.
> 
> **Parameters**:
> - `fileName`: The file name to check.
> 
> **Returns**: True if the file exists, false otherwise. Throws an error if given an invalid file name.

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
