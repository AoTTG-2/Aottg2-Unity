# LineRenderer
Inherits from object
## Initialization
```csharp
# LineRenderer(Object[])
example = LineRenderer(Object[])
```
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|StartWidth|float|False|The width of the line at the start|
|EndWidth|float|False|The width of the line at the end|
|LineColor|[Color](../objects/Color.md)|False|The color of the line|
|PositionCount|int|False|The number of points in the line|
|Enabled|bool|False|Is the line renderer enabled|
|Loop|bool|False|Is the line renderer a loop|
|NumCornerVertices|int|False|The number of corner vertices|
|NumCapVertices|int|False|The number of end cap vertices|
|Alignment|string|False|The alignment of the line renderer|
|TextureMode|string|False|The texture mode of the line renderer|
|UseWorldSpace|bool|False|Is the line renderer in world space|
|ShadowCastingMode|string|False|Does the line renderer cast shadows|
|ReceiveShadows|bool|False|Does the line renderer receive shadows|
|ColorGradient|[List](../objects/List.md)|False|The gradient of the line renderer|
|AlphaGradient|[List](../objects/List.md)|False|The alpha gradient of the line renderer|
|WidthCurve|[List](../objects/List.md)|False|The width curve of the line renderer|
|WidthMultiplier|float|False|The width multiplier of the line renderer|
|ColorGradientMode|string|False|The color gradient mode of the line renderer|
## Methods
###### function <mark style="color:yellow;">GetPosition</mark>(index: <mark style="color:blue;">int</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Get the position of a point in the line renderer

###### function <mark style="color:yellow;">SetPosition</mark>(index: <mark style="color:blue;">int</mark>, position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">null</mark>
> Set the position of a point in the line renderer


---

## Static Methods
###### function <mark style="color:yellow;">CreateLineRenderer</mark>() → <mark style="color:blue;">[LineRenderer](../objects/LineRenderer.md)</mark>
> <mark style="color:red;">This method is obselete</mark>: Create a new instance with LineRenderer() instead.

> Create a new LineRenderer


---

