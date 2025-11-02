# Titan
Inherits from [Character](../objects/Character.md)

Represents a Titan character.
Only character owner can modify fields and call functions unless otherwise specified.

### Example
```csharp
function OnCharacterSpawn(character)
{
    if (character.IsMine && character.Type == "Titan")
    {
        character.Size = 3;
        character.DetectRange = 1000;
        character.Blind();
    }
}
```
### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Size|float|False|Titan's size.|
|RunSpeedBase|float|False|Titan's base run speed. Final run speed is RunSpeedBase + Size * RunSpeedPerLevel.|
|WalkSpeedBase|float|False|Titan's base walk speed. Final walk speed is WalkSpeedBase + Size * WalkSpeedPerLevel.|
|WalkSpeedPerLevel|float|False|Titan's walk speed added per size.|
|RunSpeedPerLevel|float|False|Titan's run speed added per size.|
|TurnSpeed|float|False|Titan's turn animation speed.|
|RotateSpeed|float|False|Titan's rotate speed.|
|JumpForce|float|False|Titan's jump force.|
|ActionPause|float|False|Titan's pause delay after performing an action.|
|AttackPause|float|False|Titan's pause delay after performing an attack.|
|TurnPause|float|False|Titan's pause delay after performing a turn.|
|Stamina|float|False|PT stamina.|
|MaxStamina|float|False|PT max stamina.|
|NapePosition|[Vector3](../objects/Vector3.md)|True|The titan's nape position.|
|IsCrawler|bool|True|Is titan a crawler.|
|DetectRange|float|False|(AI) titan's detect range.|
|FocusRange|float|False|(AI) titan's focus range.|
|FocusTime|float|False|(AI) titan's focus time.|
|FarAttackCooldown|float|True|(AI) Titan's cooldown after performing a ranged attack.|
|AttackWait|float|False|(AI) Titan's wait time between being in range and performing an attack.|
|CanRun|bool|False|(AI) Titan can run or only walk.|
|AttackSpeedMultiplier|float|False|Titan's attack animation speed.|
|UsePathfinding|bool|False|Determines whether the (AI) titan uses pathfinding. (Smart Movement in titan settings)|
|HeadMount|[Transform](../objects/Transform.md)|True|Titan's head transform.|
|NeckMount|[Transform](../objects/Transform.md)|True|Titan's neck transform.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function MoveTo(position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, range: float, ignoreEnemies: bool)</code></pre>
> Causes the (AI) titan to move towards a position and stopping when within specified range. If ignoreEnemies is true, will not engage enemies along the way.
> 
<pre class="language-typescript"><code class="lang-typescript">function Target(enemyObj: <a data-footnote-ref href="#user-content-fn-57">Object</a>, focus: float)</code></pre>
> Causes the (AI) titan to target an enemy character or MapTargetable for focusTime seconds. If focusTime is 0 it will use the default focus time
> 
<pre class="language-typescript"><code class="lang-typescript">function GetTarget() -> <a data-footnote-ref href="#user-content-fn-57">Object</a></code></pre>
> Gets the target currently focused by this character. Returns null if no target is set.
> 
<pre class="language-typescript"><code class="lang-typescript">function Idle(time: float)</code></pre>
> Causes the (AI) titan to idle for time seconds before beginning to wander. During idle the titan will not react or move at all.
> 
<pre class="language-typescript"><code class="lang-typescript">function Wander()</code></pre>
> Causes the (AI) titan to cancel any move commands and begin wandering randomly.
> 
<pre class="language-typescript"><code class="lang-typescript">function Blind()</code></pre>
> Causes the titan to enter the blind state.
> 
<pre class="language-typescript"><code class="lang-typescript">function Cripple(time: float)</code></pre>
> Causes the titan to enter the cripple state for time seconds. Using 0 will use the default cripple time.
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
