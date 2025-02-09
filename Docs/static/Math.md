# Math
Inherits from object
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|PI|float|False|The value of PI|
|Infinity|float|False|The value of Infinity|
|NegativeInfinity|float|False|The value of Negative Infinity|
|Rad2DegConstant|float|False|The value of Rad2Deg constant|
|Deg2RadConstant|float|False|The value of Deg2Rad constant|
|Epsilon|float|False|The value of Epsilon|
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
<td>Clamp(value : Object,min : Object,max : Object)</td>
<td>Object</td>
<td>Clamp a value between a minimum and maximum value</td>
</tr>
<tr>
<td>Max(a : Object,b : Object)</td>
<td>Object</td>
<td>Get the maximum of two values</td>
</tr>
<tr>
<td>Min(a : Object,b : Object)</td>
<td>Object</td>
<td>Get the minimum of two values</td>
</tr>
<tr>
<td>Pow(a : Object,b : Object)</td>
<td>Object</td>
<td>Raise a value to the power of another value</td>
</tr>
<tr>
<td>Abs(value : Object)</td>
<td>Object</td>
<td>Get the absolute value of a number</td>
</tr>
<tr>
<td>Sqrt(value : Object)</td>
<td>Object</td>
<td>Get the square root of a number</td>
</tr>
<tr>
<td>Mod(a : Object,b : Object)</td>
<td>Object</td>
<td>Get the remainder of a division operation</td>
</tr>
<tr>
<td>Sin(angle : Object)</td>
<td>Object</td>
<td>Get the sine of an angle in degrees</td>
</tr>
<tr>
<td>Cos(angle : Object)</td>
<td>Object</td>
<td>Get the cosine of an angle in degrees</td>
</tr>
<tr>
<td>Tan(angle : Object)</td>
<td>Object</td>
<td>Get the tangent of an angle in degrees</td>
</tr>
<tr>
<td>Asin(value : Object)</td>
<td>Object</td>
<td>Get the arcsine of a value in degrees</td>
</tr>
<tr>
<td>Acos(value : Object)</td>
<td>Object</td>
<td>Get the arccosine of a value in degrees</td>
</tr>
<tr>
<td>Atan(value : Object)</td>
<td>Object</td>
<td>Get the arctangent of a value in degrees</td>
</tr>
<tr>
<td>Atan2(a : Object,b : Object)</td>
<td>Object</td>
<td>Get the arctangent of a value in degrees</td>
</tr>
<tr>
<td>Ceil(value : Object)</td>
<td>Object</td>
<td>Get the smallest integer greater than or equal to a value</td>
</tr>
<tr>
<td>Floor(value : Object)</td>
<td>Object</td>
<td>Get the largest integer less than or equal to a value</td>
</tr>
<tr>
<td>Round(value : Object)</td>
<td>Object</td>
<td>Round a value to the nearest integer</td>
</tr>
<tr>
<td>Deg2Rad(angle : Object)</td>
<td>Object</td>
<td>Convert an angle from degrees to radians</td>
</tr>
<tr>
<td>Rad2Deg(angle : Object)</td>
<td>Object</td>
<td>Convert an angle from radians to degrees</td>
</tr>
<tr>
<td>Lerp(a : Object,b : Object,t : Object)</td>
<td>Object</td>
<td>Linearly interpolate between two values</td>
</tr>
<tr>
<td>LerpUnclamped(a : Object,b : Object,t : Object)</td>
<td>Object</td>
<td>Linearly interpolate between two values without clamping</td>
</tr>
<tr>
<td>Sign(value : Object)</td>
<td>Object</td>
<td>Get the sign of a value</td>
</tr>
<tr>
<td>InverseLerp(a : Object,b : Object,value : Object)</td>
<td>Object</td>
<td>Get the inverse lerp of two values</td>
</tr>
<tr>
<td>LerpAngle(a : Object,b : Object,t : Object)</td>
<td>Object</td>
<td>Linearly interpolate between two angles</td>
</tr>
<tr>
<td>Log(value : Object)</td>
<td>Object</td>
<td>Get the natural logarithm of a value</td>
</tr>
<tr>
<td>MoveTowards(current : Object,target : Object,maxDelta : Object)</td>
<td>Object</td>
<td>Move a value towards a target value</td>
</tr>
<tr>
<td>MoveTowardsAngle(current : Object,target : Object,maxDelta : Object)</td>
<td>Object</td>
<td>Move an angle towards a target angle</td>
</tr>
<tr>
<td>PingPong(t : Object,length : Object)</td>
<td>Object</td>
<td>Get the ping pong value of a time value</td>
</tr>
<tr>
<td>SmoothDamp(current : Object,target : Object,currentVelocity : Object,smoothTime : Object,maxSpeed : Object,deltaTime : Object)</td>
<td>Object</td>
<td>Smoothly damp a value towards a target value</td>
</tr>
<tr>
<td>Exp(value : Object)</td>
<td>Object</td>
<td>Get the exponential value of a number</td>
</tr>
<tr>
<td>SmoothDampAngle(current : Object,target : Object,currentVelocity : Object,smoothTime : Object,maxSpeed : Object,deltaTime : Object)</td>
<td>Object</td>
<td>Smoothly damp an angle towards a target angle</td>
</tr>
<tr>
<td>SmoothStep(a : Object,b : Object,t : Object)</td>
<td>Object</td>
<td>Smoothly step between two values</td>
</tr>
<tr>
<td>BitwiseAnd(a : Object,b : Object)</td>
<td>Object</td>
<td>Perform a bitwise AND operation</td>
</tr>
<tr>
<td>BitwiseOr(a : Object,b : Object)</td>
<td>Object</td>
<td>Perform a bitwise OR operation</td>
</tr>
<tr>
<td>BitwiseXor(a : Object,b : Object)</td>
<td>Object</td>
<td>Perform a bitwise XOR operation</td>
</tr>
<tr>
<td>BitwiseNot(value : Object)</td>
<td>Object</td>
<td>Perform a bitwise NOT operation</td>
</tr>
<tr>
<td>BitwiseLeftShift(value : Object,shift : Object)</td>
<td>Object</td>
<td>Shift bits to the left</td>
</tr>
<tr>
<td>BitwiseRightShift(value : Object,shift : Object)</td>
<td>Object</td>
<td>Shift bits to the right</td>
</tr>
</tbody>
</table>
