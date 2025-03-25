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
#### function <mark style="color:yellow;">ToHexString</mark>() -> <mark style="color:blue;">[String](../static/String.md)</mark>
> Converts the color to a hex string


---

## Static Methods
#### function <mark style="color:yellow;">Lerp</mark>(a: <mark style="color:blue;">[Color](../objects/Color.md)</mark>, b: <mark style="color:blue;">[Color](../objects/Color.md)</mark>, t: <mark style="color:blue;">float</mark>) -> <mark style="color:blue;">[Color](../objects/Color.md)</mark>
> Linearly interpolates between colors a and b by t

#### function <mark style="color:yellow;">Gradient</mark>(a: <mark style="color:blue;">[Color](../objects/Color.md)</mark>, b: <mark style="color:blue;">[Color](../objects/Color.md)</mark>, t: <mark style="color:blue;">float</mark>) -> <mark style="color:blue;">[Color](../objects/Color.md)</mark>
> Creates a gradient color from two colors


---

