# Human
Inherits from [Character](../objects/Character.md)
## Initialization
<span style="color:red;">This class is abstract and cannot be instantiated.</span>
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Name|string|False|The human's name|
|Guild|string|False|The human's guild|
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
|Player|[Player](../objects/Player.md)|True|Player who owns this character.|
|IsAI|bool|True|Is this character AI?|
|ViewID|int|True|Network view ID of the character.|
|IsMine|bool|True|Is this character mine?|
|IsMainCharacter|bool|True||
|Transform|[Transform](../objects/Transform.md)|True|Unity transform of the character.|
|Position|[Vector3](../objects/Vector3.md)|False|Position of the character.|
|Rotation|[Vector3](../objects/Vector3.md)|False|Rotation of the character.|
|QuaternionRotation|[Quaternion](../objects/Quaternion.md)|False|Quaternion rotation of the character.|
|Velocity|[Vector3](../objects/Vector3.md)|False|Velocity of the character.|
|Forward|[Vector3](../objects/Vector3.md)|False|Forward direction of the character.|
|Right|[Vector3](../objects/Vector3.md)|False|Right direction of the character.|
|Up|[Vector3](../objects/Vector3.md)|False|Up direction of the character.|
|HasTargetDirection|bool|True|If the character has a target direction it is turning towards.|
|TargetDirection|[Vector3](../objects/Vector3.md)|True|The character's target direction.|
|Team|string|False|Team character belongs to.|
|Health|float|False|Character's current health.|
|MaxHealth|float|False|Character's maximum health.|
|CustomDamageEnabled|bool|False|Is custom damage dealing enabled.|
|CustomDamage|int|False|Amount of custom damage to deal per attack.|
|CurrentAnimation|string|True|Character's current playing animation.|
|Grounded|bool|True|Character's grounded status.|
## Methods
#### function <span style="color:yellow;">Refill</span>() → <span style="color:blue;">bool</span>
> Refills the gas of the human

#### function <span style="color:yellow;">RefillImmediate</span>() → <span style="color:blue;">null</span>
> Refills the gas of the human immediately

#### function <span style="color:yellow;">ClearHooks</span>() → <span style="color:blue;">null</span>
> Clears all hooks

#### function <span style="color:yellow;">ClearLeftHook</span>() → <span style="color:blue;">null</span>
> Clears the left hook

#### function <span style="color:yellow;">ClearRightHook</span>() → <span style="color:blue;">null</span>
> Clears the right hook

#### function <span style="color:yellow;">MountMapObject</span>(mapObject: <span style="color:blue;">[MapObject](../objects/MapObject.md)</span>, positionOffset: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, rotationOffset: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">null</span>
> Mounts the human on a map object

#### function <span style="color:yellow;">MountTransform</span>(transform: <span style="color:blue;">[Transform](../objects/Transform.md)</span>, positionOffset: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, rotationOffset: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">null</span>
> Mounts the human on a transform

#### function <span style="color:yellow;">Unmount</span>() → <span style="color:blue;">null</span>
> Unmounts the human

#### function <span style="color:yellow;">SetSpecial</span>(special: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Sets the special of the human

#### function <span style="color:yellow;">ActivateSpecial</span>() → <span style="color:blue;">null</span>
> Activates the special of the human

#### function <span style="color:yellow;">SetWeapon</span>(weapon: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Sets the weapon of the human

#### function <span style="color:yellow;">DisablePerks</span>() → <span style="color:blue;">null</span>
> Disables all perks of the human

#### function <span style="color:yellow;">GetKilled</span>(killer: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Kills the character. Callable by non-owners.

#### function <span style="color:yellow;">GetDamaged</span>(killer: <span style="color:blue;">string</span>, damage: <span style="color:blue;">int</span>) → <span style="color:blue;">null</span>
> Damages the character and kills it if its health reaches 0. Callable by non-owners.

#### function <span style="color:yellow;">Emote</span>(emote: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.

#### function <span style="color:yellow;">PlayAnimation</span>(animation: <span style="color:blue;">string</span>, fade: <span style="color:blue;">float</span> = <span style="color:blue;">0.1</span>) → <span style="color:blue;">null</span>
> Causes the character to play an animation.  If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.

#### function <span style="color:yellow;">ForceAnimation</span>(animation: <span style="color:blue;">string</span>, fade: <span style="color:blue;">float</span> = <span style="color:blue;">0.1</span>) → <span style="color:blue;">null</span>
> Forces the character to play an animation. If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.

#### function <span style="color:yellow;">GetAnimationLength</span>(animation: <span style="color:blue;">string</span>) → <span style="color:blue;">float</span>
> Gets the length of animation.

#### function <span style="color:yellow;">PlaySound</span>(sound: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Plays a sound if present in the character. Available sound names can be found here: Humans, Shifters, Titans. Note that shifters also have all titan sounds.

#### function <span style="color:yellow;">StopSound</span>(sound: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Stops the sound.

#### function <span style="color:yellow;">LookAt</span>(position: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">null</span>
> Rotates the character such that it is looking towards a world position.

#### function <span style="color:yellow;">AddForce</span>(force: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, mode: <span style="color:blue;">string</span> = <span style="color:blue;">Acceleration</span>) → <span style="color:blue;">null</span>
> Adds a force to the character with given force vector and optional mode. Valid modes are Force, Acceleration, Impulse, VelocityChange with default being Acceleration.

#### function <span style="color:yellow;">Reveal</span>(delay: <span style="color:blue;">float</span>) → <span style="color:blue;">null</span>
> Reveaal the titan for a set number of seconds.

#### function <span style="color:yellow;">AddOutline</span>(color: <span style="color:blue;">[Color](../objects/Color.md)</span> = <span style="color:blue;">null</span>, mode: <span style="color:blue;">string</span> = <span style="color:blue;">OutlineAll</span>) → <span style="color:blue;">null</span>
> Adds an outline effect with the given color and mode. Valid modes are: OutlineAll, OutlineVisible, OutlineHidden, OutlineAndSilhouette, SilhouetteOnly, OutlineAndLightenColor

#### function <span style="color:yellow;">RemoveOutline</span>() → <span style="color:blue;">null</span>
> Removes the outline effect from the character.


---

