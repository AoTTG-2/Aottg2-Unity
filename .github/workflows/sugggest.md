# Character API Reference

The **Character** type represents an in-game entity with properties and methods that control its behavior, appearance, and interactions in a Unity environment.

---

## Properties

**Name**  
*Type:* `String`  
*Description:* The character’s name.

**Guild**  
*Type:* `String`  
*Description:* The character’s guild.

**Player**  
*Type:* `Player`  
*Description:* The player who owns this character.

**IsAI**  
*Type:* `bool`  
*Description:* Indicates whether the character is controlled by AI.

**ViewID**  
*Type:* `int`  
*Description:* The network view ID of the character.

**IsMine**  
*Type:* `bool`  
*Description:* Indicates if the character is controlled by the local player.

**IsMainCharacter**  
*Type:* `bool`  
*Description:* *(No description provided.)*

**Transform**  
*Type:* `Transform`  
*Description:* The Unity transform of the character.

**Position**  
*Type:* `Vector3`  
*Description:* The position of the character in the game world.

**Rotation**  
*Type:* `Vector3`  
*Description:* The rotation of the character expressed in Euler angles.

**QuaternionRotation**  
*Type:* `Quaternion`  
*Description:* The rotation of the character expressed as a quaternion.

**Velocity**  
*Type:* `Vector3`  
*Description:* The current velocity of the character.

**Forward**  
*Type:* `Vector3`  
*Description:* The forward direction vector of the character.

**Right**  
*Type:* `Vector3`  
*Description:* The right direction vector of the character.

**Up**  
*Type:* `Vector3`  
*Description:* The upward direction vector of the character.

**HasTargetDirection**  
*Type:* `bool`  
*Description:* Indicates whether the character is turning towards a target direction.

**TargetDirection**  
*Type:* `Vector3`  
*Description:* The target direction the character is aiming for.

**Team**  
*Type:* `String`  
*Description:* The team to which the character belongs.

**Health**  
*Type:* `float`  
*Description:* The current health of the character.

**MaxHealth**  
*Type:* `float`  
*Description:* The maximum health of the character.

**CustomDamageEnabled**  
*Type:* `bool`  
*Description:* Determines if custom damage dealing is enabled.

**CustomDamage**  
*Type:* `int`  
*Description:* The amount of custom damage to deal per attack.

**CurrentAnimation**  
*Type:* `String`  
*Description:* The currently playing animation of the character.

**Grounded**  
*Type:* `bool`  
*Description:* Indicates whether the character is grounded.

---

## Methods

### GetKilled(killer: String)
- **Returns:** `none`
- **Description:** Instantly kills the character.  
- **Notes:** This method is callable by non-owners.

---

### GetDamaged(killer: String, damage: int)
- **Returns:** `none`
- **Description:** Applies damage to the character and kills it if the health drops to 0.  
- **Notes:** This method is callable by non-owners.

---

### Emote(emote: String)
- **Returns:** `none`
- **Description:** Causes the character to perform an emote.  
- **Notes:** The available emotes correspond to those shown in the in-game emote menu.

---

### PlayAnimation(animation: String, fade: float = 0.1)
- **Returns:** `none`
- **Description:** Plays the specified animation on the character.  
- **Parameters:**  
  - `animation`: The name of the animation to play.  
  - `fade`: *(Optional)* The duration for crossfading to the new animation (default is 0.1 seconds).  
- **Notes:** Available animations include: *Human, Titan, Annie, Eren* (use the appropriate string identifier).

---

### GetAnimationLength(animation: String)
- **Returns:** `float`
- **Description:** Retrieves the duration of the specified animation.

---

### PlaySound(sound: String)
- **Returns:** `none`
- **Description:** Plays a sound associated with the character.  
- **Notes:** Available sound names are: *Humans, Shifters, Titans*. (Shifters also include all titan sounds.)

---

### StopSound(sound: String)
- **Returns:** `none`
- **Description:** Stops the sound currently being played on the character.

---

### LookAt(position: Vector3)
- **Returns:** `none`
- **Description:** Rotates the character to face a specified world position.

---

### AddForce(force: Vector3, mode: String = "Acceleration")
- **Returns:** `none`
- **Description:** Applies a force to the character using the provided vector.  
- **Parameters:**  
  - `force`: The force vector to apply.  
  - `mode`: *(Optional)* The mode in which to apply the force.  
- **Notes:** Valid force modes include: *Force, Acceleration, Impulse, VelocityChange* (default is *Acceleration*).

---

### Reveal(delay: float)
- **Returns:** `none`
- **Description:** Reveals the titan for the specified number of seconds.

---

### AddOutline(color: Color, mode: String = "OutlineAll")
- **Returns:** `none`
- **Description:** Adds an outline effect to the character.  
- **Parameters:**  
  - `color`: The color of the outline.  
  - `mode`: *(Optional)* The outline mode to apply.  
- **Notes:** Valid modes include: *OutlineAll, OutlineVisible, OutlineHidden, OutlineAndSilhouette, SilhouetteOnly, OutlineAndLightenColor*.

---

### RemoveOutline()
- **Returns:** `none`
- **Description:** Removes any outline effect from the character.