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
<pre class="language-typescript"><code class="lang-typescript">function Clamp(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>, min: <a data-footnote-ref href="#user-content-fn-37">Object</a>, max: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Clamp a value between a minimum and maximum value
> 
<pre class="language-typescript"><code class="lang-typescript">function Max(a: <a data-footnote-ref href="#user-content-fn-37">Object</a>, b: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the maximum of two values
> 
<pre class="language-typescript"><code class="lang-typescript">function Min(a: <a data-footnote-ref href="#user-content-fn-37">Object</a>, b: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the minimum of two values
> 
<pre class="language-typescript"><code class="lang-typescript">function Pow(a: <a data-footnote-ref href="#user-content-fn-37">Object</a>, b: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Raise a value to the power of another value
> 
<pre class="language-typescript"><code class="lang-typescript">function Abs(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the absolute value of a number
> 
<pre class="language-typescript"><code class="lang-typescript">function Sqrt(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the square root of a number
> 
<pre class="language-typescript"><code class="lang-typescript">function Mod(a: <a data-footnote-ref href="#user-content-fn-37">Object</a>, b: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the remainder of a division operation
> 
<pre class="language-typescript"><code class="lang-typescript">function Sin(angle: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the sine of an angle in degrees
> 
<pre class="language-typescript"><code class="lang-typescript">function Cos(angle: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the cosine of an angle in degrees
> 
<pre class="language-typescript"><code class="lang-typescript">function Tan(angle: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the tangent of an angle in degrees
> 
<pre class="language-typescript"><code class="lang-typescript">function Asin(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the arcsine of a value in degrees
> 
<pre class="language-typescript"><code class="lang-typescript">function Acos(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the arccosine of a value in degrees
> 
<pre class="language-typescript"><code class="lang-typescript">function Atan(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the arctangent of a value in degrees
> 
<pre class="language-typescript"><code class="lang-typescript">function Atan2(a: <a data-footnote-ref href="#user-content-fn-37">Object</a>, b: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the arctangent of a value in degrees
> 
<pre class="language-typescript"><code class="lang-typescript">function Ceil(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the smallest integer greater than or equal to a value
> 
<pre class="language-typescript"><code class="lang-typescript">function Floor(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the largest integer less than or equal to a value
> 
<pre class="language-typescript"><code class="lang-typescript">function Round(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Round a value to the nearest integer
> 
<pre class="language-typescript"><code class="lang-typescript">function Deg2Rad(angle: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Convert an angle from degrees to radians
> 
<pre class="language-typescript"><code class="lang-typescript">function Rad2Deg(angle: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Convert an angle from radians to degrees
> 
<pre class="language-typescript"><code class="lang-typescript">function Lerp(a: <a data-footnote-ref href="#user-content-fn-37">Object</a>, b: <a data-footnote-ref href="#user-content-fn-37">Object</a>, t: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Linearly interpolate between two values
> 
<pre class="language-typescript"><code class="lang-typescript">function LerpUnclamped(a: <a data-footnote-ref href="#user-content-fn-37">Object</a>, b: <a data-footnote-ref href="#user-content-fn-37">Object</a>, t: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Linearly interpolate between two values without clamping
> 
<pre class="language-typescript"><code class="lang-typescript">function Sign(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the sign of a value
> 
<pre class="language-typescript"><code class="lang-typescript">function InverseLerp(a: <a data-footnote-ref href="#user-content-fn-37">Object</a>, b: <a data-footnote-ref href="#user-content-fn-37">Object</a>, value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the inverse lerp of two values
> 
<pre class="language-typescript"><code class="lang-typescript">function LerpAngle(a: <a data-footnote-ref href="#user-content-fn-37">Object</a>, b: <a data-footnote-ref href="#user-content-fn-37">Object</a>, t: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Linearly interpolate between two angles
> 
<pre class="language-typescript"><code class="lang-typescript">function Log(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the natural logarithm of a value
> 
<pre class="language-typescript"><code class="lang-typescript">function MoveTowards(current: <a data-footnote-ref href="#user-content-fn-37">Object</a>, target: <a data-footnote-ref href="#user-content-fn-37">Object</a>, maxDelta: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Move a value towards a target value
> 
<pre class="language-typescript"><code class="lang-typescript">function MoveTowardsAngle(current: <a data-footnote-ref href="#user-content-fn-37">Object</a>, target: <a data-footnote-ref href="#user-content-fn-37">Object</a>, maxDelta: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Move an angle towards a target angle
> 
<pre class="language-typescript"><code class="lang-typescript">function PingPong(t: <a data-footnote-ref href="#user-content-fn-37">Object</a>, length: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the ping pong value of a time value
> 
<pre class="language-typescript"><code class="lang-typescript">function SmoothDamp(current: <a data-footnote-ref href="#user-content-fn-37">Object</a>, target: <a data-footnote-ref href="#user-content-fn-37">Object</a>, currentVelocity: <a data-footnote-ref href="#user-content-fn-37">Object</a>, smoothTime: <a data-footnote-ref href="#user-content-fn-37">Object</a>, maxSpeed: <a data-footnote-ref href="#user-content-fn-37">Object</a>, deltaTime: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Smoothly damp a value towards a target value
> 
<pre class="language-typescript"><code class="lang-typescript">function Exp(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the exponential value of a number
> 
<pre class="language-typescript"><code class="lang-typescript">function SmoothDampAngle(current: <a data-footnote-ref href="#user-content-fn-37">Object</a>, target: <a data-footnote-ref href="#user-content-fn-37">Object</a>, currentVelocity: <a data-footnote-ref href="#user-content-fn-37">Object</a>, smoothTime: <a data-footnote-ref href="#user-content-fn-37">Object</a>, maxSpeed: <a data-footnote-ref href="#user-content-fn-37">Object</a>, deltaTime: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Smoothly damp an angle towards a target angle
> 
<pre class="language-typescript"><code class="lang-typescript">function SmoothStep(a: <a data-footnote-ref href="#user-content-fn-37">Object</a>, b: <a data-footnote-ref href="#user-content-fn-37">Object</a>, t: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Smoothly step between two values
> 
<pre class="language-typescript"><code class="lang-typescript">function BitwiseAnd(a: <a data-footnote-ref href="#user-content-fn-37">Object</a>, b: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Perform a bitwise AND operation
> 
<pre class="language-typescript"><code class="lang-typescript">function BitwiseOr(a: <a data-footnote-ref href="#user-content-fn-37">Object</a>, b: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Perform a bitwise OR operation
> 
<pre class="language-typescript"><code class="lang-typescript">function BitwiseXor(a: <a data-footnote-ref href="#user-content-fn-37">Object</a>, b: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Perform a bitwise XOR operation
> 
<pre class="language-typescript"><code class="lang-typescript">function BitwiseNot(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Perform a bitwise NOT operation
> 
<pre class="language-typescript"><code class="lang-typescript">function BitwiseLeftShift(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>, shift: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Shift bits to the left
> 
<pre class="language-typescript"><code class="lang-typescript">function BitwiseRightShift(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>, shift: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
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
[^15]: [Map](../static/Map.md)
[^16]: [MapObject](../objects/MapObject.md)
[^17]: [MapTargetable](../objects/MapTargetable.md)
[^18]: [Math](../static/Math.md)
[^19]: [Network](../static/Network.md)
[^20]: [NetworkView](../objects/NetworkView.md)
[^21]: [PersistentData](../static/PersistentData.md)
[^22]: [Physics](../static/Physics.md)
[^23]: [Player](../objects/Player.md)
[^24]: [Quaternion](../objects/Quaternion.md)
[^25]: [Random](../objects/Random.md)
[^26]: [Range](../objects/Range.md)
[^27]: [RoomData](../static/RoomData.md)
[^28]: [Set](../objects/Set.md)
[^29]: [Shifter](../objects/Shifter.md)
[^30]: [String](../static/String.md)
[^31]: [Time](../static/Time.md)
[^32]: [Titan](../objects/Titan.md)
[^33]: [Transform](../objects/Transform.md)
[^34]: [UI](../static/UI.md)
[^35]: [Vector2](../objects/Vector2.md)
[^36]: [Vector3](../objects/Vector3.md)
[^37]: [Object](../objects/Object.md)
[^38]: [Component](../objects/Component.md)
