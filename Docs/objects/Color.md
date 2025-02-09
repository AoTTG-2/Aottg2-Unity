# Color
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|R|int|False|Red component of the color|
|G|int|False|Green component of the color|
|B|int|False|Blue component of the color|
|A|int|False|Alpha component of the color|
## Methods
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
<td>ToHexString()</td>
<td>[String](../static/String.md)</td>
<td>Converts the color to a hex string</td>
</tr>
</tbody>
</table>
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
<td>Lerp(a : [Color](../objects/Color.md),b : [Color](../objects/Color.md),t : float)</td>
<td>[Color](../objects/Color.md)</td>
<td>Linearly interpolates between colors a and b by t</td>
</tr>
<tr>
<td>Gradient(a : [Color](../objects/Color.md),b : [Color](../objects/Color.md),t : float)</td>
<td>[Color](../objects/Color.md)</td>
<td>Creates a gradient color from two colors</td>
</tr>
</tbody>
</table>
