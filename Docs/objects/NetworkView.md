# NetworkView
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Owner|[Player](../objects/Player.md)|False|The network view's owner.|
## Methods
##### void Transfer([Player](../objects/Player.md) player)
- **Description:** Owner only. Transfer ownership of this NetworkView to another player.
##### void SendMessage([Player](../objects/Player.md) target, [String](../static/String.md) msg)
- **Description:** Send a message to a target player. This will be received in any of the MapObject attached components through the OnNetworkMessage callback.
##### void SendMessageAll([String](../static/String.md) msg)
- **Description:** Send a message to all players including myself.
##### void SendMessageOthers([String](../static/String.md) msg)
- **Description:** Send a message to players excluding myself.
##### void SendStream(Object obj)
- **Description:** Send an object to the network sync stream.             This represents sending data from the object owner to all non-owner observers,             and should only be called in the SendNetworkStream callback in the attached component.             It only works with some object types: primitives and Vector3.
##### Object ReceiveStream()
- **Description:** Receive an object through the network sync stream.             This represents receiving data from the object owner as a non-owner observer,             and should only be called in the OnNetworkStream callback.

---

