# Color
Inherits from object
## Initialization
> Constructor for the Color class
> Constructors:
```csharp

color = Color() # Creates a white color
color = Color(150) # Creates a gray color
color = Color(255, 0, 0) # Creates a red color
color = Color(255, 0, 0, 100) # Creates a red color with transparency
```
> Example:
```csharp

Game.Print(color.ToHexString()) // Prints the color in hex format
```
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|R|int|False|Red component of the color|
|G|int|False|Green component of the color|
|B|int|False|Blue component of the color|
|A|int|False|Alpha component of the color|
## Methods
#### function <span style="color:yellow;">ToHexString</span>() → <span style="color:blue;">string</span>
> Converts the color to a hex string


---

## Static Methods
#### function <span style="color:yellow;">Lerp</span>(a: <span style="color:blue;">[Color](../objects/Color.md)</span>, b: <span style="color:blue;">[Color](../objects/Color.md)</span>, t: <span style="color:blue;">float</span>) → <span style="color:blue;">[Color](../objects/Color.md)</span>
> Linearly interpolates between colors a and b by t

#### function <span style="color:yellow;">Gradient</span>(a: <span style="color:blue;">[Color](../objects/Color.md)</span>, b: <span style="color:blue;">[Color](../objects/Color.md)</span>, t: <span style="color:blue;">float</span>) → <span style="color:blue;">[Color](../objects/Color.md)</span>
> Creates a gradient color from two colors


---

