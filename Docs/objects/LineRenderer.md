# LineRenderer
Inherits from object
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
|Alignment|[String](../static/String.md)|False|The alignment of the line renderer|
|TextureMode|[String](../static/String.md)|False|The texture mode of the line renderer|
|UseWorldSpace|bool|False|Is the line renderer in world space|
|ShadowCastingMode|[String](../static/String.md)|False|Does the line renderer cast shadows|
|ReceiveShadows|bool|False|Does the line renderer receive shadows|
|ColorGradient|[List](../objects/List.md)|False|The gradient of the line renderer|
|AlphaGradient|[List](../objects/List.md)|False|The alpha gradient of the line renderer|
|WidthCurve|[List](../objects/List.md)|False|The width curve of the line renderer|
|WidthMultiplier|float|False|The width multiplier of the line renderer|
|ColorGradientMode|[String](../static/String.md)|False|The color gradient mode of the line renderer|
## Methods
##### [Vector3](../objects/Vector3.md) GetPosition(int index)
- **Description:** Get the position of a point in the line renderer
##### void SetPosition(int index, [Vector3](../objects/Vector3.md) position)
- **Description:** Set the position of a point in the line renderer

---

## Static Methods
##### [LineRenderer](../objects/LineRenderer.md) CreateLineRenderer()
- **Description:** [Obselete] Create a new LineRenderer

---

