# LineRenderer
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|StartWidth|float|False|The width of the line at the start|
|EndWidth|float|False|The width of the line at the end|
|LineColor|[Color](../Static/Color.md)|False|The color of the line|
|PositionCount|int|False|The number of points in the line|
|Enabled|bool|False|Is the line renderer enabled|
|Loop|bool|False|Is the line renderer a loop|
|NumCornerVertices|int|False|The number of corner vertices|
|NumCapVertices|int|False|The number of end cap vertices|
|Alignment|[String](../Static/String.md)|False|The alignment of the line renderer|
|TextureMode|[String](../Static/String.md)|False|The texture mode of the line renderer|
|UseWorldSpace|bool|False|Is the line renderer in world space|
|ShadowCastingMode|[String](../Static/String.md)|False|Does the line renderer cast shadows|
|ReceiveShadows|bool|False|Does the line renderer receive shadows|
|ColorGradient|[List](../Object/List.md)|False|The gradient of the line renderer|
|AlphaGradient|[List](../Object/List.md)|False|The alpha gradient of the line renderer|
|WidthCurve|[List](../Object/List.md)|False|The width curve of the line renderer|
|WidthMultiplier|float|False|The width multiplier of the line renderer|
|ColorGradientMode|[String](../Static/String.md)|False|The color gradient mode of the line renderer|
## Methods
|Function|Returns|Description|
|---|---|---|
|CreateLineRenderer()|[LineRenderer](../Static/LineRenderer.md)|[Obselete] Create a new LineRenderer|
|GetPosition(index : int)|[Vector3](../Static/Vector3.md)|Get the position of a point in the line renderer|
|SetPosition(index : int, position : [Vector3](../Static/Vector3.md))|none|Set the position of a point in the line renderer|
