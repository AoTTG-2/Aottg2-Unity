# Math
Inherits from [Object](../objects/Object.md)

Math functions. Note that parameter types can be either int or float unless otherwise specified.
Functions may return int or float depending on the parameter types given.

### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|PI|float|True|The value of PI|
|Infinity|float|True|The value of Infinity|
|NegativeInfinity|float|True|The value of Negative Infinity|
|Rad2DegConstant|float|True|The value of Rad2Deg constant|
|Deg2RadConstant|float|True|The value of Deg2Rad constant|
|Epsilon|float|True|The value of Epsilon|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Clamp(value: <a data-footnote-ref href="#user-content-fn-38">Object</a>, min: <a data-footnote-ref href="#user-content-fn-38">Object</a>, max: <a data-footnote-ref href="#user-content-fn-38">Object</a>) -> <a data-footnote-ref href="#user-content-fn-38">Object</a></code></pre>
> Clamp a value between a minimum and maximum value
> 
> **Parameters**:
> - `value`: The value to clamp. Can be int or float
> - `min`: The minimum value. Can be int or float
> - `max`: The maximum value. Can be int or float
> 
> **Returns**: The clamped value. Will be the same type as the inputs
<pre class="language-typescript"><code class="lang-typescript">function Max(a: <a data-footnote-ref href="#user-content-fn-38">Object</a>, b: <a data-footnote-ref href="#user-content-fn-38">Object</a>) -> <a data-footnote-ref href="#user-content-fn-38">Object</a></code></pre>
> Get the maximum of two values
> 
> **Parameters**:
> - `a`: The first value. Can be int or float
> - `b`: The second value. Can be int or float
> 
> **Returns**: The maximum of the two values. Will be the same type as the inputs
<pre class="language-typescript"><code class="lang-typescript">function Min(a: <a data-footnote-ref href="#user-content-fn-38">Object</a>, b: <a data-footnote-ref href="#user-content-fn-38">Object</a>) -> <a data-footnote-ref href="#user-content-fn-38">Object</a></code></pre>
> Get the minimum of two values
> 
> **Parameters**:
> - `a`: The first value. Can be int or float
> - `b`: The second value. Can be int or float
> 
> **Returns**: The minimum of the two values. Will be the same type as the inputs
<pre class="language-typescript"><code class="lang-typescript">function Pow(a: float, b: float) -> float</code></pre>
> Raise a value to the power of another value
> 
<pre class="language-typescript"><code class="lang-typescript">function Abs(value: <a data-footnote-ref href="#user-content-fn-38">Object</a>) -> <a data-footnote-ref href="#user-content-fn-38">Object</a></code></pre>
> Get the absolute value of a number
> 
> **Parameters**:
> - `value`: The number. Can be int or float
> 
> **Returns**: The absolute value. Will be the same type as the input
<pre class="language-typescript"><code class="lang-typescript">function Sqrt(value: float) -> float</code></pre>
> Get the square root of a number
> 
<pre class="language-typescript"><code class="lang-typescript">function Mod(a: int, b: int) -> int</code></pre>
> Get the remainder of a division operation
> 
<pre class="language-typescript"><code class="lang-typescript">function Sin(angle: float) -> float</code></pre>
> Get the sine of an angle
> 
> **Parameters**:
> - `angle`: The angle in degrees
> 
> **Returns**: Value between -1 and 1
<pre class="language-typescript"><code class="lang-typescript">function Cos(angle: float) -> float</code></pre>
> Get the cosine of an angle
> 
> **Parameters**:
> - `angle`: The angle in degrees
> 
> **Returns**: Value between -1 and 1
<pre class="language-typescript"><code class="lang-typescript">function Tan(angle: float) -> float</code></pre>
> Get the tangent of an angle in radians
> 
> **Parameters**:
> - `angle`: The angle in degrees
> 
<pre class="language-typescript"><code class="lang-typescript">function Asin(value: float) -> float</code></pre>
> Get the arcsine of a value in degrees
> 
<pre class="language-typescript"><code class="lang-typescript">function Acos(value: float) -> float</code></pre>
> Get the arccosine of a value in degrees
> 
<pre class="language-typescript"><code class="lang-typescript">function Atan(value: float) -> float</code></pre>
> Get the arctangent of a value in degrees
> 
<pre class="language-typescript"><code class="lang-typescript">function Atan2(a: float, b: float) -> float</code></pre>
> Get the arctangent of a value in degrees
> 
<pre class="language-typescript"><code class="lang-typescript">function Ceil(value: float) -> int</code></pre>
> Get the smallest integer greater than or equal to a value
> 
<pre class="language-typescript"><code class="lang-typescript">function Floor(value: float) -> int</code></pre>
> Get the largest integer less than or equal to a value
> 
<pre class="language-typescript"><code class="lang-typescript">function Round(value: float) -> int</code></pre>
> Round a value to the nearest integer
> 
<pre class="language-typescript"><code class="lang-typescript">function Deg2Rad(angle: float) -> float</code></pre>
> Convert an angle from degrees to radians
> 
<pre class="language-typescript"><code class="lang-typescript">function Rad2Deg(angle: float) -> float</code></pre>
> Convert an angle from radians to degrees
> 
<pre class="language-typescript"><code class="lang-typescript">function Lerp(a: float, b: float, t: float) -> float</code></pre>
> Linearly interpolate between two values
> 
<pre class="language-typescript"><code class="lang-typescript">function LerpUnclamped(a: float, b: float, t: float) -> float</code></pre>
> Linearly interpolate between two values without clamping
> 
<pre class="language-typescript"><code class="lang-typescript">function Sign(value: float) -> float</code></pre>
> Get the sign of a value
> 
<pre class="language-typescript"><code class="lang-typescript">function InverseLerp(a: float, b: float, value: float) -> float</code></pre>
> Get the inverse lerp of two values
> 
<pre class="language-typescript"><code class="lang-typescript">function LerpAngle(a: float, b: float, t: float) -> float</code></pre>
> Linearly interpolate between two angles
> 
<pre class="language-typescript"><code class="lang-typescript">function Log(value: float) -> float</code></pre>
> Get the natural logarithm of a value
> 
<pre class="language-typescript"><code class="lang-typescript">function MoveTowards(current: float, target: float, maxDelta: float) -> float</code></pre>
> Move a value towards a target value
> 
<pre class="language-typescript"><code class="lang-typescript">function MoveTowardsAngle(current: float, target: float, maxDelta: float) -> float</code></pre>
> Move an angle towards a target angle
> 
<pre class="language-typescript"><code class="lang-typescript">function PingPong(t: float, length: float) -> float</code></pre>
> Get the ping pong value of a time value
> 
<pre class="language-typescript"><code class="lang-typescript">function Exp(value: float) -> float</code></pre>
> Get the exponential value of a number
> 
<pre class="language-typescript"><code class="lang-typescript">function SmoothDamp(current: float, target: float, currentVelocity: float, smoothTime: float, maxSpeed: float, deltaTime: float) -> float</code></pre>
> Smoothly damp a value towards a target value
> 
<pre class="language-typescript"><code class="lang-typescript">function SmoothDampAngle(current: float, target: float, currentVelocity: float, smoothTime: float, maxSpeed: float, deltaTime: float) -> float</code></pre>
> Smoothly damp an angle towards a target angle
> 
<pre class="language-typescript"><code class="lang-typescript">function SmoothStep(a: float, b: float, t: float) -> float</code></pre>
> Smoothly step between two values
> 
<pre class="language-typescript"><code class="lang-typescript">function BitwiseAnd(a: int, b: int) -> int</code></pre>
> Perform a bitwise AND operation
> 
<pre class="language-typescript"><code class="lang-typescript">function BitwiseOr(a: int, b: int) -> int</code></pre>
> Perform a bitwise OR operation
> 
<pre class="language-typescript"><code class="lang-typescript">function BitwiseXor(a: int, b: int) -> int</code></pre>
> Perform a bitwise XOR operation
> 
<pre class="language-typescript"><code class="lang-typescript">function BitwiseNot(value: int) -> int</code></pre>
> Perform a bitwise NOT operation
> 
<pre class="language-typescript"><code class="lang-typescript">function BitwiseLeftShift(value: int, shift: int) -> int</code></pre>
> Shift bits to the left
> 
<pre class="language-typescript"><code class="lang-typescript">function BitwiseRightShift(value: int, shift: int) -> int</code></pre>
> Shift bits to the right
> 

[^0]: [Camera](../static/Camera.md)
[^1]: [Character](../objects/Character.md)
[^2]: [Collider](../objects/Collider.md)
[^3]: [Collision](../objects/Collision.md)
[^4]: [Color](../objects/Color.md)
[^5]: [Convert](../static/Convert.md)
[^6]: [Cutscene](../static/Cutscene.md)
[^7]: [Dict](../objects/Dict.md)
[^8]: [Game](../static/Game.md)
[^9]: [Human](../objects/Human.md)
[^10]: [Input](../static/Input.md)
[^11]: [Json](../static/Json.md)
[^12]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^13]: [LineRenderer](../objects/LineRenderer.md)
[^14]: [List](../objects/List.md)
[^15]: [Locale](../static/Locale.md)
[^16]: [Map](../static/Map.md)
[^17]: [MapObject](../objects/MapObject.md)
[^18]: [MapTargetable](../objects/MapTargetable.md)
[^19]: [Math](../static/Math.md)
[^20]: [Network](../static/Network.md)
[^21]: [NetworkView](../objects/NetworkView.md)
[^22]: [PersistentData](../static/PersistentData.md)
[^23]: [Physics](../static/Physics.md)
[^24]: [Player](../objects/Player.md)
[^25]: [Quaternion](../objects/Quaternion.md)
[^26]: [Random](../objects/Random.md)
[^27]: [Range](../objects/Range.md)
[^28]: [RoomData](../static/RoomData.md)
[^29]: [Set](../objects/Set.md)
[^30]: [Shifter](../objects/Shifter.md)
[^31]: [String](../static/String.md)
[^32]: [Time](../static/Time.md)
[^33]: [Titan](../objects/Titan.md)
[^34]: [Transform](../objects/Transform.md)
[^35]: [UI](../static/UI.md)
[^36]: [Vector2](../objects/Vector2.md)
[^37]: [Vector3](../objects/Vector3.md)
[^38]: [Object](../objects/Object.md)
[^39]: [Component](../objects/Component.md)
