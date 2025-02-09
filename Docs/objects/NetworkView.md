# NetworkView
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Owner|[Player](../objects/Player.md)|False|The network view's owner.|
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
<td>Transfer(player : [Player](../objects/Player.md))</td>
<td>none</td>
<td>Owner only. Transfer ownership of this NetworkView to another player.</td>
</tr>
<tr>
<td>SendMessage(target : [Player](../objects/Player.md),msg : [String](../static/String.md))</td>
<td>none</td>
<td>Send a message to a target player. This will be received in any of the MapObject attached components through the OnNetworkMessage callback.</td>
</tr>
<tr>
<td>SendMessageAll(msg : [String](../static/String.md))</td>
<td>none</td>
<td>Send a message to all players including myself.</td>
</tr>
<tr>
<td>SendMessageOthers(msg : [String](../static/String.md))</td>
<td>none</td>
<td>Send a message to players excluding myself.</td>
</tr>
<tr>
<td>SendStream(obj : Object)</td>
<td>none</td>
<td>Send an object to the network sync stream.             This represents sending data from the object owner to all non-owner observers,             and should only be called in the SendNetworkStream callback in the attached component.             It only works with some object types: primitives and Vector3.</td>
</tr>
<tr>
<td>ReceiveStream()</td>
<td>Object</td>
<td>Receive an object through the network sync stream.             This represents receiving data from the object owner as a non-owner observer,             and should only be called in the OnNetworkStream callback.</td>
</tr>
</tbody>
</table>
