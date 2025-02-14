# String
Inherits from object
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Newline|[String](../static/String.md)|False|Returns the newline character.|
## Static Methods
##### [String](../static/String.md) FormatFloat(float val, int decimals)
- **Description:** Formats a float to a string with the specified number of decimal places.
##### [String](../static/String.md) FormatFromList([String](../static/String.md) str, [List](../objects/List.md) list)
- **Description:** Equivalent to C# string.format(string, List<string>).
##### [List](../objects/List.md) Split([String](../static/String.md) toSplit, Object splitter, bool removeEmptyEntries = False)
- **Description:** Split the string into a list. Can pass in either a string to split on or a list of strings to split on, the last optional param can remove all empty entries.
##### [String](../static/String.md) Join([List](../objects/List.md) list, [String](../static/String.md) separator)
- **Description:** Joins a list of strings into a single string with the specified separator.
##### [String](../static/String.md) Substring([String](../static/String.md) str, int startIndex)
- **Description:** Returns a substring starting from the specified index.
##### [String](../static/String.md) SubstringWithLength([String](../static/String.md) str, int startIndex, int length)
- **Description:** Returns a substring of the specified length starting from the specified start index.
##### int Length([String](../static/String.md) str)
- **Description:** Length of the string.
##### [String](../static/String.md) Replace([String](../static/String.md) str, [String](../static/String.md) replace, [String](../static/String.md) with)
- **Description:** Replaces all occurrences of a substring with another substring.
##### bool Contains([String](../static/String.md) str, [String](../static/String.md) match)
- **Description:** Checks if the string contains the specified substring.
##### bool StartsWith([String](../static/String.md) str, [String](../static/String.md) match)
- **Description:** Checks if the string starts with the specified substring.
##### bool EndsWith([String](../static/String.md) str, [String](../static/String.md) match)
- **Description:** Checks if the string ends with the specified substring.
##### [String](../static/String.md) Trim([String](../static/String.md) str)
- **Description:** Trims whitespace from the start and end of the string.
##### [String](../static/String.md) Insert([String](../static/String.md) str, [String](../static/String.md) insert, int index)
- **Description:** Inserts a substring at the specified index.
##### [String](../static/String.md) Capitalize([String](../static/String.md) str)
- **Description:** Capitalizes the first letter of the string.
##### [String](../static/String.md) ToUpper([String](../static/String.md) str)
- **Description:** Converts the string to uppercase.
##### [String](../static/String.md) ToLower([String](../static/String.md) str)
- **Description:** Converts the string to lowercase.
##### int IndexOf([String](../static/String.md) str, [String](../static/String.md) substring)
- **Description:** Returns the index of the given string.

---

