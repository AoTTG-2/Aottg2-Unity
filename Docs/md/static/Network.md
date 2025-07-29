# Network
Inherits from [Object](../objects/Object.md)

Networking functions.

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|IsMasterClient|bool|True|Is the player the master client|
|Players|[List](../objects/List.md)|True|The list of players in the room|
|MasterClient|[Player](../objects/Player.md)|True|The master client|
|MyPlayer|[Player](../objects/Player.md)|True|The local player|
|NetworkTime|Double|True|The network time|
|Ping|int|True|The local player's ping|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function SendMessage(player: <a data-footnote-ref href="#user-content-fn-23">Player</a>, message: string) -> null</code></pre>
> Send a message to a player
> 
<pre class="language-typescript"><code class="lang-typescript">function SendMessageAll(message: string) -> null</code></pre>
> Send a message to all players
> 
<pre class="language-typescript"><code class="lang-typescript">function SendMessageOthers(message: string) -> null</code></pre>
> Send a message to all players except the sender
> 
<pre class="language-typescript"><code class="lang-typescript">function GetTimestampDifference(timestamp1: Double, timestamp2: Double) -> Double</code></pre>
> Get the difference between two photon timestamps
> 
<pre class="language-typescript"><code class="lang-typescript">function KickPlayer(target: <a data-footnote-ref href="#user-content-fn-37">Object</a>, reason: string = ".") -> null</code></pre>
> Kick the given player by id or player reference.
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
