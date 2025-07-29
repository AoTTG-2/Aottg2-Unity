# NetworkView
Inherits from [Object](../objects/Object.md)

Represents a network view on a map object that has the "networked" flag.
Note1: messages sent from a mapobjects network view are not component scoped, all components will receive the same message.
If you intend for a mapobject to have multiple message sending components, preface the message with the component name to determine scope.

Note2: Rooms and Players have bandwidth limits, exceeding the limits via CL will result in either the player being kicked or the room being shut down.
When possible, use basic message passing for state sync and then run logic locally instead of repeatedly sending state over the network. Also
avoid cases where message sending increases heavily with the number of players in the room.

### Example
```csharp
# The following is for a component scoped object, in general this is bad practice if the component is widely used.
# OnPlayerJoin, every object with this component will send a message to the player that joined, if you use 100 objects with this, 100 messages will be sent.
# Preferred practice for this sort of case is to have a either Main handle the single message pass or have a single ManagerComponent that handles the message pass
# and defers the value to all registered components.
KillCount = 0;

function OnNetworkMessage(player, message, sentServerTime) {
if (player.ID == Network.MasterClient.ID) {
self.KillCount == Convert.ToInt(message);
}
}

function OnCharacterDie(victim, killer, killerName) {
self.KillCount += 1;
}

function OnPlayerJoined(player) {
if (Network.IsMasterClient) {
self.NetworkView.SendMessage(player, Convert.ToString(self.KillCount));
}
}

# Good Practice would be to have a single component that handles the message pass and defers the value to all registered components.
TODO: Bother someone for good practice example - maybe move this into Networking Summary Page.
```
### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Owner|[Player](../objects/Player.md)|True|The network view's owner.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Transfer(player: <a data-footnote-ref href="#user-content-fn-23">Player</a>) -> null</code></pre>
> Owner only. Transfer ownership of this NetworkView to another player.
> 
<pre class="language-typescript"><code class="lang-typescript">function SendMessage(target: <a data-footnote-ref href="#user-content-fn-23">Player</a>, msg: string) -> null</code></pre>
> Send a message to a target player. This will be received in any of the MapObject attached components through the OnNetworkMessage callback.
> 
<pre class="language-typescript"><code class="lang-typescript">function SendMessageAll(msg: string) -> null</code></pre>
> Send a message to all players including myself.
> 
<pre class="language-typescript"><code class="lang-typescript">function SendMessageOthers(msg: string) -> null</code></pre>
> Send a message to players excluding myself.
> 
<pre class="language-typescript"><code class="lang-typescript">function SendStream(obj: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> null</code></pre>
> Send an object to the network sync stream.
This represents sending data from the object owner to all non-owner observers,
and should only be called in the SendNetworkStream callback in the attached component.
It only works with some object types: primitives and Vector3.
> 
<pre class="language-typescript"><code class="lang-typescript">function ReceiveStream() -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Receive an object through the network sync stream.
This represents receiving data from the object owner as a non-owner observer,
and should only be called in the OnNetworkStream callback.
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
