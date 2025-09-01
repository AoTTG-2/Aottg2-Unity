# Character
Inherits from [Object](../objects/Object.md)

Character is the base class that `Human`, `Titan`, and `Shifter` inherit from.
Only character owner can modify properties and call functions unless otherwise specified.

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


### Methods
<pre class="language-typescript"><code class="lang-typescript">function GetKilled(killer: string)</code></pre>
> Kills the character. Callable by non-owners.
> 
> **Parameters**:
> - `killer`: Killer name
> 
<pre class="language-typescript"><code class="lang-typescript">function GetDamaged(killer: string, damage: int)</code></pre>
> Damages the character and kills it if its health reaches 0. Callable by non-owners.
> 
> **Parameters**:
> - `killer`: Killer name
> - `damage`: Damage amount
> 
<pre class="language-typescript"><code class="lang-typescript">function Emote(emote: string)</code></pre>
> Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.
> 
<pre class="language-typescript"><code class="lang-typescript">function PlayAnimation(animation: string, fade: float = 0.1)</code></pre>
> Causes the character to play an animation.
> 
> **Parameters**:
> - `animation`: Name of the animation.
Available animations can be found here: [Human](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Human/HumanAnimations.cs), [Titan](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Titan/BasicTitanAnimations.cs), [Annie](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Shifters/Annie/AnnieAnimations.cs), [Eren](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Shifters/Eren/ErenAnimations.cs)

Use the right-hand string value for the animation.

Note that shifters also have all titan animations.
> - `fade`: Fade time. If provided, will crossfade the animation by this timestep
> 
<pre class="language-typescript"><code class="lang-typescript">function PlayAnimationAt(animation: string, t: float, fade: float = 0.1, force: bool = False)</code></pre>
> Causes the character to play an animation at a specific time.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationSpeed(animation: string)</code></pre>
> Gets the animation speed of a given animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetAnimationSpeed(animation: string, speed: float, synced: bool = True)</code></pre>
> Sets the animation speed of a given animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsPlayingAnimation(animation: string) -> bool</code></pre>
> Returns true if the animation is playing.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationNormalizedTime(animation: string) -> float</code></pre>
> Returns true if the animation is playing.
> 
<pre class="language-typescript"><code class="lang-typescript">function ForceAnimation(animation: string, fade: float = 0.1)</code></pre>
> Forces the character to play an animation.
> 
> **Parameters**:
> - `animation`: Name of the animation.
Available animations can be found here: [Human](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Human/HumanAnimations.cs), [Titan](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Titan/BasicTitanAnimations.cs), [Annie](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Shifters/Annie/AnnieAnimations.cs), [Eren](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Shifters/Eren/ErenAnimations.cs)

Use the right-hand string value for the animation.

Note that shifters also have all titan animations.
> - `fade`: Fade time. If provided, will crossfade the animation by this timestep
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationLength(animation: string) -> float</code></pre>
> Gets the length of animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsPlayingSound(sound: string) -> bool</code></pre>
> Returns true if the character is playing a sound. Available sound names can be found here: Humans, Shifters, Titans. Note that shifters also have all titan sounds.
> 
<pre class="language-typescript"><code class="lang-typescript">function PlaySound(sound: string)</code></pre>
> Plays a sound if present in the character.
> 
> **Parameters**:
> - `sound`: Name of the sound to play.
Available sound names can be found here: [Human](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/refs/heads/main/Assets/Scripts/Characters/Human/HumanSounds.cs), [Shifters](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/refs/heads/main/Assets/Scripts/Characters/Shifters/ShifterSounds.cs), [Titans](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/refs/heads/main/Assets/Scripts/Characters/Titan/TitanSounds.cs).

Note that shifters also have all titan sounds
> 
<pre class="language-typescript"><code class="lang-typescript">function StopSound(sound: string)</code></pre>
> Stops a sound if present in the character.
> 
> **Parameters**:
> - `sound`: Name of the sound to stop.
> 
<pre class="language-typescript"><code class="lang-typescript">function FadeSound(sound: string, volume: float, time: float)</code></pre>
> Fades the sound volume to a specific volume between 0.0 and 1.0 over [time] seconds. Does not play or stop the sound.
> 
<pre class="language-typescript"><code class="lang-typescript">function LookAt(position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>)</code></pre>
> Rotates the character such that it is looking towards a world position.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddForce(force: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, mode: string = "Acceleration")</code></pre>
> Adds a force to the character with given force vector and optional mode.
> 
> **Parameters**:
> - `force`: Force vector
> - `mode`: Force mode. Valid modes are Force, Acceleration, Impulse, VelocityChange
> 
<pre class="language-typescript"><code class="lang-typescript">function Reveal(delay: float)</code></pre>
> Reveal the titan for a set number of seconds.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddOutline(color: <a data-footnote-ref href="#user-content-fn-4">Color</a> = null, mode: string = "OutlineAll")</code></pre>
> Adds an outline effect with the given color and mode.
> 
> **Parameters**:
> - `color`: Outline color
> - `mode`: Outline mode. Valid modes are: OutlineAll, OutlineVisible, OutlineHidden, OutlineAndSilhouette, SilhouetteOnly, OutlineAndLightenColor
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveOutline()</code></pre>
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
[^12]: [LightBuiltin](../static/LightBuiltin.md)
[^13]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^14]: [LineRenderer](../objects/LineRenderer.md)
[^15]: [List](../objects/List.md)
[^16]: [Locale](../static/Locale.md)
[^17]: [LodBuiltin](../static/LodBuiltin.md)
[^18]: [Map](../static/Map.md)
[^19]: [MapObject](../objects/MapObject.md)
[^20]: [MapTargetable](../objects/MapTargetable.md)
[^21]: [Math](../static/Math.md)
[^22]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^23]: [Network](../static/Network.md)
[^24]: [NetworkView](../objects/NetworkView.md)
[^25]: [PersistentData](../static/PersistentData.md)
[^26]: [Physics](../static/Physics.md)
[^27]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^28]: [Player](../objects/Player.md)
[^29]: [Prefab](../objects/Prefab.md)
[^30]: [Quaternion](../objects/Quaternion.md)
[^31]: [Random](../objects/Random.md)
[^32]: [Range](../objects/Range.md)
[^33]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^34]: [RoomData](../static/RoomData.md)
[^35]: [Set](../objects/Set.md)
[^36]: [Shifter](../objects/Shifter.md)
[^37]: [String](../static/String.md)
[^38]: [Time](../static/Time.md)
[^39]: [Titan](../objects/Titan.md)
[^40]: [Transform](../objects/Transform.md)
[^41]: [UI](../static/UI.md)
[^42]: [Vector2](../objects/Vector2.md)
[^43]: [Vector3](../objects/Vector3.md)
[^44]: [WallColossal](../objects/WallColossal.md)
[^45]: [Object](../objects/Object.md)
[^46]: [Component](../objects/Component.md)
