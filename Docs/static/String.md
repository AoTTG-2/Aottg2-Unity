# String
Inherits from object
## Initialization
<span style="color:red;">This class is abstract and cannot be instantiated.</span>
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Newline|string|True|Returns the newline character.|
## Static Methods
#### function <span style="color:yellow;">FormatFloat</span>(val: <span style="color:blue;">float</span>, decimals: <span style="color:blue;">int</span>) → <span style="color:blue;">string</span>
> Formats a float to a string with the specified number of decimal places.

#### function <span style="color:yellow;">FormatFromList</span>(str: <span style="color:blue;">string</span>, list: <span style="color:blue;">[List](../objects/List.md)</span>) → <span style="color:blue;">string</span>
> Equivalent to C# string.format(string, List<string>).

#### function <span style="color:yellow;">Split</span>(toSplit: <span style="color:blue;">string</span>, splitter: <span style="color:blue;">Object</span>, removeEmptyEntries: <span style="color:blue;">bool</span> = <span style="color:blue;">False</span>) → <span style="color:blue;">[List](../objects/List.md)</span>
> Split the string into a list. Can pass in either a string to split on or a list of strings to split on, the last optional param can remove all empty entries.

#### function <span style="color:yellow;">Join</span>(list: <span style="color:blue;">[List](../objects/List.md)</span>, separator: <span style="color:blue;">string</span>) → <span style="color:blue;">string</span>
> Joins a list of strings into a single string with the specified separator.

#### function <span style="color:yellow;">Substring</span>(str: <span style="color:blue;">string</span>, startIndex: <span style="color:blue;">int</span>) → <span style="color:blue;">string</span>
> Returns a substring starting from the specified index.

#### function <span style="color:yellow;">SubstringWithLength</span>(str: <span style="color:blue;">string</span>, startIndex: <span style="color:blue;">int</span>, length: <span style="color:blue;">int</span>) → <span style="color:blue;">string</span>
> Returns a substring of the specified length starting from the specified start index.

#### function <span style="color:yellow;">Length</span>(str: <span style="color:blue;">string</span>) → <span style="color:blue;">int</span>
> Length of the string.

#### function <span style="color:yellow;">Replace</span>(str: <span style="color:blue;">string</span>, replace: <span style="color:blue;">string</span>, with: <span style="color:blue;">string</span>) → <span style="color:blue;">string</span>
> Replaces all occurrences of a substring with another substring.

#### function <span style="color:yellow;">Contains</span>(str: <span style="color:blue;">string</span>, match: <span style="color:blue;">string</span>) → <span style="color:blue;">bool</span>
> Checks if the string contains the specified substring.

#### function <span style="color:yellow;">StartsWith</span>(str: <span style="color:blue;">string</span>, match: <span style="color:blue;">string</span>) → <span style="color:blue;">bool</span>
> Checks if the string starts with the specified substring.

#### function <span style="color:yellow;">EndsWith</span>(str: <span style="color:blue;">string</span>, match: <span style="color:blue;">string</span>) → <span style="color:blue;">bool</span>
> Checks if the string ends with the specified substring.

#### function <span style="color:yellow;">Trim</span>(str: <span style="color:blue;">string</span>) → <span style="color:blue;">string</span>
> Trims whitespace from the start and end of the string.

#### function <span style="color:yellow;">Insert</span>(str: <span style="color:blue;">string</span>, insert: <span style="color:blue;">string</span>, index: <span style="color:blue;">int</span>) → <span style="color:blue;">string</span>
> Inserts a substring at the specified index.

#### function <span style="color:yellow;">Capitalize</span>(str: <span style="color:blue;">string</span>) → <span style="color:blue;">string</span>
> Capitalizes the first letter of the string.

#### function <span style="color:yellow;">ToUpper</span>(str: <span style="color:blue;">string</span>) → <span style="color:blue;">string</span>
> Converts the string to uppercase.

#### function <span style="color:yellow;">ToLower</span>(str: <span style="color:blue;">string</span>) → <span style="color:blue;">string</span>
> Converts the string to lowercase.

#### function <span style="color:yellow;">IndexOf</span>(str: <span style="color:blue;">string</span>, substring: <span style="color:blue;">string</span>) → <span style="color:blue;">int</span>
> Returns the index of the given string.


---

