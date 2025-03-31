# Math
Inherits from object
## Initialization
<span style="color:red;">This class is abstract and cannot be instantiated.</span>
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|PI|float|True|The value of PI|
|Infinity|float|True|The value of Infinity|
|NegativeInfinity|float|True|The value of Negative Infinity|
|Rad2DegConstant|float|True|The value of Rad2Deg constant|
|Deg2RadConstant|float|True|The value of Deg2Rad constant|
|Epsilon|float|True|The value of Epsilon|
## Methods
#### function <span style="color:yellow;">Clamp</span>(value: <span style="color:blue;">Object</span>, min: <span style="color:blue;">Object</span>, max: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Clamp a value between a minimum and maximum value

#### function <span style="color:yellow;">Max</span>(a: <span style="color:blue;">Object</span>, b: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the maximum of two values

#### function <span style="color:yellow;">Min</span>(a: <span style="color:blue;">Object</span>, b: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the minimum of two values

#### function <span style="color:yellow;">Pow</span>(a: <span style="color:blue;">Object</span>, b: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Raise a value to the power of another value

#### function <span style="color:yellow;">Abs</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the absolute value of a number

#### function <span style="color:yellow;">Sqrt</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the square root of a number

#### function <span style="color:yellow;">Mod</span>(a: <span style="color:blue;">Object</span>, b: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the remainder of a division operation

#### function <span style="color:yellow;">Sin</span>(angle: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the sine of an angle in degrees

#### function <span style="color:yellow;">Cos</span>(angle: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the cosine of an angle in degrees

#### function <span style="color:yellow;">Tan</span>(angle: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the tangent of an angle in degrees

#### function <span style="color:yellow;">Asin</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the arcsine of a value in degrees

#### function <span style="color:yellow;">Acos</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the arccosine of a value in degrees

#### function <span style="color:yellow;">Atan</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the arctangent of a value in degrees

#### function <span style="color:yellow;">Atan2</span>(a: <span style="color:blue;">Object</span>, b: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the arctangent of a value in degrees

#### function <span style="color:yellow;">Ceil</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the smallest integer greater than or equal to a value

#### function <span style="color:yellow;">Floor</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the largest integer less than or equal to a value

#### function <span style="color:yellow;">Round</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Round a value to the nearest integer

#### function <span style="color:yellow;">Deg2Rad</span>(angle: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Convert an angle from degrees to radians

#### function <span style="color:yellow;">Rad2Deg</span>(angle: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Convert an angle from radians to degrees

#### function <span style="color:yellow;">Lerp</span>(a: <span style="color:blue;">Object</span>, b: <span style="color:blue;">Object</span>, t: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Linearly interpolate between two values

#### function <span style="color:yellow;">LerpUnclamped</span>(a: <span style="color:blue;">Object</span>, b: <span style="color:blue;">Object</span>, t: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Linearly interpolate between two values without clamping

#### function <span style="color:yellow;">Sign</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the sign of a value

#### function <span style="color:yellow;">InverseLerp</span>(a: <span style="color:blue;">Object</span>, b: <span style="color:blue;">Object</span>, value: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the inverse lerp of two values

#### function <span style="color:yellow;">LerpAngle</span>(a: <span style="color:blue;">Object</span>, b: <span style="color:blue;">Object</span>, t: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Linearly interpolate between two angles

#### function <span style="color:yellow;">Log</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the natural logarithm of a value

#### function <span style="color:yellow;">MoveTowards</span>(current: <span style="color:blue;">Object</span>, target: <span style="color:blue;">Object</span>, maxDelta: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Move a value towards a target value

#### function <span style="color:yellow;">MoveTowardsAngle</span>(current: <span style="color:blue;">Object</span>, target: <span style="color:blue;">Object</span>, maxDelta: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Move an angle towards a target angle

#### function <span style="color:yellow;">PingPong</span>(t: <span style="color:blue;">Object</span>, length: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the ping pong value of a time value

#### function <span style="color:yellow;">SmoothDamp</span>(current: <span style="color:blue;">Object</span>, target: <span style="color:blue;">Object</span>, currentVelocity: <span style="color:blue;">Object</span>, smoothTime: <span style="color:blue;">Object</span>, maxSpeed: <span style="color:blue;">Object</span>, deltaTime: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Smoothly damp a value towards a target value

#### function <span style="color:yellow;">Exp</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Get the exponential value of a number

#### function <span style="color:yellow;">SmoothDampAngle</span>(current: <span style="color:blue;">Object</span>, target: <span style="color:blue;">Object</span>, currentVelocity: <span style="color:blue;">Object</span>, smoothTime: <span style="color:blue;">Object</span>, maxSpeed: <span style="color:blue;">Object</span>, deltaTime: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Smoothly damp an angle towards a target angle

#### function <span style="color:yellow;">SmoothStep</span>(a: <span style="color:blue;">Object</span>, b: <span style="color:blue;">Object</span>, t: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Smoothly step between two values

#### function <span style="color:yellow;">BitwiseAnd</span>(a: <span style="color:blue;">Object</span>, b: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Perform a bitwise AND operation

#### function <span style="color:yellow;">BitwiseOr</span>(a: <span style="color:blue;">Object</span>, b: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Perform a bitwise OR operation

#### function <span style="color:yellow;">BitwiseXor</span>(a: <span style="color:blue;">Object</span>, b: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Perform a bitwise XOR operation

#### function <span style="color:yellow;">BitwiseNot</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Perform a bitwise NOT operation

#### function <span style="color:yellow;">BitwiseLeftShift</span>(value: <span style="color:blue;">Object</span>, shift: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Shift bits to the left

#### function <span style="color:yellow;">BitwiseRightShift</span>(value: <span style="color:blue;">Object</span>, shift: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Shift bits to the right


---

