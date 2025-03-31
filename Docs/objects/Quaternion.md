# Quaternion
Inherits from object
## Initialization
```csharp
# Quaternion(Object[])
example = Quaternion(Object[])
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
#### function <span style="color:yellow;">Lerp</span>(a: <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>, b: <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>, t: <span style="color:blue;">float</span>) → <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>
> Interpolates between a and b by t and normalizes the result afterwards.

#### function <span style="color:yellow;">LerpUnclamped</span>(a: <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>, b: <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>, t: <span style="color:blue;">float</span>) → <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>
> Interpolates between a and b by t and normalizes the result afterwards. The parameter t is not clamped.

#### function <span style="color:yellow;">Slerp</span>(a: <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>, b: <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>, t: <span style="color:blue;">float</span>) → <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>
> Spherically linear interpolates between unit quaternions a and b by a ratio of t.

#### function <span style="color:yellow;">SlerpUnclamped</span>(a: <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>, b: <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>, t: <span style="color:blue;">float</span>) → <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>
> Spherically linear interpolates between unit quaternions a and b by t.

#### function <span style="color:yellow;">FromEuler</span>(euler: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>
> Returns the Quaternion rotation from the given euler angles.

#### function <span style="color:yellow;">LookRotation</span>(forward: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, upwards: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span> = <span style="color:blue;">null</span>) → <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>
> Creates a rotation with the specified forward and upwards directions.

#### function <span style="color:yellow;">FromToRotation</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>
> Creates a rotation from fromDirection to toDirection.

#### function <span style="color:yellow;">Inverse</span>(q: <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>) → <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>
> Returns the Inverse of rotation.

#### function <span style="color:yellow;">RotateTowards</span>(from: <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>, to: <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>, maxDegreesDelta: <span style="color:blue;">float</span>) → <span style="color:blue;">[Quaternion](../objects/Quaternion.md)</span>
> Rotates a rotation from towards to.


---

