# Input
Inherits from object
## Initialization
<span style="color:red;">This class is abstract and cannot be instantiated.</span>
## Static Methods
#### function <span style="color:yellow;">GetKeyName</span>(key: <span style="color:blue;">string</span>) → <span style="color:blue;">string</span>
> Gets the key name the player assigned to the key setting

#### function <span style="color:yellow;">GetKeyHold</span>(key: <span style="color:blue;">string</span>) → <span style="color:blue;">bool</span>
> Returns true if the key is being held down

#### function <span style="color:yellow;">GetKeyDown</span>(key: <span style="color:blue;">string</span>) → <span style="color:blue;">bool</span>
> Returns true if the key was pressed down this frame

#### function <span style="color:yellow;">GetKeyUp</span>(key: <span style="color:blue;">string</span>) → <span style="color:blue;">bool</span>
> Returns true if the key was released this frame

#### function <span style="color:yellow;">GetMouseAim</span>() → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Returns the position the player is aiming at

#### function <span style="color:yellow;">GetCursorAimDirection</span>() → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Returns the ray the player is aiming at

#### function <span style="color:yellow;">GetMouseSpeed</span>() → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Returns the speed of the mouse

#### function <span style="color:yellow;">GetMousePosition</span>() → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Returns the position of the mouse

#### function <span style="color:yellow;">GetScreenDimensions</span>() → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Returns the dimensions of the screen

#### function <span style="color:yellow;">SetKeyDefaultEnabled</span>(key: <span style="color:blue;">string</span>, enabled: <span style="color:blue;">bool</span>) → <span style="color:blue;">null</span>
> Sets whether the key is enabled by default

#### function <span style="color:yellow;">SetKeyHold</span>(key: <span style="color:blue;">string</span>, enabled: <span style="color:blue;">bool</span>) → <span style="color:blue;">null</span>
> Sets whether the key is being held down


---

