# Game
Inherits from [Object](../objects/Object.md)

Game functions such as spawning titans and managing game state.

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|IsEnding|bool|True|Is the game ending?|
|EndTimeLeft|float|True|Time left until the game ends|
|Titans|[List](../objects/List.md)|True|List of all titans|
|AITitans|[List](../objects/List.md)|True|List of all AI titans|
|PlayerTitans|[List](../objects/List.md)|True|List of all player titans|
|Shifters|[List](../objects/List.md)|True|List of all shifters|
|AIShifters|[List](../objects/List.md)|True|List of all AI shifters|
|PlayerShifters|[List](../objects/List.md)|True|List of all player shifters|
|Humans|[List](../objects/List.md)|True|List of all humans|
|AIHumans|[List](../objects/List.md)|True|List of all AI humans|
|PlayerHumans|[List](../objects/List.md)|True|List of all player humans|
|Loadouts|[List](../objects/List.md)|True|List of all loadouts|
|DefaultShowKillScore|bool|False|Is the kill score shown by default?|
|DefaultHideKillScore|bool|False|Is the kill feed shown by default?|
|DefaultAddKillScore|bool|False|Is the kill score added by default?|
|ShowScoreboardLoadout|bool|False|Is the loadout shown in the scoreboard?|
|ShowScoreboardStatus|bool|False|Is the status shown in the scoreboard?|
|ForcedCharacterType|string|False|Forced character type|
|ForcedLoadout|string|False|Forced loadout|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Debug(message: <a data-footnote-ref href="#user-content-fn-38">Object</a>)</code></pre>
> Print a debug statement to the console
> 
<pre class="language-typescript"><code class="lang-typescript">function Print(message: <a data-footnote-ref href="#user-content-fn-38">Object</a>)</code></pre>
> Print a message to the chat
> 
<pre class="language-typescript"><code class="lang-typescript">function PrintAll(message: <a data-footnote-ref href="#user-content-fn-38">Object</a>)</code></pre>
> Print a message to all players
> 
<pre class="language-typescript"><code class="lang-typescript">function GetGeneralSetting(settingName: string) -> <a data-footnote-ref href="#user-content-fn-38">Object</a></code></pre>
> Get a general setting
> 
<pre class="language-typescript"><code class="lang-typescript">function GetTitanSetting(settingName: string) -> <a data-footnote-ref href="#user-content-fn-38">Object</a></code></pre>
> Get a titan setting
> 
<pre class="language-typescript"><code class="lang-typescript">function GetMiscSetting(settingName: string) -> <a data-footnote-ref href="#user-content-fn-38">Object</a></code></pre>
> Get a misc setting
> 
<pre class="language-typescript"><code class="lang-typescript">function End(delay: float)</code></pre>
> End the game
> 
<pre class="language-typescript"><code class="lang-typescript">function FindCharacterByViewID(viewID: int) -> <a data-footnote-ref href="#user-content-fn-1">Character</a></code></pre>
> Find a character by view ID
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitan(type: string) -> <a data-footnote-ref href="#user-content-fn-33">Titan</a></code></pre>
> Spawn a titan
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitanAt(type: string, position: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, rotationY: float = 0) -> <a data-footnote-ref href="#user-content-fn-33">Titan</a></code></pre>
> Spawn a titan at a position
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitans(type: string, count: int) -> <a data-footnote-ref href="#user-content-fn-14">List</a></code></pre>
> Spawn titans
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitansAsync(type: string, count: int)</code></pre>
> Spawn titans asynchronously
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitansAt(type: string, count: int, position: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, rotationY: float = 0) -> <a data-footnote-ref href="#user-content-fn-14">List</a></code></pre>
> Spawn titans at a position
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitansAtAsync(type: string, count: int, position: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, rotationY: float = 0)</code></pre>
> Spawn titans at a position asynchronously
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnShifter(type: string) -> <a data-footnote-ref href="#user-content-fn-30">Shifter</a></code></pre>
> Spawn a shifter
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnShifterAt(type: string, position: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, rotationY: float = 0) -> <a data-footnote-ref href="#user-content-fn-30">Shifter</a></code></pre>
> Spawn a shifter at a position
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnProjectile(parameters: <a data-footnote-ref href="#user-content-fn-38">Object</a>)</code></pre>
> Spawn a projectile
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnProjectileWithOwner(parameters: <a data-footnote-ref href="#user-content-fn-38">Object</a>)</code></pre>
> Spawn a projectile with an owner
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnEffect(parameters: <a data-footnote-ref href="#user-content-fn-38">Object</a>)</code></pre>
> Spawn an effect
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayer(player: <a data-footnote-ref href="#user-content-fn-24">Player</a>, force: bool)</code></pre>
> Spawn a player
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayerAll(force: bool)</code></pre>
> Spawn a player for all players
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayerAt(player: <a data-footnote-ref href="#user-content-fn-24">Player</a>, force: bool, position: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, rotationY: float = 0)</code></pre>
> Spawn a player at a position
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayerAtAll(force: bool, position: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, rotationY: float = 0)</code></pre>
> Spawn a player at a position for all players
> 
<pre class="language-typescript"><code class="lang-typescript">function SetPlaylist(playlist: string)</code></pre>
> Set the music playlist
> 
<pre class="language-typescript"><code class="lang-typescript">function SetSong(song: string)</code></pre>
> Set the music song
> 
<pre class="language-typescript"><code class="lang-typescript">function DrawRay(start: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, dir: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, color: <a data-footnote-ref href="#user-content-fn-4">Color</a>, duration: float)</code></pre>
> Draw a ray
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowKillScore(damage: int)</code></pre>
> Show the kill score
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowKillFeed(killer: string, victim: string, score: int, weapon: string)</code></pre>
> Show the kill feed
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowKillFeedAll(killer: string, victim: string, score: int, weapon: string)</code></pre>
> Show the kill feed for all players
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
