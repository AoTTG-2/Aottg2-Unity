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
<pre class="language-typescript"><code class="lang-typescript">function MountMapObject(mapObject: <a data-footnote-ref href="#user-content-fn-17">MapObject</a>, positionOffset: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, rotationOffset: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, canMountedAttack: bool = False)</code></pre>
> Mounts the human on a map object
> 
<pre class="language-typescript"><code class="lang-typescript">function MountTransform(transform: <a data-footnote-ref href="#user-content-fn-34">Transform</a>, positionOffset: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, rotationOffset: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, canMountedAttack: bool = False)</code></pre>
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
[^12]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^13]: [LineRenderer](../objects/LineRenderer.md)
[^14]: [List](../objects/List.md)
[^15]: [Locale](../static/Locale.md)
[^16]: [Map](../static/Map.md)
[^17]: [MapObject](../objects/MapObject.md)
[^18]: [MapTargetable](../objects/MapTargetable.md)
[^19]: [Math](../static/Math.md)
[^20]: [Network](../static/Network.md)
[^21]: [NetworkView](../objects/NetworkView.md)
[^22]: [PersistentData](../static/PersistentData.md)
[^23]: [Physics](../static/Physics.md)
[^24]: [Player](../objects/Player.md)
[^25]: [Quaternion](../objects/Quaternion.md)
[^26]: [Random](../objects/Random.md)
[^27]: [Range](../objects/Range.md)
[^28]: [RoomData](../static/RoomData.md)
[^29]: [Set](../objects/Set.md)
[^30]: [Shifter](../objects/Shifter.md)
[^31]: [String](../static/String.md)
[^32]: [Time](../static/Time.md)
[^33]: [Titan](../objects/Titan.md)
[^34]: [Transform](../objects/Transform.md)
[^35]: [UI](../static/UI.md)
[^36]: [Vector2](../objects/Vector2.md)
[^37]: [Vector3](../objects/Vector3.md)
[^38]: [Object](../objects/Object.md)
[^39]: [Component](../objects/Component.md)
