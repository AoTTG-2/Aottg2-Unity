# Human
Inherits from [Character](../objects/Character.md)

Represents a human character.
Only character owner can modify fields and call functions unless otherwise specified.

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
|Weapon|string|False|The weapon the human is using|
|CurrentSpecial|string|False|The current special the human is using|
|SpecialCooldownTime|float|False|The normalized cooldown time of the special. Has a range of 0 to 1.|
|SpecialCooldown|float|False|The cooldown of the special|
|ShifterLiveTime|float|False|The live time of the shifter special|
|SpecialCooldownRatio|float|True|The ratio of the special cooldown|
|CurrentGas|float|False|The current gas of the human|
|MaxGas|float|False|The max gas of the human|
|Acceleration|int|False|The acceleration of the human|
|Speed|int|False|The speed of the human|
|HorseSpeed|float|False|The speed of the horse|
|CurrentBladeDurability|float|False|The current blade durability|
|MaxBladeDurability|float|False|The max blade durability|
|CurrentBlade|int|False|The current blade|
|MaxBlade|int|False|The max number of blades held|
|CurrentAmmoRound|int|False|The current ammo round|
|MaxAmmoRound|int|False|The max ammo round|
|CurrentAmmoLeft|int|False|The current ammo left|
|MaxAmmoTotal|int|False|The max total ammo|
|LeftHookEnabled|bool|False|Whether the left hook is enabled|
|RightHookEnabled|bool|False|Whether the right hook is enabled|
|IsMounted|bool|True|Whether the human is mounted|
|MountedMapObject|[MapObject](../objects/MapObject.md)|True|The map object the human is mounted on|
|MountedTransform|[Transform](../objects/Transform.md)|True|The transform the human is mounted on|
|AutoRefillGas|bool|False|Whether the human auto refills gas|
|State|string|True|The state of the human|
|CanDodge|bool|False|Whether the human can dodge|
|IsInvincible|bool|False|Whether the human is invincible|
|InvincibleTimeLeft|float|False|The time left for invincibility|
|IsCarried|bool|True|If the human is carried.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Refill() -> bool</code></pre>
> Refills the gas of the human
> 
<pre class="language-typescript"><code class="lang-typescript">function RefillImmediate()</code></pre>
> Refills the gas of the human immediately
> 
<pre class="language-typescript"><code class="lang-typescript">function ClearHooks()</code></pre>
> Clears all hooks
> 
<pre class="language-typescript"><code class="lang-typescript">function ClearLeftHook()</code></pre>
> Clears the left hook
> 
<pre class="language-typescript"><code class="lang-typescript">function ClearRightHook()</code></pre>
> Clears the right hook
> 
<pre class="language-typescript"><code class="lang-typescript">function LeftHookPosition() -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
> Position of the left hook, null if there is no hook.
> 
<pre class="language-typescript"><code class="lang-typescript">function RightHookPosition() -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
> Position of the right hook, null if there is no hook.
> 
<pre class="language-typescript"><code class="lang-typescript">function MountMapObject(mapObject: <a data-footnote-ref href="#user-content-fn-22">MapObject</a>, positionOffset: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotationOffset: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, canMountedAttack: bool = False)</code></pre>
> Mounts the human on a map object
> 
<pre class="language-typescript"><code class="lang-typescript">function MountTransform(transform: <a data-footnote-ref href="#user-content-fn-43">Transform</a>, positionOffset: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotationOffset: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, canMountedAttack: bool = False)</code></pre>
> Mounts the human on a transform
> 
<pre class="language-typescript"><code class="lang-typescript">function Unmount(immediate: bool = True)</code></pre>
> Unmounts the human
> 
<pre class="language-typescript"><code class="lang-typescript">function SetSpecial(special: string)</code></pre>
> Sets the special of the human
> 
<pre class="language-typescript"><code class="lang-typescript">function ActivateSpecial()</code></pre>
> Activates the special of the human
> 
<pre class="language-typescript"><code class="lang-typescript">function SetWeapon(weapon: string)</code></pre>
> Sets the weapon of the human
> 
> **Parameters**:
> - `weapon`: Name of the weapon. Available weapons: "Blade", "AHSS", "APG", "Thunderspear"
> 
<pre class="language-typescript"><code class="lang-typescript">function DisablePerks()</code></pre>
> Disables all perks of the human
> 

[^0]: [Animation](../objects/Animation.md)
[^1]: [Animator](../objects/Animator.md)
[^2]: [AudioSource](../objects/AudioSource.md)
[^3]: [Camera](../static/Camera.md)
[^4]: [Character](../objects/Character.md)
[^5]: [Collider](../objects/Collider.md)
[^6]: [Collision](../objects/Collision.md)
[^7]: [Color](../objects/Color.md)
[^8]: [Convert](../static/Convert.md)
[^9]: [Cutscene](../static/Cutscene.md)
[^10]: [Dict](../objects/Dict.md)
[^11]: [Game](../static/Game.md)
[^12]: [Human](../objects/Human.md)
[^13]: [Input](../static/Input.md)
[^14]: [Json](../static/Json.md)
[^15]: [LightBuiltin](../static/LightBuiltin.md)
[^16]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^17]: [LineRenderer](../objects/LineRenderer.md)
[^18]: [List](../objects/List.md)
[^19]: [Locale](../static/Locale.md)
[^20]: [LodBuiltin](../static/LodBuiltin.md)
[^21]: [Map](../static/Map.md)
[^22]: [MapObject](../objects/MapObject.md)
[^23]: [MapTargetable](../objects/MapTargetable.md)
[^24]: [Math](../static/Math.md)
[^25]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^26]: [Network](../static/Network.md)
[^27]: [NetworkView](../objects/NetworkView.md)
[^28]: [PersistentData](../static/PersistentData.md)
[^29]: [Physics](../static/Physics.md)
[^30]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^31]: [Player](../objects/Player.md)
[^32]: [Prefab](../objects/Prefab.md)
[^33]: [Quaternion](../objects/Quaternion.md)
[^34]: [Random](../objects/Random.md)
[^35]: [Range](../objects/Range.md)
[^36]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^37]: [RoomData](../static/RoomData.md)
[^38]: [Set](../objects/Set.md)
[^39]: [Shifter](../objects/Shifter.md)
[^40]: [String](../static/String.md)
[^41]: [Time](../static/Time.md)
[^42]: [Titan](../objects/Titan.md)
[^43]: [Transform](../objects/Transform.md)
[^44]: [UI](../static/UI.md)
[^45]: [Vector2](../objects/Vector2.md)
[^46]: [Vector3](../objects/Vector3.md)
[^47]: [WallColossal](../objects/WallColossal.md)
[^48]: [Button](../objects/Button.md)
[^49]: [Dropdown](../objects/Dropdown.md)
[^50]: [Label](../objects/Label.md)
[^51]: [ProgressBar](../objects/ProgressBar.md)
[^52]: [ScrollView](../objects/ScrollView.md)
[^53]: [Slider](../objects/Slider.md)
[^54]: [TextField](../objects/TextField.md)
[^55]: [Toggle](../objects/Toggle.md)
[^56]: [VisualElement](../objects/VisualElement.md)
[^57]: [Object](../objects/Object.md)
[^58]: [Component](../objects/Component.md)
