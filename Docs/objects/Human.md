# Human
Inherits from [Character](../objects/Character.md)

## Initialization
<mark style="color:red;">This class is abstract and cannot be instantiated.</mark>

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
<details>
<summary>Derived Fields</summary>

|Field|Type|Readonly|Description|
|---|---|---|---|
|Player|[Player](../objects/Player.md)|True|Player who owns this character.|
|IsAI|bool|True|Is this character AI?|
|ViewID|int|True|Network view ID of the character.|
|IsMine|bool|True|Is this character mine?|
|IsMainCharacter|bool|True|Character belongs to my player and is the main character (the camera-followed player).|
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
</details>

## Methods
###### function <mark style="color:yellow;">Refill</mark>() → <mark style="color:blue;">bool</mark>
> Refills the gas of the human

###### function <mark style="color:yellow;">RefillImmediate</mark>()
> Refills the gas of the human immediately

###### function <mark style="color:yellow;">ClearHooks</mark>()
> Clears all hooks

###### function <mark style="color:yellow;">ClearLeftHook</mark>()
> Clears the left hook

###### function <mark style="color:yellow;">ClearRightHook</mark>()
> Clears the right hook

###### function <mark style="color:yellow;">MountMapObject</mark>(mapObject: <mark style="color:blue;">[MapObject](../objects/MapObject.md)</mark>, positionOffset: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, rotationOffset: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>)
> Mounts the human on a map object

###### function <mark style="color:yellow;">MountTransform</mark>(transform: <mark style="color:blue;">[Transform](../objects/Transform.md)</mark>, positionOffset: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, rotationOffset: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>)
> Mounts the human on a transform

###### function <mark style="color:yellow;">Unmount</mark>()
> Unmounts the human

###### function <mark style="color:yellow;">SetSpecial</mark>(special: <mark style="color:blue;">string</mark>)
> Sets the special of the human

###### function <mark style="color:yellow;">ActivateSpecial</mark>()
> Activates the special of the human

###### function <mark style="color:yellow;">SetWeapon</mark>(weapon: <mark style="color:blue;">string</mark>)
> Sets the weapon of the human

###### function <mark style="color:yellow;">DisablePerks</mark>()
> Disables all perks of the human

###### function <mark style="color:yellow;">GetKilled</mark>(killer: <mark style="color:blue;">string</mark>)
> Kills the character. Callable by non-owners.

###### function <mark style="color:yellow;">GetDamaged</mark>(killer: <mark style="color:blue;">string</mark>, damage: <mark style="color:blue;">int</mark>)
> Damages the character and kills it if its health reaches 0. Callable by non-owners.

###### function <mark style="color:yellow;">Emote</mark>(emote: <mark style="color:blue;">string</mark>)
> Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.

###### function <mark style="color:yellow;">PlayAnimation</mark>(animation: <mark style="color:blue;">string</mark>, fade: <mark style="color:blue;">float</mark> = <mark style="color:blue;">0.1</mark>)
> Causes the character to play an animation.  If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.

###### function <mark style="color:yellow;">ForceAnimation</mark>(animation: <mark style="color:blue;">string</mark>, fade: <mark style="color:blue;">float</mark> = <mark style="color:blue;">0.1</mark>)
> Forces the character to play an animation. If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.

###### function <mark style="color:yellow;">GetAnimationLength</mark>(animation: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">float</mark>
> Gets the length of animation.

###### function <mark style="color:yellow;">PlaySound</mark>(sound: <mark style="color:blue;">string</mark>)
> Plays a sound if present in the character. Available sound names can be found here: Humans, Shifters, Titans. Note that shifters also have all titan sounds.

###### function <mark style="color:yellow;">StopSound</mark>(sound: <mark style="color:blue;">string</mark>)
> Stops the sound.

###### function <mark style="color:yellow;">LookAt</mark>(position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>)
> Rotates the character such that it is looking towards a world position.

###### function <mark style="color:yellow;">AddForce</mark>(force: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, mode: <mark style="color:blue;">string</mark> = <mark style="color:blue;">Acceleration</mark>)
> Adds a force to the character with given force vector and optional mode. Valid modes are Force, Acceleration, Impulse, VelocityChange with default being Acceleration.

###### function <mark style="color:yellow;">Reveal</mark>(delay: <mark style="color:blue;">float</mark>)
> Reveal the titan for a set number of seconds.

###### function <mark style="color:yellow;">AddOutline</mark>(color: <mark style="color:blue;">[Color](../objects/Color.md)</mark> = <mark style="color:blue;">null</mark>, mode: <mark style="color:blue;">string</mark> = <mark style="color:blue;">OutlineAll</mark>)
> Adds an outline effect with the given color and mode. Valid modes are: OutlineAll, OutlineVisible, OutlineHidden, OutlineAndSilhouette, SilhouetteOnly, OutlineAndLightenColor

###### function <mark style="color:yellow;">RemoveOutline</mark>()
> Removes the outline effect from the character.


---

