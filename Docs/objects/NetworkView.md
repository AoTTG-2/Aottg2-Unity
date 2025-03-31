# NetworkView
Inherits from object
## Initialization
<span style="color:red;">This class is abstract and cannot be instantiated.</span>
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Owner|[Player](../objects/Player.md)|True|The network view's owner.|
## Methods
#### function <span style="color:yellow;">Transfer</span>(player: <span style="color:blue;">[Player](../objects/Player.md)</span>) → <span style="color:blue;">null</span>
> Owner only. Transfer ownership of this NetworkView to another player.

#### function <span style="color:yellow;">SendMessage</span>(target: <span style="color:blue;">[Player](../objects/Player.md)</span>, msg: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Send a message to a target player. This will be received in any of the MapObject attached components through the OnNetworkMessage callback.

#### function <span style="color:yellow;">SendMessageAll</span>(msg: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Send a message to all players including myself.

#### function <span style="color:yellow;">SendMessageOthers</span>(msg: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Send a message to players excluding myself.

#### function <span style="color:yellow;">SendStream</span>(obj: <span style="color:blue;">Object</span>) → <span style="color:blue;">null</span>
> Send an object to the network sync stream.             This represents sending data from the object owner to all non-owner observers,             and should only be called in the SendNetworkStream callback in the attached component.             It only works with some object types: primitives and Vector3.

#### function <span style="color:yellow;">ReceiveStream</span>() → <span style="color:blue;">Object</span>
> Receive an object through the network sync stream.             This represents receiving data from the object owner as a non-owner observer,             and should only be called in the OnNetworkStream callback.


---

