# Character
Inherits from [Object](../objects/Object.md)

Character is the base class that `Human`, `Titan`, and `Shifter` inherit from.
Only character owner can modify properties and call functions unless otherwise specified.

### Remarks
Overloads operators: 
- `==`
- `__Hash__`
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
<pre class="language-typescript"><code class="lang-typescript">function LookAt(position: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>)</code></pre>
> Rotates the character such that it is looking towards a world position.
> 
> **Parameters**:
> - `position`: Target world position
> 
<pre class="language-typescript"><code class="lang-typescript">function AddForce(force: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, mode: string = "Acceleration")</code></pre>
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
