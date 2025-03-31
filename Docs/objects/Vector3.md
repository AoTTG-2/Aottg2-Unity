# Vector3
Inherits from object
## Initialization
```csharp
# Vector3()
example = Vector3()

# Vector3(Single)
example = Vector3(Single)

# Vector3(Single, Single)
example = Vector3(Single, Single)

# Vector3(Single, Single, Single)
example = Vector3(Single, Single, Single)
```
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|X|float|False|X component of the vector.|
|Y|float|False|Y component of the vector.|
|Z|float|False|Z component of the vector.|
|Normalized|[Vector3](../objects/Vector3.md)|True|Returns this vector with a magnitude of 1 (Read Only).|
|Magnitude|float|True|Returns the length of this vector (Read Only).|
|SqrMagnitude|float|True|Returns the squared length of this vector (Read Only).|
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Zero|[Vector3](../objects/Vector3.md)|True|Shorthand for writing Vector3(0, 0, 0).|
|One|[Vector3](../objects/Vector3.md)|True|Shorthand for writing Vector3(1, 1, 1).|
|Up|[Vector3](../objects/Vector3.md)|True|Shorthand for writing Vector3(0, 1, 0).|
|Down|[Vector3](../objects/Vector3.md)|True|Shorthand for writing Vector3(0, -1, 0).|
|Left|[Vector3](../objects/Vector3.md)|True|Shorthand for writing Vector3(-1, 0, 0).|
|Right|[Vector3](../objects/Vector3.md)|True|Shorthand for writing Vector3(1, 0, 0).|
|Forward|[Vector3](../objects/Vector3.md)|True|Shorthand for writing Vector3(0, 0, 1).|
|Back|[Vector3](../objects/Vector3.md)|True|Shorthand for writing Vector3(0, 0, -1).|
|NegativeInfinity|[Vector3](../objects/Vector3.md)|True|Shorthand for writing Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity).|
|PositiveInfinity|[Vector3](../objects/Vector3.md)|True|Shorthand for writing Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity).|
## Methods
#### function <span style="color:yellow;">Set</span>(x: <span style="color:blue;">float</span>, y: <span style="color:blue;">float</span>, z: <span style="color:blue;">float</span>) → <span style="color:blue;">null</span>
> Set x, y and z components of an existing Vector3.

#### function <span style="color:yellow;">Scale</span>(scale: <span style="color:blue;">Object</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> <span style="color:red;">This method is obselete</span>: Use multiply operator instead

> Returns the Vector3 multiplied by scale.

#### function <span style="color:yellow;">Multiply</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> <span style="color:red;">This method is obselete</span>: Use multiply operator instead

> Returns the multiplication of two Vector3s.

#### function <span style="color:yellow;">Divide</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> <span style="color:red;">This method is obselete</span>: Use divide operator instead

> Returns the division of two Vector3s.

#### function <span style="color:yellow;">GetRotationDirection</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Gets the relational Vector3 "b" using "a" as a reference. This is equivalent to setting MapObject.Forward to Vector "a", and finding the relative "b" vector.

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
#### function <span style="color:yellow;">Angle</span>(from: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, to: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">float</span>
> Calculates the angle between vectors from and.

#### function <span style="color:yellow;">ClampMagnitude</span>(vector: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, maxLength: <span style="color:blue;">float</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Returns a copy of vector with its magnitude clamped to maxLength.

#### function <span style="color:yellow;">Cross</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Cross Product of two vectors.

#### function <span style="color:yellow;">Distance</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">float</span>
> Returns the distance between a and b.

#### function <span style="color:yellow;">Dot</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">float</span>
> Dot Product of two vectors.

#### function <span style="color:yellow;">Lerp</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, t: <span style="color:blue;">float</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Linearly interpolates between two points.

#### function <span style="color:yellow;">LerpUnclamped</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, t: <span style="color:blue;">float</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Linearly interpolates between two vectors.

#### function <span style="color:yellow;">Max</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Returns a vector that is made from the largest components of two vectors.

#### function <span style="color:yellow;">Min</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Returns a vector that is made from the smallest components of two vectors.

#### function <span style="color:yellow;">MoveTowards</span>(current: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, target: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, maxDistanceDelta: <span style="color:blue;">float</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Calculate a position between the points specified by current and target, moving no farther than the distance specified by maxDistanceDelta.

#### function <span style="color:yellow;">Normalize</span>(value: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Makes this vector have a magnitude of 1.

#### function <span style="color:yellow;">OrthoNormalize</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">null</span>
> <span style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</span>

#### function <span style="color:yellow;">Project</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Projects a vector onto another vector.

#### function <span style="color:yellow;">ProjectOnPlane</span>(vector: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, plane: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Projects a vector onto a plane defined by a normal orthogonal to the plane.

#### function <span style="color:yellow;">Reflect</span>(inDirection: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, inNormal: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Reflects a vector off the plane defined by a normal.

#### function <span style="color:yellow;">RotateTowards</span>(current: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, target: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, maxRadiansDelta: <span style="color:blue;">float</span>, maxMagnitudeDelta: <span style="color:blue;">float</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Rotates a vector current towards target.

#### function <span style="color:yellow;">SignedAngle</span>(from: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, to: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, axis: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">float</span>
> Calculates the signed angle between vectors from and to in relation to axis.

#### function <span style="color:yellow;">Slerp</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, t: <span style="color:blue;">float</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Spherically interpolates between two vectors.

#### function <span style="color:yellow;">SlerpUnclamped</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, t: <span style="color:blue;">float</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Spherically interpolates between two vectors.

#### function <span style="color:yellow;">SmoothDamp</span>(current: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, target: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, currentVelocity: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, smoothTime: <span style="color:blue;">float</span>, maxSpeed: <span style="color:blue;">float</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> <span style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</span>


---

