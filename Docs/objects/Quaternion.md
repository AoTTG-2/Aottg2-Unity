# Quaternion
Inherits from object
## Initialization
> Constructors:
```csharp

# Quaternion takes four floats X, Y, Z and W as parameters when initializing. 
quaternion = Quaternion(0.5, 0.5, 0.5, 0.5);
```
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
|Identity|[Quaternion](../objects/Quaternion.md)|True|The identity rotation (Read Only).|
## Methods
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


---

## Static Methods
###### function <mark style="color:yellow;">Lerp</mark>(a: <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>, b: <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>, t: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>
> Interpolates between a and b by t and normalizes the result afterwards.

###### function <mark style="color:yellow;">LerpUnclamped</mark>(a: <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>, b: <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>, t: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>
> Interpolates between a and b by t and normalizes the result afterwards. The parameter t is not clamped.

###### function <mark style="color:yellow;">Slerp</mark>(a: <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>, b: <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>, t: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>
> Spherically linear interpolates between unit quaternions a and b by a ratio of t.

###### function <mark style="color:yellow;">SlerpUnclamped</mark>(a: <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>, b: <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>, t: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>
> Spherically linear interpolates between unit quaternions a and b by t.

###### function <mark style="color:yellow;">FromEuler</mark>(euler: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>
> Returns the Quaternion rotation from the given euler angles.

###### function <mark style="color:yellow;">LookRotation</mark>(forward: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, upwards: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark> = <mark style="color:blue;">null</mark>) → <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>
> Creates a rotation with the specified forward and upwards directions.

###### function <mark style="color:yellow;">FromToRotation</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>
> Creates a rotation from fromDirection to toDirection.

###### function <mark style="color:yellow;">Inverse</mark>(q: <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>) → <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>
> Returns the Inverse of rotation.

###### function <mark style="color:yellow;">RotateTowards</mark>(from: <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>, to: <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>, maxDegreesDelta: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[Quaternion](../objects/Quaternion.md)</mark>
> Rotates a rotation from towards to.


---

