# PersistentData
Inherits from object
## Static Methods
#### void SetProperty([String](../static/String.md) property, Object value)
- **Description:** Sets the property with given name to the object value. Valid value types are float, string, bool, and int.

---

#### Object GetProperty([String](../static/String.md) property, Object defaultValue)
- **Description:** Gets the property with given name. If property does not exist, returns defaultValue.

---

#### void LoadFromFile([String](../static/String.md) fileName, bool encrypted)
- **Description:** Loads persistent data from given file name. If encrypted is true, will treat the file as having been saved as encrypted.

---

#### void SaveToFile([String](../static/String.md) fileName, bool encrypted)
- **Description:** Saves current persistent data to given file name. If encrypted is true, will also encrypt the file instead of using plaintext.

---

#### void Clear()
- **Description:** Clears current persistent data.

---

#### bool IsValidFileName([String](../static/String.md) fileName)
- **Description:** Determines whether or not the given fileName will be allowed for use when saving/loading a file.

---

#### bool FileExists([String](../static/String.md) fileName)
- **Description:** Determines whether the file given already exists. Throws an error if given an invalid file name.
