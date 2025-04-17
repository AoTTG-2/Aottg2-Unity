# Input
Inherits from object
## Initialization
<mark style="color:red;">This class is abstract and cannot be instantiated.</mark>
## Static Methods
###### function <mark style="color:yellow;">GetKeyName</mark>(key: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">string</mark>
> Gets the key name the player assigned to the key setting

###### function <mark style="color:yellow;">GetKeyHold</mark>(key: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">bool</mark>
> Returns true if the key is being held down

###### function <mark style="color:yellow;">GetKeyDown</mark>(key: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">bool</mark>
> Returns true if the key was pressed down this frame

###### function <mark style="color:yellow;">GetKeyUp</mark>(key: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">bool</mark>
> Returns true if the key was released this frame

###### function <mark style="color:yellow;">GetMouseAim</mark>() → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Returns the position the player is aiming at

###### function <mark style="color:yellow;">GetCursorAimDirection</mark>() → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Returns the ray the player is aiming at

###### function <mark style="color:yellow;">GetMouseSpeed</mark>() → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Returns the speed of the mouse

###### function <mark style="color:yellow;">GetMousePosition</mark>() → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Returns the position of the mouse

###### function <mark style="color:yellow;">GetScreenDimensions</mark>() → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Returns the dimensions of the screen

###### function <mark style="color:yellow;">SetKeyDefaultEnabled</mark>(key: <mark style="color:blue;">string</mark>, enabled: <mark style="color:blue;">bool</mark>) → <mark style="color:blue;">null</mark>
> Sets whether the key is enabled by default

###### function <mark style="color:yellow;">SetKeyHold</mark>(key: <mark style="color:blue;">string</mark>, enabled: <mark style="color:blue;">bool</mark>) → <mark style="color:blue;">null</mark>
> Sets whether the key is being held down


---

