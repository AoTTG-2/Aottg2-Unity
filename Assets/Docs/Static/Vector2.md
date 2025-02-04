# Vector2
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|X|float|False|X component of the vector.|
|Y|float|False|Y component of the vector.|
|Normalized|[Vector2](../Static/Vector2.md)|False|Returns this vector with a magnitude of 1 (Read Only).|
|Magnitude|float|False|Returns the length of this vector (Read Only).|
|SqrMagnitude|float|False|Returns the squared length of this vector (Read Only).|
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Zero|[Vector2](../Static/Vector2.md)|False|Shorthand for writing Vector2(0, 0).|
|One|[Vector2](../Static/Vector2.md)|False|Shorthand for writing Vector2(1, 1).|
|Up|[Vector2](../Static/Vector2.md)|False|Shorthand for writing Vector2(0, 1).|
|Down|[Vector2](../Static/Vector2.md)|False|Shorthand for writing Vector2(0, -1).|
|Left|[Vector2](../Static/Vector2.md)|False|Shorthand for writing Vector2(-1, 0).|
|Right|[Vector2](../Static/Vector2.md)|False|Shorthand for writing Vector2(1, 0).|
|NegativeInfinity|[Vector2](../Static/Vector2.md)|False|Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).|
|PositiveInfinity|[Vector2](../Static/Vector2.md)|False|Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).|
## Methods
|Function|Returns|Description|
|---|---|---|
|Angle(from : [Vector2](../Static/Vector2.md), to : [Vector2](../Static/Vector2.md))|float|Gets the unsigned angle in degrees between from and to.|
|ClampMagnitude(vector : [Vector2](../Static/Vector2.md), maxLength : float)|[Vector2](../Static/Vector2.md)|Returns a copy of vector with its magnitude clamped to maxLength.|
|Distance(a : [Vector2](../Static/Vector2.md), b : [Vector2](../Static/Vector2.md))|float|Returns the distance between a and b.|
|Dot(a : [Vector2](../Static/Vector2.md), b : [Vector2](../Static/Vector2.md))|float|Dot Product of two vectors.|
|Lerp(a : [Vector2](../Static/Vector2.md), b : [Vector2](../Static/Vector2.md), t : float)|[Vector2](../Static/Vector2.md)|Linearly interpolates between vectors a and b by t.|
|LerpUnclamped(a : [Vector2](../Static/Vector2.md), b : [Vector2](../Static/Vector2.md), t : float)|[Vector2](../Static/Vector2.md)|Linearly interpolates between vectors a and b by t.|
|Max(a : [Vector2](../Static/Vector2.md), b : [Vector2](../Static/Vector2.md))|[Vector2](../Static/Vector2.md)|Returns a vector that is made from the largest components of two vectors.|
|Min(a : [Vector2](../Static/Vector2.md), b : [Vector2](../Static/Vector2.md))|[Vector2](../Static/Vector2.md)|Returns a vector that is made from the smallest components of two vectors.|
|MoveTowards(current : [Vector2](../Static/Vector2.md), target : [Vector2](../Static/Vector2.md), maxDistanceDelta : float)|[Vector2](../Static/Vector2.md)|Moves a point current towards target.|
|Reflect(inDirection : [Vector2](../Static/Vector2.md), inNormal : [Vector2](../Static/Vector2.md))|[Vector2](../Static/Vector2.md)|Reflects a vector off the vector defined by a normal.|
|SignedAngle(from : [Vector2](../Static/Vector2.md), to : [Vector2](../Static/Vector2.md))|float|Gets the signed angle in degrees between from and to.|
|SmoothDamp(current : [Vector2](../Static/Vector2.md), target : [Vector2](../Static/Vector2.md), currentVelocity : [Vector2](../Static/Vector2.md), smoothTime : float, maxSpeed : float)|[Vector2](../Static/Vector2.md)||
|Set(x : float, y : float)|none|Set x and y components of an existing Vector2.|
|Normalize()|none|Makes this vector have a magnitude of 1.|
|\_\_Copy\_\_()|Object|Override to deepcopy object on assignment, used for structs. Ex: copy = original is equivalent to copy = original.\_\_Copy\_\_()|
|\_\_Add\_\_(self : Object, other : Object)|Object|Override to implement addition, used for + operator. Ex: a + b is equivalent to a.\_\_Add\_\_(a, b)|
|\_\_Sub\_\_(self : Object, other : Object)|Object|Override to implement subtraction, used for - operator. Ex: a - b is equivalent to a.\_\_Sub\_\_(a, b)|
|\_\_Mul\_\_(self : Object, other : Object)|Object|Override to implement multiplication, used for * operator. Ex: a * b is equivalent to a.\_\_Mul\_\_(a, b)|
|\_\_Div\_\_(self : Object, other : Object)|Object|Override to implement division, used for / operator. Ex: a / b is equivalent to a.\_\_Div\_\_(a, b)|
|\_\_Eq\_\_(self : Object, other : Object)|bool|Override to implement equality comparison, used for == and != operators. Ex: a == b is equivalent to a.\_\_Eq\_\_(a, b)|
|\_\_Hash\_\_()|int|Override to implement hashing, used for GetHashCode - Used for Dictionaries/Sets. Ex: hash = obj.GetHashCode() is equivalent to hash = obj.\_\_Hash\_\_()|
