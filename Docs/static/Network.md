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
##### void SendMessage([Player](../objects/Player.md) player, [String](../static/String.md) message)
- **Description:** Send a message to a player
##### void SendMessageAll([String](../static/String.md) message)
- **Description:** Send a message to all players
##### void SendMessageOthers([String](../static/String.md) message)
- **Description:** Send a message to all players except the sender
##### double GetTimestampDifference(double timestamp1, double timestamp2)
- **Description:** Get the difference between two photon timestamps
##### void KickPlayer(Object target, [String](../static/String.md) reason = .)
- **Description:** Kick the given player by id or player reference.

---

