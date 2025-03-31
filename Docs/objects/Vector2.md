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
#### function <span style="color:yellow;">Set</span>(x: <span style="color:blue;">float</span>, y: <span style="color:blue;">float</span>) → <span style="color:blue;">null</span>
> Set x and y components of an existing Vector2.

#### function <span style="color:yellow;">Normalize</span>() → <span style="color:blue;">null</span>
> Makes this vector have a magnitude of 1.

#### function <span style="color:yellow;">\_\_Copy\_\_</span>() → <span style="color:blue;">Object</span>
> <span style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</span>

#### function <span style="color:yellow;">\_\_Add\_\_</span>(self: <span style="color:blue;">Object</span>, other: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> <span style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</span>

#### function <span style="color:yellow;">\_\_Sub\_\_</span>(self: <span style="color:blue;">Object</span>, other: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> <span style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</span>

#### function <span style="color:yellow;">\_\_Mul\_\_</span>(self: <span style="color:blue;">Object</span>, other: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> <span style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</span>

#### function <span style="color:yellow;">\_\_Div\_\_</span>(self: <span style="color:blue;">Object</span>, other: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> <span style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</span>

#### function <span style="color:yellow;">\_\_Eq\_\_</span>(self: <span style="color:blue;">Object</span>, other: <span style="color:blue;">Object</span>) → <span style="color:blue;">bool</span>
> <span style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</span>

#### function <span style="color:yellow;">\_\_Hash\_\_</span>() → <span style="color:blue;">int</span>
> <span style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</span>


---

## Static Methods
#### function <span style="color:yellow;">Angle</span>(from: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, to: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>) → <span style="color:blue;">float</span>
> Gets the unsigned angle in degrees between from and to.

#### function <span style="color:yellow;">ClampMagnitude</span>(vector: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, maxLength: <span style="color:blue;">float</span>) → <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>
> Returns a copy of vector with its magnitude clamped to maxLength.

#### function <span style="color:yellow;">Distance</span>(a: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, b: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>) → <span style="color:blue;">float</span>
> Returns the distance between a and b.

#### function <span style="color:yellow;">Dot</span>(a: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, b: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>) → <span style="color:blue;">float</span>
> Dot Product of two vectors.

#### function <span style="color:yellow;">Lerp</span>(a: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, b: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, t: <span style="color:blue;">float</span>) → <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>
> Linearly interpolates between vectors a and b by t.

#### function <span style="color:yellow;">LerpUnclamped</span>(a: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, b: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, t: <span style="color:blue;">float</span>) → <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>
> Linearly interpolates between vectors a and b by t.

#### function <span style="color:yellow;">Max</span>(a: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, b: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>) → <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>
> Returns a vector that is made from the largest components of two vectors.

#### function <span style="color:yellow;">Min</span>(a: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, b: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>) → <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>
> Returns a vector that is made from the smallest components of two vectors.

#### function <span style="color:yellow;">MoveTowards</span>(current: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, target: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, maxDistanceDelta: <span style="color:blue;">float</span>) → <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>
> Moves a point current towards target.

#### function <span style="color:yellow;">Reflect</span>(inDirection: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, inNormal: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>) → <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>
> Reflects a vector off the vector defined by a normal.

#### function <span style="color:yellow;">SignedAngle</span>(from: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, to: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>) → <span style="color:blue;">float</span>
> Gets the signed angle in degrees between from and to.

#### function <span style="color:yellow;">SmoothDamp</span>(current: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, target: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, currentVelocity: <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>, smoothTime: <span style="color:blue;">float</span>, maxSpeed: <span style="color:blue;">float</span>) → <span style="color:blue;">[Vector2](../objects/Vector2.md)</span>
> <span style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</span>


---

