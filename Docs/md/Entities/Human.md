# Human
Inherits from [Character](../Entities/Character.md)

Represents a human character. Only character owner can modify fields and call functions unless otherwise specified.

### Example
```csharp
function OnCharacterSpawn(character)
{
    if (character.IsMainCharacter && character.Type == "Human")
    {
        character.SetWeapon(WeaponEnum.Blade);
        character.SetSpecial(SpecialEnum.Potato);
        character.CurrentGas = character.MaxGas / 2;
    }
}
```
### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Weapon|string|False|The weapon the human is using. Refer to [WeaponEnum](../Enums/WeaponEnum.md)|
|CurrentSpecial|string|False|The current special the human is using. Refer to [SpecialEnum](../Enums/SpecialEnum.md)|
|SpecialCooldownTime|float|False|The normalized cooldown time of the special. Has a range of 0 to 1.|
|SpecialCooldown|float|False|The cooldown of the special.|
|ShifterLiveTime|float|False|The live time of the shifter special.|
|SpecialCooldownRatio|float|True|The ratio of the special cooldown.|
|CurrentGas|float|False|The current gas of the human.|
|MaxGas|float|False|The max gas of the human.|
|Acceleration|int|False|The acceleration of the human.|
|Speed|int|False|The speed of the human.|
|HorseTransform|[Transform](../Entities/Transform.md)|True|The transform of the horse belonging to this human. Returns null if horses are disabled.|
|HorseSpeed|float|False|The speed of the horse.|
|CurrentBladeDurability|float|False|The current blade durability.|
|MaxBladeDurability|float|False|The max blade durability.|
|CurrentBlade|int|False|The current blade.|
|MaxBlade|int|False|The max number of blades held.|
|CurrentAmmoRound|int|False|The current ammo round.|
|MaxAmmoRound|int|False|The max ammo round.|
|CurrentAmmoLeft|int|False|The current ammo left.|
|MaxAmmoTotal|int|False|The max total ammo.|
|LeftHookEnabled|bool|False|Whether the left hook is enabled.|
|RightHookEnabled|bool|False|Whether the right hook is enabled.|
|IsMounted|bool|True|Whether the human is mounted.|
|MountedMapObject|[MapObject](../Entities/MapObject.md)|True|The map object the human is mounted on.|
|MountedTransform|[Transform](../Entities/Transform.md)|True|The transform the human is mounted on.|
|AutoRefillGas|bool|False|Whether the human auto refills gas.|
|State|string|True|The state of the human. Refer to [HumanStateEnum](../Enums/HumanStateEnum.md)|
|CanDodge|bool|False|Whether the human can dodge.|
|IsInvincible|bool|False|Whether the human is invincible.|
|InvincibleTimeLeft|float|False|The time left for invincibility.|
|IsCarried|bool|True|If the human is carried.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Refill() -> bool</code></pre>
> Refills the gas of the human.
> 
> **Returns**: True if refill was successful, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function RefillImmediate()</code></pre>
> Refills the gas of the human immediately.
> 
<pre class="language-typescript"><code class="lang-typescript">function ClearHooks()</code></pre>
> Clears all hooks.
> 
<pre class="language-typescript"><code class="lang-typescript">function ClearLeftHook()</code></pre>
> Clears the left hook.
> 
<pre class="language-typescript"><code class="lang-typescript">function ClearRightHook()</code></pre>
> Clears the right hook.
> 
<pre class="language-typescript"><code class="lang-typescript">function LeftHookPosition() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Position of the left hook, null if there is no hook.
> 
> **Returns**: The position of the left hook, or null if there is no hook.
<pre class="language-typescript"><code class="lang-typescript">function RightHookPosition() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Position of the right hook, null if there is no hook.
> 
> **Returns**: The position of the right hook, or null if there is no hook.
<pre class="language-typescript"><code class="lang-typescript">function MountMapObject(mapObject: <a data-footnote-ref href="#user-content-fn-23">MapObject</a>, positionOffset: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationOffset: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, canMountedAttack: bool = False)</code></pre>
> Mounts the human on a map object.
> 
> **Parameters**:
> - `mapObject`: The map object to mount on.
> - `positionOffset`: The position offset from the mount point.
> - `rotationOffset`: The rotation offset from the mount point.
> - `canMountedAttack`: If true, allows the human to attack while mounted (default: false).
> 
<pre class="language-typescript"><code class="lang-typescript">function MountTransform(transform: <a data-footnote-ref href="#user-content-fn-29">Transform</a>, positionOffset: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationOffset: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, canMountedAttack: bool = False)</code></pre>
> Mounts the human on a transform.
> 
> **Parameters**:
> - `transform`: The transform to mount on.
> - `positionOffset`: The position offset from the mount point.
> - `rotationOffset`: The rotation offset from the mount point.
> - `canMountedAttack`: If true, allows the human to attack while mounted (default: false).
> 
<pre class="language-typescript"><code class="lang-typescript">function Unmount(immediate: bool = True)</code></pre>
> Unmounts the human.
> 
> **Parameters**:
> - `immediate`: If true, unmounts immediately without animation (default: true).
> 
<pre class="language-typescript"><code class="lang-typescript">function SetSpecial(special: string)</code></pre>
> Sets the special of the human.
> 
> **Parameters**:
> - `special`: The name of the special to set. Refer to [SpecialEnum](../Enums/SpecialEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function ActivateSpecial()</code></pre>
> Activates the special of the human.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetWeapon(weapon: string)</code></pre>
> Sets the weapon for the human.
> 
> **Parameters**:
> - `weapon`: Name of the weapon. Refer to [WeaponEnum](../Enums/WeaponEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function DisablePerks()</code></pre>
> Disables all perks of the human.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetParticleEffect(effectName: string, enabled: bool)</code></pre>
> Enables or disables a particle effect.
> 
> **Parameters**:
> - `effectName`: Name of the effect. Refer to [HumanParticleEffectEnum](../Enums/HumanParticleEffectEnum.md)
> - `enabled`: True to enable, false to disable.
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
