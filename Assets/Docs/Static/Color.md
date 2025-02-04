# Color
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|R|int|False|Red component of the color|
|G|int|False|Green component of the color|
|B|int|False|Blue component of the color|
|A|int|False|Alpha component of the color|
## Methods
|Function|Returns|Description|
|---|---|---|
|ToHexString()|[String](../Static/String.md)|Converts the color to a hex string|
|Lerp(a : [Color](../Static/Color.md), b : [Color](../Static/Color.md), t : float)|[Color](../Static/Color.md)|Linearly interpolates between colors a and b by t|
|Gradient(a : [Color](../Static/Color.md), b : [Color](../Static/Color.md), t : float)|[Color](../Static/Color.md)|Creates a gradient color from two colors|
