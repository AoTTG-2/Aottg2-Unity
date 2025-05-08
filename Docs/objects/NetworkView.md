# NetworkView
Inherits from Object

## Initialization
<mark style="color:red;">This class is abstract and cannot be instantiated.</mark>

> Represents a network view on a map object that has the "networked" flag.             Note1: messages sent from a mapobjects network view are not component scoped, all components will receive the same message.             If you intend for a mapobject to have multiple message sending components, preface the message with the component name to determine scope.                          Note2: Rooms and Players have bandwidth limits, exceeding the limits via CL will result in either the player being kicked or the room being shut down.             When possible, use basic message passing for state sync and then run logic locally instead of repeatedly sending state over the network. Also             avoid cases where message sending increases heavily with the number of players in the room.
> Snippet:
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
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Owner|[Player](../objects/Player.md)|True|The network view's owner.|
## Methods
###### function <mark style="color:yellow;">Transfer</mark>(player: <mark style="color:blue;">[Player](../objects/Player.md)</mark>)
> Owner only. Transfer ownership of this NetworkView to another player.

###### function <mark style="color:yellow;">SendMessage</mark>(target: <mark style="color:blue;">[Player](../objects/Player.md)</mark>, msg: <mark style="color:blue;">string</mark>)
> Send a message to a target player. This will be received in any of the MapObject attached components through the OnNetworkMessage callback.

###### function <mark style="color:yellow;">SendMessageAll</mark>(msg: <mark style="color:blue;">string</mark>)
> Send a message to all players including myself.

###### function <mark style="color:yellow;">SendMessageOthers</mark>(msg: <mark style="color:blue;">string</mark>)
> Send a message to players excluding myself.

###### function <mark style="color:yellow;">SendStream</mark>(obj: <mark style="color:blue;">Object</mark>)
> Send an object to the network sync stream.             This represents sending data from the object owner to all non-owner observers,             and should only be called in the SendNetworkStream callback in the attached component.             It only works with some object types: primitives and Vector3.

###### function <mark style="color:yellow;">ReceiveStream</mark>() → <mark style="color:blue;">Object</mark>
> Receive an object through the network sync stream.             This represents receiving data from the object owner as a non-owner observer,             and should only be called in the OnNetworkStream callback.


---

