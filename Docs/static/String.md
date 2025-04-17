# String
Inherits from object
## Initialization
<mark style="color:red;">This class is abstract and cannot be instantiated.</mark>
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Newline|string|True|Returns the newline character.|
## Static Methods
###### function <mark style="color:yellow;">FormatFloat</mark>(val: <mark style="color:blue;">float</mark>, decimals: <mark style="color:blue;">int</mark>) → <mark style="color:blue;">string</mark>
> Formats a float to a string with the specified number of decimal places.

###### function <mark style="color:yellow;">FormatFromList</mark>(str: <mark style="color:blue;">string</mark>, list: <mark style="color:blue;">[List](../objects/List.md)</mark>) → <mark style="color:blue;">string</mark>
> Equivalent to C# string.format(string, List<string>).

###### function <mark style="color:yellow;">Split</mark>(toSplit: <mark style="color:blue;">string</mark>, splitter: <mark style="color:blue;">Object</mark>, removeEmptyEntries: <mark style="color:blue;">bool</mark> = <mark style="color:blue;">False</mark>) → <mark style="color:blue;">[List](../objects/List.md)</mark>
> Split the string into a list. Can pass in either a string to split on or a list of strings to split on, the last optional param can remove all empty entries.

###### function <mark style="color:yellow;">Join</mark>(list: <mark style="color:blue;">[List](../objects/List.md)</mark>, separator: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">string</mark>
> Joins a list of strings into a single string with the specified separator.

###### function <mark style="color:yellow;">Substring</mark>(str: <mark style="color:blue;">string</mark>, startIndex: <mark style="color:blue;">int</mark>) → <mark style="color:blue;">string</mark>
> Returns a substring starting from the specified index.

###### function <mark style="color:yellow;">SubstringWithLength</mark>(str: <mark style="color:blue;">string</mark>, startIndex: <mark style="color:blue;">int</mark>, length: <mark style="color:blue;">int</mark>) → <mark style="color:blue;">string</mark>
> Returns a substring of the specified length starting from the specified start index.

###### function <mark style="color:yellow;">Length</mark>(str: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">int</mark>
> Length of the string.

###### function <mark style="color:yellow;">Replace</mark>(str: <mark style="color:blue;">string</mark>, replace: <mark style="color:blue;">string</mark>, with: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">string</mark>
> Replaces all occurrences of a substring with another substring.

###### function <mark style="color:yellow;">Contains</mark>(str: <mark style="color:blue;">string</mark>, match: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">bool</mark>
> Checks if the string contains the specified substring.

###### function <mark style="color:yellow;">StartsWith</mark>(str: <mark style="color:blue;">string</mark>, match: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">bool</mark>
> Checks if the string starts with the specified substring.

###### function <mark style="color:yellow;">EndsWith</mark>(str: <mark style="color:blue;">string</mark>, match: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">bool</mark>
> Checks if the string ends with the specified substring.

###### function <mark style="color:yellow;">Trim</mark>(str: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">string</mark>
> Trims whitespace from the start and end of the string.

###### function <mark style="color:yellow;">Insert</mark>(str: <mark style="color:blue;">string</mark>, insert: <mark style="color:blue;">string</mark>, index: <mark style="color:blue;">int</mark>) → <mark style="color:blue;">string</mark>
> Inserts a substring at the specified index.

###### function <mark style="color:yellow;">Capitalize</mark>(str: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">string</mark>
> Capitalizes the first letter of the string.

###### function <mark style="color:yellow;">ToUpper</mark>(str: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">string</mark>
> Converts the string to uppercase.

###### function <mark style="color:yellow;">ToLower</mark>(str: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">string</mark>
> Converts the string to lowercase.

###### function <mark style="color:yellow;">IndexOf</mark>(str: <mark style="color:blue;">string</mark>, substring: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">int</mark>
> Returns the index of the given string.


---

