# RoomData
Inherits from Object

<mark style="color:red;">This class is static and cannot be instantiated.</mark>

> Store and retrieve room variables. Room data is cleared upon joining or creating a new lobby and does not reset between game rounds. Supports float, string, bool, and int types.             Note that RoomData is local only and does not sync.You must use network messages to sync room variables.
## Static Methods
###### function <mark style="color:yellow;">SetProperty</mark>(property: <mark style="color:blue;">string</mark>, value: <mark style="color:blue;">Object</mark>)
> Sets the property with given name to the object value. Valid value types are float, string, bool, and int.

###### function <mark style="color:yellow;">GetProperty</mark>(property: <mark style="color:blue;">string</mark>, defaultValue: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">Object</mark>
> Gets the property with given name. If property does not exist, returns defaultValue.

###### function <mark style="color:yellow;">Clear</mark>()
> Clears all room data.


---

