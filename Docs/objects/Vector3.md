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
<td>Set(x : float,y : float,z : float)</td>
<td>none</td>
<td>Set x, y and z components of an existing Vector3.</td>
</tr>
<tr>
<td>Scale(scale : Object)</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Returns the Vector3 multiplied by scale.</td>
</tr>
<tr>
<td>Multiply(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Returns the multiplication of two Vector3s.</td>
</tr>
<tr>
<td>Divide(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Returns the division of two Vector3s.</td>
</tr>
<tr>
<td>GetRotationDirection(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Gets the relational Vector3 "b" using "a" as a reference. This is equivalent to setting MapObject.Forward to Vector "a", and finding the relative "b" vector.</td>
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
<td>Angle(from : [Vector3](../objects/Vector3.md),to : [Vector3](../objects/Vector3.md))</td>
<td>float</td>
<td>Calculates the angle between vectors from and.</td>
</tr>
<tr>
<td>ClampMagnitude(vector : [Vector3](../objects/Vector3.md),maxLength : float)</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Returns a copy of vector with its magnitude clamped to maxLength.</td>
</tr>
<tr>
<td>Cross(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Cross Product of two vectors.</td>
</tr>
<tr>
<td>Distance(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md))</td>
<td>float</td>
<td>Returns the distance between a and b.</td>
</tr>
<tr>
<td>Dot(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md))</td>
<td>float</td>
<td>Dot Product of two vectors.</td>
</tr>
<tr>
<td>Lerp(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md),t : float)</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Linearly interpolates between two points.</td>
</tr>
<tr>
<td>LerpUnclamped(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md),t : float)</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Linearly interpolates between two vectors.</td>
</tr>
<tr>
<td>Max(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Returns a vector that is made from the largest components of two vectors.</td>
</tr>
<tr>
<td>Min(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Returns a vector that is made from the smallest components of two vectors.</td>
</tr>
<tr>
<td>MoveTowards(current : [Vector3](../objects/Vector3.md),target : [Vector3](../objects/Vector3.md),maxDistanceDelta : float)</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Calculate a position between the points specified by current and target, moving no farther than the distance specified by maxDistanceDelta.</td>
</tr>
<tr>
<td>Normalize(value : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Makes this vector have a magnitude of 1.</td>
</tr>
<tr>
<td>OrthoNormalize(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md))</td>
<td>none</td>
<td></td>
</tr>
<tr>
<td>Project(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Projects a vector onto another vector.</td>
</tr>
<tr>
<td>ProjectOnPlane(vector : [Vector3](../objects/Vector3.md),plane : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Projects a vector onto a plane defined by a normal orthogonal to the plane.</td>
</tr>
<tr>
<td>Reflect(inDirection : [Vector3](../objects/Vector3.md),inNormal : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Reflects a vector off the plane defined by a normal.</td>
</tr>
<tr>
<td>RotateTowards(current : [Vector3](../objects/Vector3.md),target : [Vector3](../objects/Vector3.md),maxRadiansDelta : float,maxMagnitudeDelta : float)</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Rotates a vector current towards target.</td>
</tr>
<tr>
<td>SignedAngle(from : [Vector3](../objects/Vector3.md),to : [Vector3](../objects/Vector3.md),axis : [Vector3](../objects/Vector3.md))</td>
<td>float</td>
<td>Calculates the signed angle between vectors from and to in relation to axis.</td>
</tr>
<tr>
<td>Slerp(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md),t : float)</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Spherically interpolates between two vectors.</td>
</tr>
<tr>
<td>SlerpUnclamped(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md),t : float)</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Spherically interpolates between two vectors.</td>
</tr>
<tr>
<td>SmoothDamp(current : [Vector3](../objects/Vector3.md),target : [Vector3](../objects/Vector3.md),currentVelocity : [Vector3](../objects/Vector3.md),smoothTime : float,maxSpeed : float)</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td></td>
</tr>
</tbody>
</table>
