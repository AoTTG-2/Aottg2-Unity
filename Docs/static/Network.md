# Network
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|IsMasterClient|bool|False|Is the player the master client|
|Players|[List](../objects/List.md)|False|The list of players in the room|
|MasterClient|[Player](../objects/Player.md)|False|The master client|
|MyPlayer|[Player](../objects/Player.md)|False|The local player|
|NetworkTime|double|False|The network time|
|Ping|int|False|The local player's ping|
## Methods
#### function <mark style="color:yellow;">SendMessage</mark>(player: <mark style="color:blue;">[Player](../objects/Player.md)</mark>, message: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Send a message to a player

#### function <mark style="color:yellow;">SendMessageAll</mark>(message: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Send a message to all players

#### function <mark style="color:yellow;">SendMessageOthers</mark>(message: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Send a message to all players except the sender

#### function <mark style="color:yellow;">GetTimestampDifference</mark>(timestamp1: <mark style="color:blue;">double</mark>, timestamp2: <mark style="color:blue;">double</mark>) -> <mark style="color:blue;">double</mark>
> Get the difference between two photon timestamps

#### function <mark style="color:yellow;">KickPlayer</mark>(target: <mark style="color:blue;">Object</mark>, reason: <mark style="color:blue;">[String](../static/String.md)</mark> = <mark style="color:blue;">.</mark>) -> <mark style="color:blue;">void</mark>
> Kick the given player by id or player reference.


---

