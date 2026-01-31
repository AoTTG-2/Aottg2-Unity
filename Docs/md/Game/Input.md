# Input
Inherits from [Object](../objects/Object.md)

Reading player key inputs. Note that inputs are best handled in OnFrame rather than OnTick,
due to being updated every frame and not every physics tick.

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function GetKeyName(key: string) -> string</code></pre>
> Gets the key name the player assigned to the key setting.
> 
> **Parameters**:
> - `key`: The key setting path (e.g., "General/Attack" or "CustomKey/Space"). Refer to [InputGeneralEnum](../Enums/InputGeneralEnum.md), [InputHumanEnum](../Enums/InputHumanEnum.md), [InputTitanEnum](../Enums/InputTitanEnum.md), [InputInteractionEnum](../Enums/InputInteractionEnum.md), [InputAnnieShifterEnum](../Enums/InputAnnieShifterEnum.md), [InputErenShifterEnum](../Enums/InputErenShifterEnum.md)
> 
> **Returns**: The key name.
<pre class="language-typescript"><code class="lang-typescript">function GetKeyHold(key: string) -> bool</code></pre>
> Returns true if the key is being held down.
> 
> **Parameters**:
> - `key`: The key setting path (e.g., "General/Attack" or "CustomKey/Space"). Refer to [InputGeneralEnum](../Enums/InputGeneralEnum.md), [InputHumanEnum](../Enums/InputHumanEnum.md), [InputTitanEnum](../Enums/InputTitanEnum.md), [InputInteractionEnum](../Enums/InputInteractionEnum.md), [InputAnnieShifterEnum](../Enums/InputAnnieShifterEnum.md), [InputErenShifterEnum](../Enums/InputErenShifterEnum.md)
> 
> **Returns**: True if the key is being held down, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function GetKeyDown(key: string) -> bool</code></pre>
> Returns true if the key was pressed down this frame.
> 
> **Parameters**:
> - `key`: The key setting path (e.g., "General/Attack" or "CustomKey/Space"). Refer to [InputGeneralEnum](../Enums/InputGeneralEnum.md), [InputHumanEnum](../Enums/InputHumanEnum.md), [InputTitanEnum](../Enums/InputTitanEnum.md), [InputInteractionEnum](../Enums/InputInteractionEnum.md), [InputAnnieShifterEnum](../Enums/InputAnnieShifterEnum.md), [InputErenShifterEnum](../Enums/InputErenShifterEnum.md)
> 
> **Returns**: True if the key was pressed down this frame, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function GetKeyUp(key: string) -> bool</code></pre>
> Returns true if the key was released this frame.
> 
> **Parameters**:
> - `key`: The key setting path (e.g., "General/Attack" or "CustomKey/Space"). Refer to [InputGeneralEnum](../Enums/InputGeneralEnum.md), [InputHumanEnum](../Enums/InputHumanEnum.md), [InputTitanEnum](../Enums/InputTitanEnum.md), [InputInteractionEnum](../Enums/InputInteractionEnum.md), [InputAnnieShifterEnum](../Enums/InputAnnieShifterEnum.md), [InputErenShifterEnum](../Enums/InputErenShifterEnum.md)
> 
> **Returns**: True if the key was released this frame, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function GetMouseAim() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns the position the player is aiming at.
> 
> **Returns**: The aim position.
<pre class="language-typescript"><code class="lang-typescript">function GetCursorAimDirection() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns the ray the player is aiming at.
> 
> **Returns**: The aim direction vector.
<pre class="language-typescript"><code class="lang-typescript">function GetMouseSpeed() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns the speed of the mouse.
> 
> **Returns**: The mouse speed vector.
<pre class="language-typescript"><code class="lang-typescript">function GetMousePosition() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns the position of the mouse.
> 
> **Returns**: The mouse position.
<pre class="language-typescript"><code class="lang-typescript">function GetScreenDimensions() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns the dimensions of the screen.
> 
> **Returns**: The screen dimensions.
<pre class="language-typescript"><code class="lang-typescript">function SetKeyDefaultEnabled(key: string, enabled: bool)</code></pre>
> Sets whether the key is enabled by default.
> 
> **Parameters**:
> - `key`: The key setting path (e.g., "General/Attack" or "CustomKey/Space"). Refer to [InputGeneralEnum](../Enums/InputGeneralEnum.md), [InputHumanEnum](../Enums/InputHumanEnum.md), [InputTitanEnum](../Enums/InputTitanEnum.md), [InputInteractionEnum](../Enums/InputInteractionEnum.md), [InputAnnieShifterEnum](../Enums/InputAnnieShifterEnum.md), [InputErenShifterEnum](../Enums/InputErenShifterEnum.md)
> - `enabled`: Whether the key should be enabled by default.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetKeyHold(key: string, enabled: bool)</code></pre>
> Sets whether the key is being held down.
> 
> **Parameters**:
> - `key`: The key setting path (e.g., "General/Attack" or "CustomKey/Space"). Refer to [InputGeneralEnum](../Enums/InputGeneralEnum.md), [InputHumanEnum](../Enums/InputHumanEnum.md), [InputTitanEnum](../Enums/InputTitanEnum.md), [InputInteractionEnum](../Enums/InputInteractionEnum.md), [InputAnnieShifterEnum](../Enums/InputAnnieShifterEnum.md), [InputErenShifterEnum](../Enums/InputErenShifterEnum.md)
> - `enabled`: Whether the key should be held down.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetCategoryKeysEnabled(category: string, enabled: bool)</code></pre>
> Sets whether all keys in the specified category are enabled by default.
> 
> **Parameters**:
> - `category`: The category name. Refer to [InputCategoryEnum](../Enums/InputCategoryEnum.md)
> - `enabled`: Whether the keys should be enabled by default.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetGeneralKeysEnabled(enabled: bool)</code></pre>
> Sets whether all General category keys are enabled by default.
> 
> **Parameters**:
> - `enabled`: Whether the keys should be enabled by default.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetInteractionKeysEnabled(enabled: bool)</code></pre>
> Sets whether all Interaction category keys are enabled by default.
> 
> **Parameters**:
> - `enabled`: Whether the keys should be enabled by default.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetTitanKeysEnabled(enabled: bool)</code></pre>
> Sets whether all Titan category keys are enabled by default.
> 
> **Parameters**:
> - `enabled`: Whether the keys should be enabled by default.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetHumanKeysEnabled(enabled: bool)</code></pre>
> Sets whether all Human category keys are enabled by default.
> 
> **Parameters**:
> - `enabled`: Whether the keys should be enabled by default.
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
