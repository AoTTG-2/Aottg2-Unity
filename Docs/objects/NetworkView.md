# NetworkView
Inherits from object
## Initialization
<mark style="color:red;">This class is abstract and cannot be instantiated.</mark>
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Owner|[Player](../objects/Player.md)|True|The network view's owner.|
## Methods
#### function <mark style="color:yellow;">Transfer</mark>(player: <mark style="color:blue;">[Player](../objects/Player.md)</mark>) → <mark style="color:blue;">null</mark>
> Owner only. Transfer ownership of this NetworkView to another player.

#### function <mark style="color:yellow;">SendMessage</mark>(target: <mark style="color:blue;">[Player](../objects/Player.md)</mark>, msg: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">null</mark>
> Send a message to a target player. This will be received in any of the MapObject attached components through the OnNetworkMessage callback.

#### function <mark style="color:yellow;">SendMessageAll</mark>(msg: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">null</mark>
> Send a message to all players including myself.

#### function <mark style="color:yellow;">SendMessageOthers</mark>(msg: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">null</mark>
> Send a message to players excluding myself.

#### function <mark style="color:yellow;">SendStream</mark>(obj: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">null</mark>
> Send an object to the network sync stream.             This represents sending data from the object owner to all non-owner observers,             and should only be called in the SendNetworkStream callback in the attached component.             It only works with some object types: primitives and Vector3.

#### function <mark style="color:yellow;">ReceiveStream</mark>() → <mark style="color:blue;">Object</mark>
> Receive an object through the network sync stream.             This represents receiving data from the object owner as a non-owner observer,             and should only be called in the OnNetworkStream callback.


---

