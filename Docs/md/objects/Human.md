# Human
Inherits from [Character](../objects/Character.md)

Represents a human character. Only character owner can modify fields and call functions unless otherwise specified.

### Example
```csharp
function OnCharacterSpawn(character)
{
    if (character.IsMainCharacter && character.Type == "Human")
    {
        character.SetWeapon("Blade");
        character.SetSpecial("Potato");
        character.CurrentGas = character.MaxGas / 2;
    }
}
```
### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Weapon|string|False|The weapon the human is using. Refer to [WeaponEnum](../static/WeaponEnum.md)|
|CurrentSpecial|string|False|The current special the human is using.|
|SpecialCooldownTime|float|False|The normalized cooldown time of the special. Has a range of 0 to 1.|
|SpecialCooldown|float|False|The cooldown of the special.|
|ShifterLiveTime|float|False|The live time of the shifter special.|
|SpecialCooldownRatio|float|True|The ratio of the special cooldown.|
|CurrentGas|float|False|The current gas of the human.|
|MaxGas|float|False|The max gas of the human.|
|Acceleration|int|False|The acceleration of the human.|
|Speed|int|False|The speed of the human.|
|HorseTransform|[Transform](../objects/Transform.md)|True|The transform of the horse belonging to this human. Returns null if horses are disabled.|
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
|MountedMapObject|[MapObject](../objects/MapObject.md)|True|The map object the human is mounted on.|
|MountedTransform|[Transform](../objects/Transform.md)|True|The transform the human is mounted on.|
|AutoRefillGas|bool|False|Whether the human auto refills gas.|
|State|string|True|The state of the human.|
|CanDodge|bool|False|Whether the human can dodge.|
|IsInvincible|bool|False|Whether the human is invincible.|
|InvincibleTimeLeft|float|False|The time left for invincibility.|
|IsCarried|bool|True|If the human is carried.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Refill() -> bool</code></pre>
> Refills the gas of the human.
> 
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
<pre class="language-typescript"><code class="lang-typescript">function RightHookPosition() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Position of the right hook, null if there is no hook.
> 
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
> - `special`: The name of the special to set.
> 
<pre class="language-typescript"><code class="lang-typescript">function ActivateSpecial()</code></pre>
> Activates the special of the human.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetWeapon(weapon: string)</code></pre>
> Sets the weapon for the human.
> 
> **Parameters**:
> - `weapon`: Name of the weapon. Refer to [WeaponEnum](../static/WeaponEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function DisablePerks()</code></pre>
> Disables all perks of the human.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetParticleEffect(effectName: string, enabled: bool)</code></pre>
> Enables or disables a particle effect.
> 
> **Parameters**:
> - `effectName`: Name of the effect. Refer to [HumanParticleEffectEnum](../static/HumanParticleEffectEnum.md)
> - `enabled`: True to enable, false to disable
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
