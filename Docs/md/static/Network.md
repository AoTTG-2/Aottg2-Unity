# Network
Inherits from [Object](../objects/Object.md)

Networking functions.

### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|IsMasterClient|bool|True|Is the player the master client.|
|Players|[List](../objects/List.md)<[Player](../objects/Player.md)>|True|The list of players in the room.|
|MasterClient|[Player](../objects/Player.md)|True|The master client.|
|MyPlayer|[Player](../objects/Player.md)|True|The local player.|
|NetworkTime|double|True|The network time.|
|Ping|int|True|The local player's ping.|


### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function SendMessage(player: <a data-footnote-ref href="#user-content-fn-25">Player</a>, message: string)</code></pre>
> Send a message to a player.
> 
<pre class="language-typescript"><code class="lang-typescript">function SendMessageAll(message: string)</code></pre>
> Send a message to all players.
> 
<pre class="language-typescript"><code class="lang-typescript">function SendMessageOthers(message: string)</code></pre>
> Send a message to all players except the sender.
> 
<pre class="language-typescript"><code class="lang-typescript">function FindPlayer(id: int) -> <a data-footnote-ref href="#user-content-fn-25">Player</a></code></pre>
> Finds a player in the room by id.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetTimestampDifference(timestamp1: double, timestamp2: double) -> double</code></pre>
> Get the difference between two photon timestamps.
> 
<pre class="language-typescript"><code class="lang-typescript">function KickPlayer(target: <a data-footnote-ref href="#user-content-fn-59">Object</a>, reason: string = ".")</code></pre>
> Kick the given player by id or player reference.
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
