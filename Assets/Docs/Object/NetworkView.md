# NetworkView
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Owner|[Player](../Object/Player.md)|False|The network view's owner.|
## Methods
|Function|Returns|Description|
|---|---|---|
|Transfer(player : [Player](../Object/Player.md))|none|Owner only. Transfer ownership of this NetworkView to another player.|
|SendMessage(target : [Player](../Object/Player.md), msg : [String](../Static/String.md))|none|Send a message to a target player. This will be received in any of the MapObject attached components through the OnNetworkMessage callback.|
|SendMessageAll(msg : [String](../Static/String.md))|none|Send a message to all players including myself.|
|SendMessageOthers(msg : [String](../Static/String.md))|none|Send a message to players excluding myself.|
|SendStream(obj : Object)|none|Send an object to the network sync stream.             This represents sending data from the object owner to all non-owner observers,             and should only be called in the SendNetworkStream callback in the attached component.             It only works with some object types: primitives and Vector3.|
|ReceiveStream()|Object|Receive an object through the network sync stream.             This represents receiving data from the object owner as a non-owner observer,             and should only be called in the OnNetworkStream callback.|
