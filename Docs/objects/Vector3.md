# Vector3
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|X|float|False|X component of the vector.|
|Y|float|False|Y component of the vector.|
|Z|float|False|Z component of the vector.|
|Normalized|[Vector3](../objects/Vector3.md)|False|Returns this vector with a magnitude of 1 (Read Only).|
|Magnitude|float|False|Returns the length of this vector (Read Only).|
|SqrMagnitude|float|False|Returns the squared length of this vector (Read Only).|
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Zero|[Vector3](../objects/Vector3.md)|False|Shorthand for writing Vector3(0, 0, 0).|
|One|[Vector3](../objects/Vector3.md)|False|Shorthand for writing Vector3(1, 1, 1).|
|Up|[Vector3](../objects/Vector3.md)|False|Shorthand for writing Vector3(0, 1, 0).|
|Down|[Vector3](../objects/Vector3.md)|False|Shorthand for writing Vector3(0, -1, 0).|
|Left|[Vector3](../objects/Vector3.md)|False|Shorthand for writing Vector3(-1, 0, 0).|
|Right|[Vector3](../objects/Vector3.md)|False|Shorthand for writing Vector3(1, 0, 0).|
|Forward|[Vector3](../objects/Vector3.md)|False|Shorthand for writing Vector3(0, 0, 1).|
|Back|[Vector3](../objects/Vector3.md)|False|Shorthand for writing Vector3(0, 0, -1).|
|NegativeInfinity|[Vector3](../objects/Vector3.md)|False|Shorthand for writing Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity).|
|PositiveInfinity|[Vector3](../objects/Vector3.md)|False|Shorthand for writing Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity).|
## Methods
##### void Set(float x, float y, float z)
- **Description:** Set x, y and z components of an existing Vector3.
##### [Vector3](../objects/Vector3.md) Scale(Object scale)
- **Description:** Returns the Vector3 multiplied by scale.
##### [Vector3](../objects/Vector3.md) Multiply([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b)
- **Description:** Returns the multiplication of two Vector3s.
##### [Vector3](../objects/Vector3.md) Divide([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b)
- **Description:** Returns the division of two Vector3s.
##### [Vector3](../objects/Vector3.md) GetRotationDirection([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b)
- **Description:** Gets the relational Vector3 "b" using "a" as a reference. This is equivalent to setting MapObject.Forward to Vector "a", and finding the relative "b" vector.
##### Object \_\_Copy\_\_()
- **Description:** Override to deepcopy object on assignment, used for structs. Ex: copy = original is equivalent to copy = original.\_\_Copy\_\_()
##### Object \_\_Add\_\_(Object self, Object other)
- **Description:** Override to implement addition, used for + operator. Ex: a + b is equivalent to a.\_\_Add\_\_(a, b)
##### Object \_\_Sub\_\_(Object self, Object other)
- **Description:** Override to implement subtraction, used for - operator. Ex: a - b is equivalent to a.\_\_Sub\_\_(a, b)
##### Object \_\_Mul\_\_(Object self, Object other)
- **Description:** Override to implement multiplication, used for * operator. Ex: a * b is equivalent to a.\_\_Mul\_\_(a, b)
##### Object \_\_Div\_\_(Object self, Object other)
- **Description:** Override to implement division, used for / operator. Ex: a / b is equivalent to a.\_\_Div\_\_(a, b)
##### bool \_\_Eq\_\_(Object self, Object other)
- **Description:** Override to implement equality comparison, used for == and != operators. Ex: a == b is equivalent to a.\_\_Eq\_\_(a, b)
##### int \_\_Hash\_\_()
- **Description:** Override to implement hashing, used for GetHashCode - Used for Dictionaries/Sets. Ex: hash = obj.GetHashCode() is equivalent to hash = obj.\_\_Hash\_\_()

---

## Static Methods
##### float Angle([Vector3](../objects/Vector3.md) from, [Vector3](../objects/Vector3.md) to)
- **Description:** Calculates the angle between vectors from and.
##### [Vector3](../objects/Vector3.md) ClampMagnitude([Vector3](../objects/Vector3.md) vector, float maxLength)
- **Description:** Returns a copy of vector with its magnitude clamped to maxLength.
##### [Vector3](../objects/Vector3.md) Cross([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b)
- **Description:** Cross Product of two vectors.
##### float Distance([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b)
- **Description:** Returns the distance between a and b.
##### float Dot([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b)
- **Description:** Dot Product of two vectors.
##### [Vector3](../objects/Vector3.md) Lerp([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b, float t)
- **Description:** Linearly interpolates between two points.
##### [Vector3](../objects/Vector3.md) LerpUnclamped([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b, float t)
- **Description:** Linearly interpolates between two vectors.
##### [Vector3](../objects/Vector3.md) Max([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b)
- **Description:** Returns a vector that is made from the largest components of two vectors.
##### [Vector3](../objects/Vector3.md) Min([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b)
- **Description:** Returns a vector that is made from the smallest components of two vectors.
##### [Vector3](../objects/Vector3.md) MoveTowards([Vector3](../objects/Vector3.md) current, [Vector3](../objects/Vector3.md) target, float maxDistanceDelta)
- **Description:** Calculate a position between the points specified by current and target, moving no farther than the distance specified by maxDistanceDelta.
##### [Vector3](../objects/Vector3.md) Normalize([Vector3](../objects/Vector3.md) value)
- **Description:** Makes this vector have a magnitude of 1.
##### void OrthoNormalize([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b)
- **Description:** 
##### [Vector3](../objects/Vector3.md) Project([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b)
- **Description:** Projects a vector onto another vector.
##### [Vector3](../objects/Vector3.md) ProjectOnPlane([Vector3](../objects/Vector3.md) vector, [Vector3](../objects/Vector3.md) plane)
- **Description:** Projects a vector onto a plane defined by a normal orthogonal to the plane.
##### [Vector3](../objects/Vector3.md) Reflect([Vector3](../objects/Vector3.md) inDirection, [Vector3](../objects/Vector3.md) inNormal)
- **Description:** Reflects a vector off the plane defined by a normal.
##### [Vector3](../objects/Vector3.md) RotateTowards([Vector3](../objects/Vector3.md) current, [Vector3](../objects/Vector3.md) target, float maxRadiansDelta, float maxMagnitudeDelta)
- **Description:** Rotates a vector current towards target.
##### float SignedAngle([Vector3](../objects/Vector3.md) from, [Vector3](../objects/Vector3.md) to, [Vector3](../objects/Vector3.md) axis)
- **Description:** Calculates the signed angle between vectors from and to in relation to axis.
##### [Vector3](../objects/Vector3.md) Slerp([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b, float t)
- **Description:** Spherically interpolates between two vectors.
##### [Vector3](../objects/Vector3.md) SlerpUnclamped([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b, float t)
- **Description:** Spherically interpolates between two vectors.
##### [Vector3](../objects/Vector3.md) SmoothDamp([Vector3](../objects/Vector3.md) current, [Vector3](../objects/Vector3.md) target, [Vector3](../objects/Vector3.md) currentVelocity, float smoothTime, float maxSpeed)
- **Description:** 

---

