# PersistentData
Inherits from object
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
<td>SetProperty(property : [String](../static/String.md),value : Object)</td>
<td>none</td>
<td>Sets the property with given name to the object value. Valid value types are float, string, bool, and int.</td>
</tr>
<tr>
<td>GetProperty(property : [String](../static/String.md),defaultValue : Object)</td>
<td>Object</td>
<td>Gets the property with given name. If property does not exist, returns defaultValue.</td>
</tr>
<tr>
<td>LoadFromFile(fileName : [String](../static/String.md),encrypted : bool)</td>
<td>none</td>
<td>Loads persistent data from given file name. If encrypted is true, will treat the file as having been saved as encrypted.</td>
</tr>
<tr>
<td>SaveToFile(fileName : [String](../static/String.md),encrypted : bool)</td>
<td>none</td>
<td>Saves current persistent data to given file name. If encrypted is true, will also encrypt the file instead of using plaintext.</td>
</tr>
<tr>
<td>Clear()</td>
<td>none</td>
<td>Clears current persistent data.</td>
</tr>
<tr>
<td>IsValidFileName(fileName : [String](../static/String.md))</td>
<td>bool</td>
<td>Determines whether or not the given fileName will be allowed for use when saving/loading a file.</td>
</tr>
<tr>
<td>FileExists(fileName : [String](../static/String.md))</td>
<td>bool</td>
<td>Determines whether the file given already exists. Throws an error if given an invalid file name.</td>
</tr>
</tbody>
</table>
