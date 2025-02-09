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
<table>
<colgroup><col style="width: 30%"/>
<col style="width: 20%"/>
<col style="width: 50%"/>
</colgroup>
<thead>
<tr>
<th>Function</th>
<th>Returns</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr>
<td>Set(x : float,y : float)</td>
<td>none</td>
<td>Set x and y components of an existing Vector2.</td>
</tr>
<tr>
<td>Normalize()</td>
<td>none</td>
<td>Makes this vector have a magnitude of 1.</td>
</tr>
<tr>
<td>\_\_Copy\_\_()</td>
<td>Object</td>
<td>Override to deepcopy object on assignment, used for structs. Ex: copy = original is equivalent to copy = original.\_\_Copy\_\_()</td>
</tr>
<tr>
<td>\_\_Add\_\_(self : Object,other : Object)</td>
<td>Object</td>
<td>Override to implement addition, used for + operator. Ex: a + b is equivalent to a.\_\_Add\_\_(a, b)</td>
</tr>
<tr>
<td>\_\_Sub\_\_(self : Object,other : Object)</td>
<td>Object</td>
<td>Override to implement subtraction, used for - operator. Ex: a - b is equivalent to a.\_\_Sub\_\_(a, b)</td>
</tr>
<tr>
<td>\_\_Mul\_\_(self : Object,other : Object)</td>
<td>Object</td>
<td>Override to implement multiplication, used for * operator. Ex: a * b is equivalent to a.\_\_Mul\_\_(a, b)</td>
</tr>
<tr>
<td>\_\_Div\_\_(self : Object,other : Object)</td>
<td>Object</td>
<td>Override to implement division, used for / operator. Ex: a / b is equivalent to a.\_\_Div\_\_(a, b)</td>
</tr>
<tr>
<td>\_\_Eq\_\_(self : Object,other : Object)</td>
<td>bool</td>
<td>Override to implement equality comparison, used for == and != operators. Ex: a == b is equivalent to a.\_\_Eq\_\_(a, b)</td>
</tr>
<tr>
<td>\_\_Hash\_\_()</td>
<td>int</td>
<td>Override to implement hashing, used for GetHashCode - Used for Dictionaries/Sets. Ex: hash = obj.GetHashCode() is equivalent to hash = obj.\_\_Hash\_\_()</td>
</tr>
</tbody>
</table>
## Static Methods
<table>
<colgroup><col style="width: 30%"/>
<col style="width: 20%"/>
<col style="width: 50%"/>
</colgroup>
<thead>
<tr>
<th>Function</th>
<th>Returns</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr>
<td>Angle(from : [Vector2](../objects/Vector2.md),to : [Vector2](../objects/Vector2.md))</td>
<td>float</td>
<td>Gets the unsigned angle in degrees between from and to.</td>
</tr>
<tr>
<td>ClampMagnitude(vector : [Vector2](../objects/Vector2.md),maxLength : float)</td>
<td>[Vector2](../objects/Vector2.md)</td>
<td>Returns a copy of vector with its magnitude clamped to maxLength.</td>
</tr>
<tr>
<td>Distance(a : [Vector2](../objects/Vector2.md),b : [Vector2](../objects/Vector2.md))</td>
<td>float</td>
<td>Returns the distance between a and b.</td>
</tr>
<tr>
<td>Dot(a : [Vector2](../objects/Vector2.md),b : [Vector2](../objects/Vector2.md))</td>
<td>float</td>
<td>Dot Product of two vectors.</td>
</tr>
<tr>
<td>Lerp(a : [Vector2](../objects/Vector2.md),b : [Vector2](../objects/Vector2.md),t : float)</td>
<td>[Vector2](../objects/Vector2.md)</td>
<td>Linearly interpolates between vectors a and b by t.</td>
</tr>
<tr>
<td>LerpUnclamped(a : [Vector2](../objects/Vector2.md),b : [Vector2](../objects/Vector2.md),t : float)</td>
<td>[Vector2](../objects/Vector2.md)</td>
<td>Linearly interpolates between vectors a and b by t.</td>
</tr>
<tr>
<td>Max(a : [Vector2](../objects/Vector2.md),b : [Vector2](../objects/Vector2.md))</td>
<td>[Vector2](../objects/Vector2.md)</td>
<td>Returns a vector that is made from the largest components of two vectors.</td>
</tr>
<tr>
<td>Min(a : [Vector2](../objects/Vector2.md),b : [Vector2](../objects/Vector2.md))</td>
<td>[Vector2](../objects/Vector2.md)</td>
<td>Returns a vector that is made from the smallest components of two vectors.</td>
</tr>
<tr>
<td>MoveTowards(current : [Vector2](../objects/Vector2.md),target : [Vector2](../objects/Vector2.md),maxDistanceDelta : float)</td>
<td>[Vector2](../objects/Vector2.md)</td>
<td>Moves a point current towards target.</td>
</tr>
<tr>
<td>Reflect(inDirection : [Vector2](../objects/Vector2.md),inNormal : [Vector2](../objects/Vector2.md))</td>
<td>[Vector2](../objects/Vector2.md)</td>
<td>Reflects a vector off the vector defined by a normal.</td>
</tr>
<tr>
<td>SignedAngle(from : [Vector2](../objects/Vector2.md),to : [Vector2](../objects/Vector2.md))</td>
<td>float</td>
<td>Gets the signed angle in degrees between from and to.</td>
</tr>
<tr>
<td>SmoothDamp(current : [Vector2](../objects/Vector2.md),target : [Vector2](../objects/Vector2.md),currentVelocity : [Vector2](../objects/Vector2.md),smoothTime : float,maxSpeed : float)</td>
<td>[Vector2](../objects/Vector2.md)</td>
<td></td>
</tr>
</tbody>
</table>
