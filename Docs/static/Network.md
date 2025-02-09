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
<table>
<colgroup><col style="width: 30%"/>
<col style="width: 20%"/>
<col style="width: 50%"/>
</colgroup>
<thead>
<tr>
<th>Function</th>
<th>Returns</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr>
<td>SendMessage(player : [Player](../objects/Player.md),message : [String](../static/String.md))</td>
<td>none</td>
<td>Send a message to a player</td>
</tr>
<tr>
<td>SendMessageAll(message : [String](../static/String.md))</td>
<td>none</td>
<td>Send a message to all players</td>
</tr>
<tr>
<td>SendMessageOthers(message : [String](../static/String.md))</td>
<td>none</td>
<td>Send a message to all players except the sender</td>
</tr>
<tr>
<td>GetTimestampDifference(timestamp1 : double,timestamp2 : double)</td>
<td>double</td>
<td>Get the difference between two photon timestamps</td>
</tr>
<tr>
<td>KickPlayer(target : Object,reason : [String](../static/String.md) = .)</td>
<td>none</td>
<td>Kick the given player by id or player reference.</td>
</tr>
</tbody>
</table>
