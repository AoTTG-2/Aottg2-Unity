# Human
Inherits from [Character](../objects/Character.md)
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Name|[String](../static/String.md)|False|The human's name|
|Guild|[String](../static/String.md)|False|The human's guild|
|Weapon|[String](../static/String.md)|False|The weapon the human is using|
|CurrentSpecial|[String](../static/String.md)|False|The current special the human is using|
|SpecialCooldown|float|False|The cooldown of the special|
|ShifterLiveTime|float|False|The live time of the shifter special|
|SpecialCooldownRatio|float|False|The ratio of the special cooldown|
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
|IsMounted|bool|False|Whether the human is mounted|
|MountedMapObject|[MapObject](../objects/MapObject.md)|False|The map object the human is mounted on|
|MountedTransform|[Transform](../objects/Transform.md)|False|The transform the human is mounted on|
|AutoRefillGas|bool|False|Whether the human auto refills gas|
|State|[String](../static/String.md)|False|The state of the human|
|CanDodge|bool|False|Whether the human can dodge|
|IsInvincible|bool|False|Whether the human is invincible|
|InvincibleTimeLeft|float|False|The time left for invincibility|
|IsCarried|bool|False|If the human is carried.|
|Player|[Player](../objects/Player.md)|False|Player who owns this character.|
|IsAI|bool|False|Is this character AI?|
|ViewID|int|False|Network view ID of the character.|
|IsMine|bool|False|Is this character mine?|
|IsMainCharacter|bool|False||
|Transform|[Transform](../objects/Transform.md)|False|Unity transform of the character.|
|Position|[Vector3](../objects/Vector3.md)|False|Position of the character.|
|Rotation|[Vector3](../objects/Vector3.md)|False|Rotation of the character.|
|QuaternionRotation|[Quaternion](../objects/Quaternion.md)|False|Quaternion rotation of the character.|
|Velocity|[Vector3](../objects/Vector3.md)|False|Velocity of the character.|
|Forward|[Vector3](../objects/Vector3.md)|False|Forward direction of the character.|
|Right|[Vector3](../objects/Vector3.md)|False|Right direction of the character.|
|Up|[Vector3](../objects/Vector3.md)|False|Up direction of the character.|
|HasTargetDirection|bool|False|If the character has a target direction it is turning towards.|
|TargetDirection|[Vector3](../objects/Vector3.md)|False|The character's target direction.|
|Team|[String](../static/String.md)|False|Team character belongs to.|
|Health|float|False|Character's current health.|
|MaxHealth|float|False|Character's maximum health.|
|CustomDamageEnabled|bool|False|Is custom damage dealing enabled.|
|CustomDamage|int|False|Amount of custom damage to deal per attack.|
|CurrentAnimation|[String](../static/String.md)|False|Character's current playing animation.|
|Grounded|bool|False|Character's grounded status.|
## Methods
#### function <mark style="color:yellow;">Refill</mark>() -> <mark style="color:blue;">bool</mark>
> Refills the gas of the human

#### function <mark style="color:yellow;">RefillImmediate</mark>() -> <mark style="color:blue;">void</mark>
> Refills the gas of the human immediately

#### function <mark style="color:yellow;">ClearHooks</mark>() -> <mark style="color:blue;">void</mark>
> Clears all hooks

#### function <mark style="color:yellow;">ClearLeftHook</mark>() -> <mark style="color:blue;">void</mark>
> Clears the left hook

#### function <mark style="color:yellow;">ClearRightHook</mark>() -> <mark style="color:blue;">void</mark>
> Clears the right hook

#### function <mark style="color:yellow;">MountMapObject</mark>(mapObject: <mark style="color:blue;">[MapObject](../objects/MapObject.md)</mark>, positionOffset: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, rotationOffset: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) -> <mark style="color:blue;">void</mark>
> Mounts the human on a map object

#### function <mark style="color:yellow;">MountTransform</mark>(transform: <mark style="color:blue;">[Transform](../objects/Transform.md)</mark>, positionOffset: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, rotationOffset: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) -> <mark style="color:blue;">void</mark>
> Mounts the human on a transform

#### function <mark style="color:yellow;">Unmount</mark>() -> <mark style="color:blue;">void</mark>
> Unmounts the human

#### function <mark style="color:yellow;">SetSpecial</mark>(special: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Sets the special of the human

#### function <mark style="color:yellow;">ActivateSpecial</mark>() -> <mark style="color:blue;">void</mark>
> Activates the special of the human

#### function <mark style="color:yellow;">SetWeapon</mark>(weapon: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Sets the weapon of the human

#### function <mark style="color:yellow;">DisablePerks</mark>() -> <mark style="color:blue;">void</mark>
> Disables all perks of the human

#### function <mark style="color:yellow;">GetKilled</mark>(killer: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Kills the character. Callable by non-owners.

#### function <mark style="color:yellow;">GetDamaged</mark>(killer: <mark style="color:blue;">[String](../static/String.md)</mark>, damage: <mark style="color:blue;">int</mark>) -> <mark style="color:blue;">void</mark>
> Damages the character and kills it if its health reaches 0. Callable by non-owners.

#### function <mark style="color:yellow;">Emote</mark>(emote: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.

#### function <mark style="color:yellow;">PlayAnimation</mark>(animation: <mark style="color:blue;">[String](../static/String.md)</mark>, fade: <mark style="color:blue;">float</mark> = <mark style="color:blue;">0.1</mark>) -> <mark style="color:blue;">void</mark>
> Causes the character to play an animation.  If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.

#### function <mark style="color:yellow;">ForceAnimation</mark>(animation: <mark style="color:blue;">[String](../static/String.md)</mark>, fade: <mark style="color:blue;">float</mark> = <mark style="color:blue;">0.1</mark>) -> <mark style="color:blue;">void</mark>
> Forces the character to play an animation. If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.

#### function <mark style="color:yellow;">GetAnimationLength</mark>(animation: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">float</mark>
> Gets the length of animation.

#### function <mark style="color:yellow;">PlaySound</mark>(sound: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Plays a sound if present in the character. Available sound names can be found here: Humans, Shifters, Titans. Note that shifters also have all titan sounds.

#### function <mark style="color:yellow;">StopSound</mark>(sound: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Stops the sound.

#### function <mark style="color:yellow;">LookAt</mark>(position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) -> <mark style="color:blue;">void</mark>
> Rotates the character such that it is looking towards a world position.

#### function <mark style="color:yellow;">AddForce</mark>(force: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, mode: <mark style="color:blue;">[String](../static/String.md)</mark> = <mark style="color:blue;">Acceleration</mark>) -> <mark style="color:blue;">void</mark>
> Adds a force to the character with given force vector and optional mode. Valid modes are Force, Acceleration, Impulse, VelocityChange with default being Acceleration.

#### function <mark style="color:yellow;">Reveal</mark>(delay: <mark style="color:blue;">float</mark>) -> <mark style="color:blue;">void</mark>
> Reveaal the titan for a set number of seconds.

#### function <mark style="color:yellow;">AddOutline</mark>(color: <mark style="color:blue;">[Color](../objects/Color.md)</mark> = <mark style="color:blue;">null</mark>, mode: <mark style="color:blue;">[String](../static/String.md)</mark> = <mark style="color:blue;">OutlineAll</mark>) -> <mark style="color:blue;">void</mark>
> Adds an outline effect with the given color and mode. Valid modes are: OutlineAll, OutlineVisible, OutlineHidden, OutlineAndSilhouette, SilhouetteOnly, OutlineAndLightenColor

#### function <mark style="color:yellow;">RemoveOutline</mark>() -> <mark style="color:blue;">void</mark>
> Removes the outline effect from the character.


---

