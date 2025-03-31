# Network
Inherits from object
## Initialization
<mark style="color:red;">This class is abstract and cannot be instantiated.</mark>## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|IsMasterClient|bool|True|Is the player the master client|
|Players|[List](../objects/List.md)|True|The list of players in the room|
|MasterClient|[Player](../objects/Player.md)|True|The master client|
|MyPlayer|[Player](../objects/Player.md)|True|The local player|
|NetworkTime|double|True|The network time|
|Ping|int|True|The local player's ping|
## Methods
#### function <mark style="color:yellow;">SendMessage</mark>(player: <mark style="color:blue;">[Player](../objects/Player.md)</mark>, message: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">null</mark>
> Send a message to a player

#### function <mark style="color:yellow;">SendMessageAll</mark>(message: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">null</mark>
> Send a message to all players

#### function <mark style="color:yellow;">SendMessageOthers</mark>(message: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">null</mark>
> Send a message to all players except the sender

#### function <mark style="color:yellow;">GetTimestampDifference</mark>(timestamp1: <mark style="color:blue;">double</mark>, timestamp2: <mark style="color:blue;">double</mark>) → <mark style="color:blue;">double</mark>
> Get the difference between two photon timestamps

#### function <mark style="color:yellow;">KickPlayer</mark>(target: <mark style="color:blue;">Object</mark>, reason: <mark style="color:blue;">string</mark> = <mark style="color:blue;">.</mark>) → <mark style="color:blue;">null</mark>
> Kick the given player by id or player reference.


---

