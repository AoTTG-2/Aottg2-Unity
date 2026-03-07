# Convert
Inherits from [Object](../objects/Object.md)

Converting objects to different types.

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function ToFloat(value: <a data-footnote-ref href="#user-content-fn-119">Object</a>) -> float</code></pre>
> Converts a value to a float.
> 
> **Parameters**:
> - `value`: The value to convert (can be string, float, int, or bool).
> 
> **Returns**: The converted float value.
<pre class="language-typescript"><code class="lang-typescript">function ToInt(value: <a data-footnote-ref href="#user-content-fn-119">Object</a>) -> int</code></pre>
> Converts a value to an int.
> 
> **Parameters**:
> - `value`: The value to convert (can be string, float, int, or bool).
> 
> **Returns**: The converted int value.
<pre class="language-typescript"><code class="lang-typescript">function ToBool(value: <a data-footnote-ref href="#user-content-fn-119">Object</a>) -> bool</code></pre>
> Converts a value to a bool.
> 
> **Parameters**:
> - `value`: The value to convert (can be string, float, int, or bool).
> 
> **Returns**: The converted bool value.
<pre class="language-typescript"><code class="lang-typescript">function ToString(value: <a data-footnote-ref href="#user-content-fn-119">Object</a>) -> string</code></pre>
> Converts a value to a string.
> 
> **Parameters**:
> - `value`: The value to convert.
> 
> **Returns**: The converted string value.
<pre class="language-typescript"><code class="lang-typescript">function IsFloat(value: <a data-footnote-ref href="#user-content-fn-119">Object</a>) -> bool</code></pre>
> Checks if the value is a float.
> 
> **Parameters**:
> - `value`: The value to check.
> 
> **Returns**: True if the value is a float, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function IsInt(value: <a data-footnote-ref href="#user-content-fn-119">Object</a>) -> bool</code></pre>
> Checks if the value is an int.
> 
> **Parameters**:
> - `value`: The value to check.
> 
> **Returns**: True if the value is an int, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function IsBool(value: <a data-footnote-ref href="#user-content-fn-119">Object</a>) -> bool</code></pre>
> Checks if the value is a bool.
> 
> **Parameters**:
> - `value`: The value to check.
> 
> **Returns**: True if the value is a bool, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function IsString(value: <a data-footnote-ref href="#user-content-fn-119">Object</a>) -> bool</code></pre>
> Checks if the value is a string.
> 
> **Parameters**:
> - `value`: The value to check.
> 
> **Returns**: True if the value is a string, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function IsObject(value: <a data-footnote-ref href="#user-content-fn-119">Object</a>) -> bool</code></pre>
> Checks if the value is an object.
> 
> **Parameters**:
> - `value`: The value to check.
> 
> **Returns**: True if the value is an object, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function IsList(value: <a data-footnote-ref href="#user-content-fn-119">Object</a>) -> bool</code></pre>
> Checks if the value is a list.
> 
> **Parameters**:
> - `value`: The value to check.
> 
> **Returns**: True if the value is a list, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function IsDict(value: <a data-footnote-ref href="#user-content-fn-119">Object</a>) -> bool</code></pre>
> Checks if the value is a dictionary.
> 
> **Parameters**:
> - `value`: The value to check.
> 
> **Returns**: True if the value is a dictionary, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function HasVariable(cInstance: class, variableName: string) -> bool</code></pre>
> Checks if the class instance has a variable.
> 
> **Parameters**:
> - `cInstance`: The class instance to check.
> - `variableName`: The name of the variable to check for.
> 
> **Returns**: True if the class instance has the variable, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function DefineVariable(cInstance: class, variableName: string, value: <a data-footnote-ref href="#user-content-fn-119">Object</a>)</code></pre>
> Defines a variable for the class instance.
> 
> **Parameters**:
> - `cInstance`: The class instance to define the variable on.
> - `variableName`: The name of the variable to define.
> - `value`: The value to assign to the variable.
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveVariable(cInstance: class, variableName: string)</code></pre>
> Removes a variable from the class instance.
> 
> **Parameters**:
> - `cInstance`: The class instance to remove the variable from.
> - `variableName`: The name of the variable to remove.
> 
<pre class="language-typescript"><code class="lang-typescript">function HasMethod(cInstance: class, methodName: string) -> bool</code></pre>
> Checks if the class instance has a method.
> 
> **Parameters**:
> - `cInstance`: The class instance to check.
> - `methodName`: The name of the method to check for.
> 
> **Returns**: True if the class instance has the method, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function GetType(cInstance: class) -> string</code></pre>
> Gets the type of the class instance.
> 
> **Parameters**:
> - `cInstance`: The class instance to get the type of.
> 
> **Returns**: The class name of the instance.

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
