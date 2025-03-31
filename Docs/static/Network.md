# Network
Inherits from object
## Initialization
<span style="color:red;">This class is abstract and cannot be instantiated.</span>
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|IsMasterClient|bool|True|Is the player the master client|
|Players|[List](../objects/List.md)|True|The list of players in the room|
|MasterClient|[Player](../objects/Player.md)|True|The master client|
|MyPlayer|[Player](../objects/Player.md)|True|The local player|
|NetworkTime|double|True|The network time|
|Ping|int|True|The local player's ping|
## Methods
#### function <span style="color:yellow;">SendMessage</span>(player: <span style="color:blue;">[Player](../objects/Player.md)</span>, message: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Send a message to a player

#### function <span style="color:yellow;">SendMessageAll</span>(message: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Send a message to all players

#### function <span style="color:yellow;">SendMessageOthers</span>(message: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Send a message to all players except the sender

#### function <span style="color:yellow;">GetTimestampDifference</span>(timestamp1: <span style="color:blue;">double</span>, timestamp2: <span style="color:blue;">double</span>) → <span style="color:blue;">double</span>
> Get the difference between two photon timestamps

#### function <span style="color:yellow;">KickPlayer</span>(target: <span style="color:blue;">Object</span>, reason: <span style="color:blue;">string</span> = <span style="color:blue;">.</span>) → <span style="color:blue;">null</span>
> Kick the given player by id or player reference.


---

