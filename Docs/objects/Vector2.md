# Vector2
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|X|float|False|X component of the vector.|
|Y|float|False|Y component of the vector.|
|Normalized|[Vector2](../objects/Vector2.md)|False|Returns this vector with a magnitude of 1 (Read Only).|
|Magnitude|float|False|Returns the length of this vector (Read Only).|
|SqrMagnitude|float|False|Returns the squared length of this vector (Read Only).|
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Zero|[Vector2](../objects/Vector2.md)|False|Shorthand for writing Vector2(0, 0).|
|One|[Vector2](../objects/Vector2.md)|False|Shorthand for writing Vector2(1, 1).|
|Up|[Vector2](../objects/Vector2.md)|False|Shorthand for writing Vector2(0, 1).|
|Down|[Vector2](../objects/Vector2.md)|False|Shorthand for writing Vector2(0, -1).|
|Left|[Vector2](../objects/Vector2.md)|False|Shorthand for writing Vector2(-1, 0).|
|Right|[Vector2](../objects/Vector2.md)|False|Shorthand for writing Vector2(1, 0).|
|NegativeInfinity|[Vector2](../objects/Vector2.md)|False|Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).|
|PositiveInfinity|[Vector2](../objects/Vector2.md)|False|Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).|
## Methods
##### void Set(float x, float y)
- **Description:** Set x and y components of an existing Vector2.
##### void Normalize()
- **Description:** Makes this vector have a magnitude of 1.
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
##### float Angle([Vector2](../objects/Vector2.md) from, [Vector2](../objects/Vector2.md) to)
- **Description:** Gets the unsigned angle in degrees between from and to.
##### [Vector2](../objects/Vector2.md) ClampMagnitude([Vector2](../objects/Vector2.md) vector, float maxLength)
- **Description:** Returns a copy of vector with its magnitude clamped to maxLength.
##### float Distance([Vector2](../objects/Vector2.md) a, [Vector2](../objects/Vector2.md) b)
- **Description:** Returns the distance between a and b.
##### float Dot([Vector2](../objects/Vector2.md) a, [Vector2](../objects/Vector2.md) b)
- **Description:** Dot Product of two vectors.
##### [Vector2](../objects/Vector2.md) Lerp([Vector2](../objects/Vector2.md) a, [Vector2](../objects/Vector2.md) b, float t)
- **Description:** Linearly interpolates between vectors a and b by t.
##### [Vector2](../objects/Vector2.md) LerpUnclamped([Vector2](../objects/Vector2.md) a, [Vector2](../objects/Vector2.md) b, float t)
- **Description:** Linearly interpolates between vectors a and b by t.
##### [Vector2](../objects/Vector2.md) Max([Vector2](../objects/Vector2.md) a, [Vector2](../objects/Vector2.md) b)
- **Description:** Returns a vector that is made from the largest components of two vectors.
##### [Vector2](../objects/Vector2.md) Min([Vector2](../objects/Vector2.md) a, [Vector2](../objects/Vector2.md) b)
- **Description:** Returns a vector that is made from the smallest components of two vectors.
##### [Vector2](../objects/Vector2.md) MoveTowards([Vector2](../objects/Vector2.md) current, [Vector2](../objects/Vector2.md) target, float maxDistanceDelta)
- **Description:** Moves a point current towards target.
##### [Vector2](../objects/Vector2.md) Reflect([Vector2](../objects/Vector2.md) inDirection, [Vector2](../objects/Vector2.md) inNormal)
- **Description:** Reflects a vector off the vector defined by a normal.
##### float SignedAngle([Vector2](../objects/Vector2.md) from, [Vector2](../objects/Vector2.md) to)
- **Description:** Gets the signed angle in degrees between from and to.
##### [Vector2](../objects/Vector2.md) SmoothDamp([Vector2](../objects/Vector2.md) current, [Vector2](../objects/Vector2.md) target, [Vector2](../objects/Vector2.md) currentVelocity, float smoothTime, float maxSpeed)
- **Description:** 

---

