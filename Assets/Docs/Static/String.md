# String
Inherits from object
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Newline|[String](../Static/String.md)|False|Returns the newline character.|
## Methods
|Function|Returns|Description|
|---|---|---|
|FormatFloat(val : float, decimals : int)|[String](../Static/String.md)|Formats a float to a string with the specified number of decimal places.|
|FormatFromList(str : [String](../Static/String.md), list : [List](../Object/List.md))|[String](../Static/String.md)|Equivalent to C# string.format(string, List<string>).|
|Split(toSplit : [String](../Static/String.md), splitter : Object, removeEmptyEntries : bool = False)|[List](../Object/List.md)|Split the string into a list. Can pass in either a string to split on or a list of strings to split on, the last optional param can remove all empty entries.|
|Join(list : [List](../Object/List.md), separator : [String](../Static/String.md))|[String](../Static/String.md)|Joins a list of strings into a single string with the specified separator.|
|Substring(str : [String](../Static/String.md), startIndex : int)|[String](../Static/String.md)|Returns a substring starting from the specified index.|
|SubstringWithLength(str : [String](../Static/String.md), startIndex : int, length : int)|[String](../Static/String.md)|Returns a substring of the specified length starting from the specified start index.|
|Length(str : [String](../Static/String.md))|int|Length of the string.|
|Replace(str : [String](../Static/String.md), replace : [String](../Static/String.md), with : [String](../Static/String.md))|[String](../Static/String.md)|Replaces all occurrences of a substring with another substring.|
|Contains(str : [String](../Static/String.md), match : [String](../Static/String.md))|bool|Checks if the string contains the specified substring.|
|StartsWith(str : [String](../Static/String.md), match : [String](../Static/String.md))|bool|Checks if the string starts with the specified substring.|
|EndsWith(str : [String](../Static/String.md), match : [String](../Static/String.md))|bool|Checks if the string ends with the specified substring.|
|Trim(str : [String](../Static/String.md))|[String](../Static/String.md)|Trims whitespace from the start and end of the string.|
|Insert(str : [String](../Static/String.md), insert : [String](../Static/String.md), index : int)|[String](../Static/String.md)|Inserts a substring at the specified index.|
|Capitalize(str : [String](../Static/String.md))|[String](../Static/String.md)|Capitalizes the first letter of the string.|
|ToUpper(str : [String](../Static/String.md))|[String](../Static/String.md)|Converts the string to uppercase.|
|ToLower(str : [String](../Static/String.md))|[String](../Static/String.md)|Converts the string to lowercase.|
|IndexOf(str : [String](../Static/String.md), substring : [String](../Static/String.md))|int|Returns the index of the given string.|
