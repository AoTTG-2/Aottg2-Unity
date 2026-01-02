# Character
Inherits from [Object](../objects/Object.md)

Character is the base class that `Human`, `Titan`, and `Shifter` inherit from. Only character owner can modify properties and call functions unless otherwise specified.

### Remarks
Overloads operators: 
`==`, `__Hash__`
### Example
```csharp
function OnCharacterSpawn(character)
{
    if (character.IsMine)
    {
        # Character is owned (network-wise) by the person running this script.
        # Ex: If user is host, this could either be their actual player character or AI titans/shifters.
    }
    
    if (character.IsMainCharacter)
    {
        # Character is the main character (the camera-followed player).
    }
    
    if (character.IsAI)
    {
        # Character is AI and likely controlled via MasterClient.
        
        if (character.Player.ID == Network.MasterClient.ID)
        {
            # Character is owned by masterclient, if we're not masterclient, we cannot modify props.    
        }
    }
}
```
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
|Rigidbody|[RigidbodyBuiltin](../static/RigidbodyBuiltin.md)|True|Character's rigidbody component (if available).|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function GetKilled(killer: string)</code></pre>
> Kills the character.
> 
> **Parameters**:
> - `killer`: Killer name.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetDamaged(killer: string, damage: int)</code></pre>
> Damages the character.
> 
> **Parameters**:
> - `killer`: Killer name.
> - `damage`: Damage amount.
> 
<pre class="language-typescript"><code class="lang-typescript">function Emote(emote: string)</code></pre>
> Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.
> 
> **Parameters**:
> - `emote`: Name of the emote to play.
> 
<pre class="language-typescript"><code class="lang-typescript">function PlayAnimation(animation: string, fade: float = 0.1)</code></pre>
> Causes the character to play an animation.
> 
> **Parameters**:
> - `animation`: Name of the animation. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation. Note that shifters also have all titan animations.
> - `fade`: Fade time. If provided, will crossfade the animation by this timestep.
> 
<pre class="language-typescript"><code class="lang-typescript">function PlayAnimationAt(animation: string, t: float, fade: float = 0.1, force: bool = False)</code></pre>
> Causes the character to play an animation at a specific time.
> 
> **Parameters**:
> - `animation`: Name of the animation.
> - `t`: Time in the animation to start playing.
> - `fade`: Fade time.
> - `force`: Whether to force the animation even if it's already playing.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationSpeed(animation: string) -> float</code></pre>
> Gets the animation speed of a given animation. Returns 1.0 if the character is not owned by the player or is dead.
> 
> **Parameters**:
> - `animation`: Name of the animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetAnimationSpeed(animation: string, speed: float, synced: bool = True)</code></pre>
> Sets the animation speed of a given animation.
> 
> **Parameters**:
> - `animation`: Name of the animation.
> - `speed`: The animation speed multiplier.
> - `synced`: Whether to sync the speed across the network.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsPlayingAnimation(animation: string) -> bool</code></pre>
> Returns true if the animation is playing. Returns: True if the animation is playing, false otherwise.
> 
> **Parameters**:
> - `animation`: Name of the animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationNormalizedTime(animation: string) -> float</code></pre>
> Gets the normalized time of the currently playing animation. Returns: The normalized time (0-1) of the animation.
> 
> **Parameters**:
> - `animation`: Name of the animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function ForceAnimation(animation: string, fade: float = 0.1)</code></pre>
> Forces the character to play an animation, even if it's already playing.
> 
> **Parameters**:
> - `animation`: Name of the animation. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation. Note that shifters also have all titan animations.
> - `fade`: Fade time. If provided, will crossfade the animation by this timestep.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationLength(animation: string) -> float</code></pre>
> Gets the length of animation. Returns: The length of the animation in seconds.
> 
> **Parameters**:
> - `animation`: Name of the animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsPlayingSound(sound: string) -> bool</code></pre>
> Returns true if the character is playing a sound. Returns: True if the sound is playing, false otherwise. Available sound names can be found here: Humans, Shifters, Titans. Note that shifters also have all titan sounds.
> 
> **Parameters**:
> - `sound`: Name of the sound.
> 
<pre class="language-typescript"><code class="lang-typescript">function PlaySound(sound: string)</code></pre>
> Plays a sound for the character.
> 
> **Parameters**:
> - `sound`: Name of the sound to play. Available sound names can be found here: Human, Shifters, Titans. Note that shifters also have all titan sounds.
> 
<pre class="language-typescript"><code class="lang-typescript">function StopSound(sound: string)</code></pre>
> Stops a sound for the character.
> 
> **Parameters**:
> - `sound`: Name of the sound to stop.
> 
<pre class="language-typescript"><code class="lang-typescript">function FadeSound(sound: string, volume: float, time: float)</code></pre>
> Fades the sound volume to a specific volume between 0.0 and 1.0 over [time] seconds. Does not play or stop the sound.
> 
> **Parameters**:
> - `sound`: Name of the sound.
> - `volume`: Target volume (0.0 to 1.0).
> - `time`: Time in seconds to fade over.
> 
<pre class="language-typescript"><code class="lang-typescript">function LookAt(position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>)</code></pre>
> Rotates the character such that it is looking towards a world position.
> 
> **Parameters**:
> - `position`: The world position to look at.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddForce(force: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, mode: string = "Acceleration")</code></pre>
> Adds a force to the character with given force vector and optional mode.
> 
> **Parameters**:
> - `force`: Force vector.
> - `mode`: Force mode. Valid modes are Force, Acceleration, Impulse, VelocityChange.
> 
<pre class="language-typescript"><code class="lang-typescript">function Reveal(delay: float)</code></pre>
> Reveal the titan for a set number of seconds.
> 
> **Parameters**:
> - `delay`: Delay in seconds before revealing.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddOutline(color: <a data-footnote-ref href="#user-content-fn-0">Color</a> = null, mode: string = "OutlineAll")</code></pre>
> Adds an outline effect with the given color and mode.
> 
> **Parameters**:
> - `color`: Outline color.
> - `mode`: Outline mode. Valid modes are: OutlineAll, OutlineVisible, OutlineHidden, OutlineAndSilhouette, SilhouetteOnly, OutlineAndLightenColor.
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveOutline()</code></pre>
> Removes the outline effect from the character.
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
[^31]: [Camera](../static/Camera.md)
[^32]: [Cutscene](../static/Cutscene.md)
[^33]: [Game](../static/Game.md)
[^34]: [Input](../static/Input.md)
[^35]: [Locale](../static/Locale.md)
[^36]: [Map](../static/Map.md)
[^37]: [Network](../static/Network.md)
[^38]: [PersistentData](../static/PersistentData.md)
[^39]: [Physics](../static/Physics.md)
[^40]: [RoomData](../static/RoomData.md)
[^41]: [Time](../static/Time.md)
[^42]: [Button](../objects/Button.md)
[^43]: [Dropdown](../objects/Dropdown.md)
[^44]: [Icon](../objects/Icon.md)
[^45]: [Image](../objects/Image.md)
[^46]: [Label](../objects/Label.md)
[^47]: [ProgressBar](../objects/ProgressBar.md)
[^48]: [ScrollView](../objects/ScrollView.md)
[^49]: [Slider](../objects/Slider.md)
[^50]: [TextField](../objects/TextField.md)
[^51]: [Toggle](../objects/Toggle.md)
[^52]: [UI](../static/UI.md)
[^53]: [VisualElement](../objects/VisualElement.md)
[^54]: [Convert](../static/Convert.md)
[^55]: [Json](../static/Json.md)
[^56]: [Math](../static/Math.md)
[^57]: [Random](../objects/Random.md)
[^58]: [String](../static/String.md)
[^59]: [Object](../objects/Object.md)
[^60]: [Component](../objects/Component.md)
