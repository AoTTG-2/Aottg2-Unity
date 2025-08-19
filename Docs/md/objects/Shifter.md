# Shifter
Inherits from [Character](../objects/Character.md)

Represents a Shifter character.
Only character owner can modify fields and call functions unless otherwise specified.

### Example
```csharp
function OnCharacterSpawn(character)
{
    if (character.IsMine && character.Type == "Shifter")
    {
        character.Size = 2;
        if (Network.MyPlayer.Status == "Alive" && Network.MyPlayer.Character.Type == "Human")
        {
            character.Target(Network.MyPlayer, 10);
        }
    }
}
```
### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Size|float|False|Shifter's size.|
|RunSpeedBase|float|False|Shifter's base run speed. Final run speed is RunSpeedBase + Size * RunSpeedPerLevel.|
|WalkSpeedBase|float|False|Shifter's base walk speed. Final walk speed is WalkSpeedBase + Size * WalkSpeedPerLevel.|
|WalkSpeedPerLevel|float|False|Shifter's walk speed added per level.|
|RunSpeedPerLevel|float|False|Shifter's run speed added per level.|
|TurnSpeed|float|False|Shifter's turn speed when running turn animation.|
|RotateSpeed|float|False|Shifter's rotate speed when rotating body.|
|JumpForce|float|False|Shifter's jump force when jumping.|
|ActionPause|float|False|Shifter's pause delay after performing an action.|
|AttackPause|float|False|Shifter's pause delay after performing an attack.|
|TurnPause|float|False|Shifter's pause delay after performing a turn.|
|DetectRange|float|False|(AI) shifter's detect range.|
|FocusRange|float|False|(AI) shifter's focus range.|
|FocusTime|float|False|(AI) shifter's focus time before switching targets.|
|FarAttackCooldown|float|True|(AI) Shifter's cooldown after performing a ranged attack.|
|AttackWait|float|True|(AI) Shifter's wait time between being in range to attack and performing the attack.|
|AttackSpeedMultiplier|float|False|Shifter's attack animation speed.|
|UsePathfinding|bool|False|(AI) Shifter uses pathfinding.|
|NapePosition|[Vector3](../objects/Vector3.md)|True|The shifter's nape position.|
|DeathAnimLength|float|False|The length of the death animation.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function MoveTo(position: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, range: float, ignoreEnemies: bool)</code></pre>
> Causes the (AI) shifter to move towards a position. If ignoreEnemies is true, will not engage enemies along the way.
> 
<pre class="language-typescript"><code class="lang-typescript">function Target(enemyObj: <a data-footnote-ref href="#user-content-fn-38">Object</a>, focus: float)</code></pre>
> Causes the (AI) shifter to target an enemy character or MapTargetable for focusTime seconds. If focusTime is 0 it will use the default focus time.
> 
<pre class="language-typescript"><code class="lang-typescript">function Idle(time: float)</code></pre>
> Causes the (AI) shifter to idle for time seconds before beginning to wander. During idle the titan will not react or move at all.
> 
<pre class="language-typescript"><code class="lang-typescript">function Wander()</code></pre>
> Causes the (AI) shifter to cancel any move commands and begin wandering randomly.
> 
<pre class="language-typescript"><code class="lang-typescript">function Blind()</code></pre>
> Causes the shifter to enter the blind state.
> 
<pre class="language-typescript"><code class="lang-typescript">function Cripple(time: float)</code></pre>
> Causes the shifter to enter the cripple state.
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
