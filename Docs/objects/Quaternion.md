# Quaternion
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|X|float|False|X component of the Quaternion. Don't modify this directly unless you know quaternions inside out.|
|Y|float|False|Y component of the Quaternion. Don't modify this directly unless you know quaternions inside out.|
|Z|float|False|Z component of the Quaternion. Don't modify this directly unless you know quaternions inside out.|
|W|float|False|W component of the Quaternion. Do not directly modify quaternions.|
|Euler|[Vector3](../objects/Vector3.md)|False|Returns or sets the euler angle representation of the rotation.|
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Identity|[Quaternion](../objects/Quaternion.md)|False|The identity rotation (Read Only).|
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
<td>Lerp(a : [Quaternion](../objects/Quaternion.md),b : [Quaternion](../objects/Quaternion.md),t : float)</td>
<td>[Quaternion](../objects/Quaternion.md)</td>
<td>Interpolates between a and b by t and normalizes the result afterwards.</td>
</tr>
<tr>
<td>LerpUnclamped(a : [Quaternion](../objects/Quaternion.md),b : [Quaternion](../objects/Quaternion.md),t : float)</td>
<td>[Quaternion](../objects/Quaternion.md)</td>
<td>Interpolates between a and b by t and normalizes the result afterwards. The parameter t is not clamped.</td>
</tr>
<tr>
<td>Slerp(a : [Quaternion](../objects/Quaternion.md),b : [Quaternion](../objects/Quaternion.md),t : float)</td>
<td>[Quaternion](../objects/Quaternion.md)</td>
<td>Spherically linear interpolates between unit quaternions a and b by a ratio of t.</td>
</tr>
<tr>
<td>SlerpUnclamped(a : [Quaternion](../objects/Quaternion.md),b : [Quaternion](../objects/Quaternion.md),t : float)</td>
<td>[Quaternion](../objects/Quaternion.md)</td>
<td>Spherically linear interpolates between unit quaternions a and b by t.</td>
</tr>
<tr>
<td>FromEuler(euler : [Vector3](../objects/Vector3.md))</td>
<td>[Quaternion](../objects/Quaternion.md)</td>
<td>Returns the Quaternion rotation from the given euler angles.</td>
</tr>
<tr>
<td>LookRotation(forward : [Vector3](../objects/Vector3.md),upwards : [Vector3](../objects/Vector3.md) = )</td>
<td>[Quaternion](../objects/Quaternion.md)</td>
<td>Creates a rotation with the specified forward and upwards directions.</td>
</tr>
<tr>
<td>FromToRotation(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md))</td>
<td>[Quaternion](../objects/Quaternion.md)</td>
<td>Creates a rotation from fromDirection to toDirection.</td>
</tr>
<tr>
<td>Inverse(q : [Quaternion](../objects/Quaternion.md))</td>
<td>[Quaternion](../objects/Quaternion.md)</td>
<td>Returns the Inverse of rotation.</td>
</tr>
<tr>
<td>RotateTowards(from : [Quaternion](../objects/Quaternion.md),to : [Quaternion](../objects/Quaternion.md),maxDegreesDelta : float)</td>
<td>[Quaternion](../objects/Quaternion.md)</td>
<td>Rotates a rotation from towards to.</td>
</tr>
</tbody>
</table>
