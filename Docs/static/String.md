# String
Inherits from object
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Newline|[String](../static/String.md)|False|Returns the newline character.|
## Static Methods
<table>
<colgroup><col style="width: 30%"/>
<col style="width: 20%"/>
<col style="width: 50%"/>
</colgroup>
<thead>
<tr>
<th>Function</th>
<th>Returns</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr>
<td>FormatFloat(val : float,decimals : int)</td>
<td>[String](../static/String.md)</td>
<td>Formats a float to a string with the specified number of decimal places.</td>
</tr>
<tr>
<td>FormatFromList(str : [String](../static/String.md),list : [List](../objects/List.md))</td>
<td>[String](../static/String.md)</td>
<td>Equivalent to C# string.format(string, List<string>).</td>
</tr>
<tr>
<td>Split(toSplit : [String](../static/String.md),splitter : Object,removeEmptyEntries : bool = False)</td>
<td>[List](../objects/List.md)</td>
<td>Split the string into a list. Can pass in either a string to split on or a list of strings to split on, the last optional param can remove all empty entries.</td>
</tr>
<tr>
<td>Join(list : [List](../objects/List.md),separator : [String](../static/String.md))</td>
<td>[String](../static/String.md)</td>
<td>Joins a list of strings into a single string with the specified separator.</td>
</tr>
<tr>
<td>Substring(str : [String](../static/String.md),startIndex : int)</td>
<td>[String](../static/String.md)</td>
<td>Returns a substring starting from the specified index.</td>
</tr>
<tr>
<td>SubstringWithLength(str : [String](../static/String.md),startIndex : int,length : int)</td>
<td>[String](../static/String.md)</td>
<td>Returns a substring of the specified length starting from the specified start index.</td>
</tr>
<tr>
<td>Length(str : [String](../static/String.md))</td>
<td>int</td>
<td>Length of the string.</td>
</tr>
<tr>
<td>Replace(str : [String](../static/String.md),replace : [String](../static/String.md),with : [String](../static/String.md))</td>
<td>[String](../static/String.md)</td>
<td>Replaces all occurrences of a substring with another substring.</td>
</tr>
<tr>
<td>Contains(str : [String](../static/String.md),match : [String](../static/String.md))</td>
<td>bool</td>
<td>Checks if the string contains the specified substring.</td>
</tr>
<tr>
<td>StartsWith(str : [String](../static/String.md),match : [String](../static/String.md))</td>
<td>bool</td>
<td>Checks if the string starts with the specified substring.</td>
</tr>
<tr>
<td>EndsWith(str : [String](../static/String.md),match : [String](../static/String.md))</td>
<td>bool</td>
<td>Checks if the string ends with the specified substring.</td>
</tr>
<tr>
<td>Trim(str : [String](../static/String.md))</td>
<td>[String](../static/String.md)</td>
<td>Trims whitespace from the start and end of the string.</td>
</tr>
<tr>
<td>Insert(str : [String](../static/String.md),insert : [String](../static/String.md),index : int)</td>
<td>[String](../static/String.md)</td>
<td>Inserts a substring at the specified index.</td>
</tr>
<tr>
<td>Capitalize(str : [String](../static/String.md))</td>
<td>[String](../static/String.md)</td>
<td>Capitalizes the first letter of the string.</td>
</tr>
<tr>
<td>ToUpper(str : [String](../static/String.md))</td>
<td>[String](../static/String.md)</td>
<td>Converts the string to uppercase.</td>
</tr>
<tr>
<td>ToLower(str : [String](../static/String.md))</td>
<td>[String](../static/String.md)</td>
<td>Converts the string to lowercase.</td>
</tr>
<tr>
<td>IndexOf(str : [String](../static/String.md),substring : [String](../static/String.md))</td>
<td>int</td>
<td>Returns the index of the given string.</td>
</tr>
</tbody>
</table>
