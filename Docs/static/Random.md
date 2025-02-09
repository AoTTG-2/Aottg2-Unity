# Random
Inherits from object
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
<td>RandomInt(min : int,max : int)</td>
<td>int</td>
<td>Generates a random integer between the specified range.</td>
</tr>
<tr>
<td>RandomFloat(min : float,max : float)</td>
<td>float</td>
<td>Generates a random float between the specified range.</td>
</tr>
<tr>
<td>RandomBool()</td>
<td>bool</td>
<td>Returns random boolean.</td>
</tr>
<tr>
<td>RandomVector3(a : [Vector3](../objects/Vector3.md),b : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Generates a random Vector3 between the specified ranges.</td>
</tr>
<tr>
<td>RandomDirection(flat : bool = False)</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Generates a random normalized direction vector. If flat is true, the y component will be zero.</td>
</tr>
<tr>
<td>RandomSign()</td>
<td>int</td>
<td>Generates a random sign, either 1 or -1.</td>
</tr>
<tr>
<td>PerlinNoise(x : float,y : float)</td>
<td>float</td>
<td>Returns a point sampled from generated 2d perlin noise. (see Unity Mathf.PerlinNoise for more information)</td>
</tr>
</tbody>
</table>
