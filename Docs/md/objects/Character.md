# Character
Inherits from [Object](../objects/Object.md)

Character is the base class that Human, Titan, and Shifter inherit from.
Only character owner can modify fields and call functions unless otherwise specified.

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Name|string|False|Character's name.|
|Guild|string|False|Character's guild.|
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


### Methods
<pre class="language-typescript"><code class="lang-typescript">function GetKilled(killer: string) -> null</code></pre>
> Kills the character. Callable by non-owners.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetDamaged(killer: string, damage: int) -> null</code></pre>
> Damages the character and kills it if its health reaches 0. Callable by non-owners.
> 
<pre class="language-typescript"><code class="lang-typescript">function Emote(emote: string) -> null</code></pre>
> Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.
> 
<pre class="language-typescript"><code class="lang-typescript">function PlayAnimation(animation: string, fade: float = 0.1) -> null</code></pre>
> Causes the character to play an animation.  If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function ForceAnimation(animation: string, fade: float = 0.1) -> null</code></pre>
> Forces the character to play an animation. If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationLength(animation: string) -> float</code></pre>
> Gets the length of animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function PlaySound(sound: string) -> null</code></pre>
> Plays a sound if present in the character. Available sound names can be found here: Humans, Shifters, Titans. Note that shifters also have all titan sounds.
> 
<pre class="language-typescript"><code class="lang-typescript">function StopSound(sound: string) -> null</code></pre>
> Stops the sound.
> 
<pre class="language-typescript"><code class="lang-typescript">function LookAt(position: <a data-footnote-ref href="#user-content-fn-36">Vector3</a>) -> null</code></pre>
> Rotates the character such that it is looking towards a world position.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddForce(force: <a data-footnote-ref href="#user-content-fn-36">Vector3</a>, mode: string = "Acceleration") -> null</code></pre>
> Adds a force to the character with given force vector and optional mode. Valid modes are Force, Acceleration, Impulse, VelocityChange with default being Acceleration.
> 
<pre class="language-typescript"><code class="lang-typescript">function Reveal(delay: float) -> null</code></pre>
> Reveal the titan for a set number of seconds.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddOutline(color: <a data-footnote-ref href="#user-content-fn-4">Color</a> = null, mode: string = "OutlineAll") -> null</code></pre>
> Adds an outline effect with the given color and mode. Valid modes are: OutlineAll, OutlineVisible, OutlineHidden, OutlineAndSilhouette, SilhouetteOnly, OutlineAndLightenColor
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveOutline() -> null</code></pre>
> Removes the outline effect from the character.
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
[^15]: [Map](../static/Map.md)
[^16]: [MapObject](../objects/MapObject.md)
[^17]: [MapTargetable](../objects/MapTargetable.md)
[^18]: [Math](../static/Math.md)
[^19]: [Network](../static/Network.md)
[^20]: [NetworkView](../objects/NetworkView.md)
[^21]: [PersistentData](../static/PersistentData.md)
[^22]: [Physics](../static/Physics.md)
[^23]: [Player](../objects/Player.md)
[^24]: [Quaternion](../objects/Quaternion.md)
[^25]: [Random](../objects/Random.md)
[^26]: [Range](../objects/Range.md)
[^27]: [RoomData](../static/RoomData.md)
[^28]: [Set](../objects/Set.md)
[^29]: [Shifter](../objects/Shifter.md)
[^30]: [String](../static/String.md)
[^31]: [Time](../static/Time.md)
[^32]: [Titan](../objects/Titan.md)
[^33]: [Transform](../objects/Transform.md)
[^34]: [UI](../static/UI.md)
[^35]: [Vector2](../objects/Vector2.md)
[^36]: [Vector3](../objects/Vector3.md)
[^37]: [Object](../objects/Object.md)
[^38]: [Component](../objects/Component.md)
