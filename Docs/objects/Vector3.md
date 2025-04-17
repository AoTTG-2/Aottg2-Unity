# Vector3
Inherits from object
## Initialization
```csharp
# Vector3()
example = Vector3()

# Vector3(float)
example = Vector3(0)

# Vector3(float, float)
example = Vector3(0, 0)

# Vector3(float, float, float)
example = Vector3(0, 0, 0)
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
###### function <mark style="color:yellow;">Set</mark>(x: <mark style="color:blue;">float</mark>, y: <mark style="color:blue;">float</mark>, z: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">null</mark>
> Set x, y and z components of an existing Vector3.

###### function <mark style="color:yellow;">GetRotationDirection</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Gets the relational Vector3 "b" using "a" as a reference. This is equivalent to setting MapObject.Forward to Vector "a", and finding the relative "b" vector.

###### function <mark style="color:yellow;">\_\_Copy\_\_</mark>() → <mark style="color:blue;">Object</mark>
> Overrides the assignment operator to create a deep copy of the object.

###### function <mark style="color:yellow;">\_\_Add\_\_</mark>(self: <mark style="color:blue;">Object</mark>, other: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">Object</mark>
> Overrides addition, used for + operator. Ex: a + b is equivalent to a.\_\_Add\_\_(a, b)

###### function <mark style="color:yellow;">\_\_Sub\_\_</mark>(self: <mark style="color:blue;">Object</mark>, other: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">Object</mark>
> Overrides subtraction, used for - operator. Ex: a - b is equivalent to a.\_\_Sub\_\_(a, b)

###### function <mark style="color:yellow;">\_\_Mul\_\_</mark>(self: <mark style="color:blue;">Object</mark>, other: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">Object</mark>
> Overrides multiplication, used for * operator. Ex: a * b is equivalent to a.\_\_Mul\_\_(a, b)

###### function <mark style="color:yellow;">\_\_Div\_\_</mark>(self: <mark style="color:blue;">Object</mark>, other: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">Object</mark>
> Overrides division, used for / operator. Ex: a / b is equivalent to a.\_\_Div\_\_(a, b)

###### function <mark style="color:yellow;">\_\_Eq\_\_</mark>(self: <mark style="color:blue;">Object</mark>, other: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">bool</mark>
> Overrides the equality comparison, used for == and != operators. Ex: a == b is equivalent to a.\_\_Eq\_\_(a, b)

###### function <mark style="color:yellow;">\_\_Hash\_\_</mark>() → <mark style="color:blue;">int</mark>
> Overrides hashing, used for GetHashCode - Used for Dictionaries/Sets. Ex: hash = obj.GetHashCode() is equivalent to hash = obj.\_\_Hash\_\_()

###### function <mark style="color:yellow;">Scale</mark>(scale: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> <mark style="color:red;">This method is obselete</mark>: Use multiply operator instead

> Returns the Vector3 multiplied by scale.

###### function <mark style="color:yellow;">Multiply</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> <mark style="color:red;">This method is obselete</mark>: Use multiply operator instead

> Returns the multiplication of two Vector3s.

###### function <mark style="color:yellow;">Divide</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> <mark style="color:red;">This method is obselete</mark>: Use divide operator instead

> Returns the division of two Vector3s.


---

## Static Methods
###### function <mark style="color:yellow;">Angle</mark>(from: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, to: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">float</mark>
> Calculates the angle between vectors from and.

###### function <mark style="color:yellow;">ClampMagnitude</mark>(vector: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, maxLength: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Returns a copy of vector with its magnitude clamped to maxLength.

###### function <mark style="color:yellow;">Cross</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Cross Product of two vectors.

###### function <mark style="color:yellow;">Distance</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">float</mark>
> Returns the distance between a and b.

###### function <mark style="color:yellow;">Dot</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">float</mark>
> Dot Product of two vectors.

###### function <mark style="color:yellow;">Lerp</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, t: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Linearly interpolates between two points.

###### function <mark style="color:yellow;">LerpUnclamped</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, t: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Linearly interpolates between two vectors.

###### function <mark style="color:yellow;">Max</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Returns a vector that is made from the largest components of two vectors.

###### function <mark style="color:yellow;">Min</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Returns a vector that is made from the smallest components of two vectors.

###### function <mark style="color:yellow;">MoveTowards</mark>(current: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, target: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, maxDistanceDelta: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Calculate a position between the points specified by current and target, moving no farther than the distance specified by maxDistanceDelta.

###### function <mark style="color:yellow;">Normalize</mark>(value: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Makes this vector have a magnitude of 1.

###### function <mark style="color:yellow;">OrthoNormalize</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">null</mark>
> <mark style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</mark>

###### function <mark style="color:yellow;">Project</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Projects a vector onto another vector.

###### function <mark style="color:yellow;">ProjectOnPlane</mark>(vector: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, plane: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Projects a vector onto a plane defined by a normal orthogonal to the plane.

###### function <mark style="color:yellow;">Reflect</mark>(inDirection: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, inNormal: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Reflects a vector off the plane defined by a normal.

###### function <mark style="color:yellow;">RotateTowards</mark>(current: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, target: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, maxRadiansDelta: <mark style="color:blue;">float</mark>, maxMagnitudeDelta: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Rotates a vector current towards target.

###### function <mark style="color:yellow;">SignedAngle</mark>(from: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, to: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, axis: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">float</mark>
> Calculates the signed angle between vectors from and to in relation to axis.

###### function <mark style="color:yellow;">Slerp</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, t: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Spherically interpolates between two vectors.

###### function <mark style="color:yellow;">SlerpUnclamped</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, t: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Spherically interpolates between two vectors.

###### function <mark style="color:yellow;">SmoothDamp</mark>(current: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, target: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, currentVelocity: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, smoothTime: <mark style="color:blue;">float</mark>, maxSpeed: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> <mark style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</mark>


---

