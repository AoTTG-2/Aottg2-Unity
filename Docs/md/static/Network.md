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
<pre class="language-typescript"><code class="lang-typescript">function SendMessage(player: <a data-footnote-ref href="#user-content-fn-24">Player</a>, message: string)</code></pre>
> Send a message to a player
> 
<pre class="language-typescript"><code class="lang-typescript">function SendMessageAll(message: string)</code></pre>
> Send a message to all players
> 
<pre class="language-typescript"><code class="lang-typescript">function SendMessageOthers(message: string)</code></pre>
> Send a message to all players except the sender
> 
<pre class="language-typescript"><code class="lang-typescript">function GetTimestampDifference(timestamp1: Double, timestamp2: Double) -> Double</code></pre>
> Get the difference between two photon timestamps
> 
<pre class="language-typescript"><code class="lang-typescript">function KickPlayer(target: <a data-footnote-ref href="#user-content-fn-38">Object</a>, reason: string = ".")</code></pre>
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
