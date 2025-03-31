# Vector2
Inherits from object
## Initialization
```csharp
# Vector2(Object[])
example = Vector2(Object[])
```
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|X|float|False|X component of the vector.|
|Y|float|False|Y component of the vector.|
|Normalized|[Vector2](../objects/Vector2.md)|True|Returns this vector with a magnitude of 1 (Read Only).|
|Magnitude|float|True|Returns the length of this vector (Read Only).|
|SqrMagnitude|float|True|Returns the squared length of this vector (Read Only).|
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Zero|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(0, 0).|
|One|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(1, 1).|
|Up|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(0, 1).|
|Down|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(0, -1).|
|Left|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(-1, 0).|
|Right|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(1, 0).|
|NegativeInfinity|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).|
|PositiveInfinity|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).|
## Methods
#### function <mark style="color:yellow;">Set</mark>(x: <mark style="color:blue;">float</mark>, y: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">null</mark>
> Set x and y components of an existing Vector2.

#### function <mark style="color:yellow;">Normalize</mark>() → <mark style="color:blue;">null</mark>
> Makes this vector have a magnitude of 1.

#### function <mark style="color:yellow;">\_\_Copy\_\_</mark>() → <mark style="color:blue;">Object</mark>
> <mark style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</mark>

#### function <mark style="color:yellow;">\_\_Add\_\_</mark>(self: <mark style="color:blue;">Object</mark>, other: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">Object</mark>
> <mark style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</mark>

#### function <mark style="color:yellow;">\_\_Sub\_\_</mark>(self: <mark style="color:blue;">Object</mark>, other: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">Object</mark>
> <mark style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</mark>

#### function <mark style="color:yellow;">\_\_Mul\_\_</mark>(self: <mark style="color:blue;">Object</mark>, other: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">Object</mark>
> <mark style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</mark>

#### function <mark style="color:yellow;">\_\_Div\_\_</mark>(self: <mark style="color:blue;">Object</mark>, other: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">Object</mark>
> <mark style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</mark>

#### function <mark style="color:yellow;">\_\_Eq\_\_</mark>(self: <mark style="color:blue;">Object</mark>, other: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">bool</mark>
> <mark style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</mark>

#### function <mark style="color:yellow;">\_\_Hash\_\_</mark>() → <mark style="color:blue;">int</mark>
> <mark style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</mark>


---

## Static Methods
#### function <mark style="color:yellow;">Angle</mark>(from: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, to: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>) → <mark style="color:blue;">float</mark>
> Gets the unsigned angle in degrees between from and to.

#### function <mark style="color:yellow;">ClampMagnitude</mark>(vector: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, maxLength: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>
> Returns a copy of vector with its magnitude clamped to maxLength.

#### function <mark style="color:yellow;">Distance</mark>(a: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, b: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>) → <mark style="color:blue;">float</mark>
> Returns the distance between a and b.

#### function <mark style="color:yellow;">Dot</mark>(a: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, b: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>) → <mark style="color:blue;">float</mark>
> Dot Product of two vectors.

#### function <mark style="color:yellow;">Lerp</mark>(a: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, b: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, t: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>
> Linearly interpolates between vectors a and b by t.

#### function <mark style="color:yellow;">LerpUnclamped</mark>(a: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, b: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, t: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>
> Linearly interpolates between vectors a and b by t.

#### function <mark style="color:yellow;">Max</mark>(a: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, b: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>) → <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>
> Returns a vector that is made from the largest components of two vectors.

#### function <mark style="color:yellow;">Min</mark>(a: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, b: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>) → <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>
> Returns a vector that is made from the smallest components of two vectors.

#### function <mark style="color:yellow;">MoveTowards</mark>(current: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, target: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, maxDistanceDelta: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>
> Moves a point current towards target.

#### function <mark style="color:yellow;">Reflect</mark>(inDirection: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, inNormal: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>) → <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>
> Reflects a vector off the vector defined by a normal.

#### function <mark style="color:yellow;">SignedAngle</mark>(from: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, to: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>) → <mark style="color:blue;">float</mark>
> Gets the signed angle in degrees between from and to.

#### function <mark style="color:yellow;">SmoothDamp</mark>(current: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, target: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, currentVelocity: <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>, smoothTime: <mark style="color:blue;">float</mark>, maxSpeed: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Vector2](../objects/Vector2.md)</mark>
> <mark style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</mark>


---

