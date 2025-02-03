# Quaternion
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|X|float|False|X component of the Quaternion. Don't modify this directly unless you know quaternions inside out.|
|Y|float|False|Y component of the Quaternion. Don't modify this directly unless you know quaternions inside out.|
|Z|float|False|Z component of the Quaternion. Don't modify this directly unless you know quaternions inside out.|
|W|float|False|W component of the Quaternion. Do not directly modify quaternions.|
|Euler|[Vector3](../Static/Vector3.md)|False|Returns or sets the euler angle representation of the rotation.|
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Identity|[Quaternion](../Static/Quaternion.md)|False|The identity rotation (Read Only).|
## Methods
|Function|Returns|Description|
|---|---|---|
|Lerp(a : [Quaternion](../Static/Quaternion.md), b : [Quaternion](../Static/Quaternion.md), t : float)|[Quaternion](../Static/Quaternion.md)|Interpolates between a and b by t and normalizes the result afterwards.|
|LerpUnclamped(a : [Quaternion](../Static/Quaternion.md), b : [Quaternion](../Static/Quaternion.md), t : float)|[Quaternion](../Static/Quaternion.md)|Interpolates between a and b by t and normalizes the result afterwards. The parameter t is not clamped.|
|Slerp(a : [Quaternion](../Static/Quaternion.md), b : [Quaternion](../Static/Quaternion.md), t : float)|[Quaternion](../Static/Quaternion.md)|Spherically linear interpolates between unit quaternions a and b by a ratio of t.|
|SlerpUnclamped(a : [Quaternion](../Static/Quaternion.md), b : [Quaternion](../Static/Quaternion.md), t : float)|[Quaternion](../Static/Quaternion.md)|Spherically linear interpolates between unit quaternions a and b by t.|
|FromEuler(euler : [Vector3](../Static/Vector3.md))|[Quaternion](../Static/Quaternion.md)|Returns the Quaternion rotation from the given euler angles.|
|LookRotation(forward : [Vector3](../Static/Vector3.md), upwards : [Vector3](../Static/Vector3.md) = )|[Quaternion](../Static/Quaternion.md)|Creates a rotation with the specified forward and upwards directions.|
|FromToRotation(a : [Vector3](../Static/Vector3.md), b : [Vector3](../Static/Vector3.md))|[Quaternion](../Static/Quaternion.md)|Creates a rotation from fromDirection to toDirection.|
|Inverse(q : [Quaternion](../Static/Quaternion.md))|[Quaternion](../Static/Quaternion.md)|Returns the Inverse of rotation.|
|RotateTowards(from : [Quaternion](../Static/Quaternion.md), to : [Quaternion](../Static/Quaternion.md), maxDegreesDelta : float)|[Quaternion](../Static/Quaternion.md)|Rotates a rotation from towards to.|
|\_\_Copy\_\_()|Object||
|\_\_Add\_\_(self : Object, other : Object)|Object||
|\_\_Sub\_\_(self : Object, other : Object)|Object||
|\_\_Mul\_\_(self : Object, other : Object)|Object||
|\_\_Div\_\_(self : Object, other : Object)|Object||
|\_\_Eq\_\_(self : Object, other : Object)|bool||
|\_\_Hash\_\_()|int||
