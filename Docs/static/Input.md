# Input
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
<td>GetKeyName(key : [String](../static/String.md))</td>
<td>[String](../static/String.md)</td>
<td>Gets the key name the player assigned to the key setting</td>
</tr>
<tr>
<td>GetKeyHold(key : [String](../static/String.md))</td>
<td>bool</td>
<td>Returns true if the key is being held down</td>
</tr>
<tr>
<td>GetKeyDown(key : [String](../static/String.md))</td>
<td>bool</td>
<td>Returns true if the key was pressed down this frame</td>
</tr>
<tr>
<td>GetKeyUp(key : [String](../static/String.md))</td>
<td>bool</td>
<td>Returns true if the key was released this frame</td>
</tr>
<tr>
<td>GetMouseAim()</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Returns the position the player is aiming at</td>
</tr>
<tr>
<td>GetCursorAimDirection()</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Returns the ray the player is aiming at</td>
</tr>
<tr>
<td>GetMouseSpeed()</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Returns the speed of the mouse</td>
</tr>
<tr>
<td>GetMousePosition()</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Returns the position of the mouse</td>
</tr>
<tr>
<td>GetScreenDimensions()</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Returns the dimensions of the screen</td>
</tr>
<tr>
<td>SetKeyDefaultEnabled(key : [String](../static/String.md),enabled : bool)</td>
<td>none</td>
<td>Sets whether the key is enabled by default</td>
</tr>
<tr>
<td>SetKeyHold(key : [String](../static/String.md),enabled : bool)</td>
<td>none</td>
<td>Sets whether the key is being held down</td>
</tr>
</tbody>
</table>
