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
#### Object \_\_Copy\_\_()
- **Description:** Override to deepcopy object on assignment, used for structs. Ex: copy = original is equivalent to copy = original.\_\_Copy\_\_()

---

#### Object \_\_Add\_\_(Object self, Object other)
- **Description:** Override to implement addition, used for + operator. Ex: a + b is equivalent to a.\_\_Add\_\_(a, b)

---

#### Object \_\_Sub\_\_(Object self, Object other)
- **Description:** Override to implement subtraction, used for - operator. Ex: a - b is equivalent to a.\_\_Sub\_\_(a, b)

---

#### Object \_\_Mul\_\_(Object self, Object other)
- **Description:** Override to implement multiplication, used for * operator. Ex: a * b is equivalent to a.\_\_Mul\_\_(a, b)

---

#### Object \_\_Div\_\_(Object self, Object other)
- **Description:** Override to implement division, used for / operator. Ex: a / b is equivalent to a.\_\_Div\_\_(a, b)

---

#### bool \_\_Eq\_\_(Object self, Object other)
- **Description:** Override to implement equality comparison, used for == and != operators. Ex: a == b is equivalent to a.\_\_Eq\_\_(a, b)

---

#### int \_\_Hash\_\_()
- **Description:** Override to implement hashing, used for GetHashCode - Used for Dictionaries/Sets. Ex: hash = obj.GetHashCode() is equivalent to hash = obj.\_\_Hash\_\_()
## Static Methods
#### [Quaternion](../objects/Quaternion.md) Lerp([Quaternion](../objects/Quaternion.md) a, [Quaternion](../objects/Quaternion.md) b, float t)
- **Description:** Interpolates between a and b by t and normalizes the result afterwards.

---

#### [Quaternion](../objects/Quaternion.md) LerpUnclamped([Quaternion](../objects/Quaternion.md) a, [Quaternion](../objects/Quaternion.md) b, float t)
- **Description:** Interpolates between a and b by t and normalizes the result afterwards. The parameter t is not clamped.

---

#### [Quaternion](../objects/Quaternion.md) Slerp([Quaternion](../objects/Quaternion.md) a, [Quaternion](../objects/Quaternion.md) b, float t)
- **Description:** Spherically linear interpolates between unit quaternions a and b by a ratio of t.

---

#### [Quaternion](../objects/Quaternion.md) SlerpUnclamped([Quaternion](../objects/Quaternion.md) a, [Quaternion](../objects/Quaternion.md) b, float t)
- **Description:** Spherically linear interpolates between unit quaternions a and b by t.

---

#### [Quaternion](../objects/Quaternion.md) FromEuler([Vector3](../objects/Vector3.md) euler)
- **Description:** Returns the Quaternion rotation from the given euler angles.

---

#### [Quaternion](../objects/Quaternion.md) LookRotation([Vector3](../objects/Vector3.md) forward, [Vector3](../objects/Vector3.md) upwards = null)
- **Description:** Creates a rotation with the specified forward and upwards directions.

---

#### [Quaternion](../objects/Quaternion.md) FromToRotation([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b)
- **Description:** Creates a rotation from fromDirection to toDirection.

---

#### [Quaternion](../objects/Quaternion.md) Inverse([Quaternion](../objects/Quaternion.md) q)
- **Description:** Returns the Inverse of rotation.

---

#### [Quaternion](../objects/Quaternion.md) RotateTowards([Quaternion](../objects/Quaternion.md) from, [Quaternion](../objects/Quaternion.md) to, float maxDegreesDelta)
- **Description:** Rotates a rotation from towards to.
