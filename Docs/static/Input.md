# Input
Inherits from object
## Static Methods
#### [String](../static/String.md) GetKeyName([String](../static/String.md) key)
- **Description:** Gets the key name the player assigned to the key setting

---

#### bool GetKeyHold([String](../static/String.md) key)
- **Description:** Returns true if the key is being held down

---

#### bool GetKeyDown([String](../static/String.md) key)
- **Description:** Returns true if the key was pressed down this frame

---

#### bool GetKeyUp([String](../static/String.md) key)
- **Description:** Returns true if the key was released this frame

---

#### [Vector3](../objects/Vector3.md) GetMouseAim()
- **Description:** Returns the position the player is aiming at

---

#### [Vector3](../objects/Vector3.md) GetCursorAimDirection()
- **Description:** Returns the ray the player is aiming at

---

#### [Vector3](../objects/Vector3.md) GetMouseSpeed()
- **Description:** Returns the speed of the mouse

---

#### [Vector3](../objects/Vector3.md) GetMousePosition()
- **Description:** Returns the position of the mouse

---

#### [Vector3](../objects/Vector3.md) GetScreenDimensions()
- **Description:** Returns the dimensions of the screen

---

#### void SetKeyDefaultEnabled([String](../static/String.md) key, bool enabled)
- **Description:** Sets whether the key is enabled by default

---

#### void SetKeyHold([String](../static/String.md) key, bool enabled)
- **Description:** Sets whether the key is being held down
