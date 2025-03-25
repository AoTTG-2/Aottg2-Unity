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
#### function <mark style="color:yellow;">Clamp</mark>(value: <mark style="color:blue;">Object</mark>, min: <mark style="color:blue;">Object</mark>, max: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Clamp a value between a minimum and maximum value

#### function <mark style="color:yellow;">Max</mark>(a: <mark style="color:blue;">Object</mark>, b: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the maximum of two values

#### function <mark style="color:yellow;">Min</mark>(a: <mark style="color:blue;">Object</mark>, b: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the minimum of two values

#### function <mark style="color:yellow;">Pow</mark>(a: <mark style="color:blue;">Object</mark>, b: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Raise a value to the power of another value

#### function <mark style="color:yellow;">Abs</mark>(value: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the absolute value of a number

#### function <mark style="color:yellow;">Sqrt</mark>(value: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the square root of a number

#### function <mark style="color:yellow;">Mod</mark>(a: <mark style="color:blue;">Object</mark>, b: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the remainder of a division operation

#### function <mark style="color:yellow;">Sin</mark>(angle: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the sine of an angle in degrees

#### function <mark style="color:yellow;">Cos</mark>(angle: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the cosine of an angle in degrees

#### function <mark style="color:yellow;">Tan</mark>(angle: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the tangent of an angle in degrees

#### function <mark style="color:yellow;">Asin</mark>(value: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the arcsine of a value in degrees

#### function <mark style="color:yellow;">Acos</mark>(value: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the arccosine of a value in degrees

#### function <mark style="color:yellow;">Atan</mark>(value: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the arctangent of a value in degrees

#### function <mark style="color:yellow;">Atan2</mark>(a: <mark style="color:blue;">Object</mark>, b: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the arctangent of a value in degrees

#### function <mark style="color:yellow;">Ceil</mark>(value: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the smallest integer greater than or equal to a value

#### function <mark style="color:yellow;">Floor</mark>(value: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the largest integer less than or equal to a value

#### function <mark style="color:yellow;">Round</mark>(value: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Round a value to the nearest integer

#### function <mark style="color:yellow;">Deg2Rad</mark>(angle: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Convert an angle from degrees to radians

#### function <mark style="color:yellow;">Rad2Deg</mark>(angle: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Convert an angle from radians to degrees

#### function <mark style="color:yellow;">Lerp</mark>(a: <mark style="color:blue;">Object</mark>, b: <mark style="color:blue;">Object</mark>, t: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Linearly interpolate between two values

#### function <mark style="color:yellow;">LerpUnclamped</mark>(a: <mark style="color:blue;">Object</mark>, b: <mark style="color:blue;">Object</mark>, t: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Linearly interpolate between two values without clamping

#### function <mark style="color:yellow;">Sign</mark>(value: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the sign of a value

#### function <mark style="color:yellow;">InverseLerp</mark>(a: <mark style="color:blue;">Object</mark>, b: <mark style="color:blue;">Object</mark>, value: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the inverse lerp of two values

#### function <mark style="color:yellow;">LerpAngle</mark>(a: <mark style="color:blue;">Object</mark>, b: <mark style="color:blue;">Object</mark>, t: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Linearly interpolate between two angles

#### function <mark style="color:yellow;">Log</mark>(value: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the natural logarithm of a value

#### function <mark style="color:yellow;">MoveTowards</mark>(current: <mark style="color:blue;">Object</mark>, target: <mark style="color:blue;">Object</mark>, maxDelta: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Move a value towards a target value

#### function <mark style="color:yellow;">MoveTowardsAngle</mark>(current: <mark style="color:blue;">Object</mark>, target: <mark style="color:blue;">Object</mark>, maxDelta: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Move an angle towards a target angle

#### function <mark style="color:yellow;">PingPong</mark>(t: <mark style="color:blue;">Object</mark>, length: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the ping pong value of a time value

#### function <mark style="color:yellow;">SmoothDamp</mark>(current: <mark style="color:blue;">Object</mark>, target: <mark style="color:blue;">Object</mark>, currentVelocity: <mark style="color:blue;">Object</mark>, smoothTime: <mark style="color:blue;">Object</mark>, maxSpeed: <mark style="color:blue;">Object</mark>, deltaTime: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Smoothly damp a value towards a target value

#### function <mark style="color:yellow;">Exp</mark>(value: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Get the exponential value of a number

#### function <mark style="color:yellow;">SmoothDampAngle</mark>(current: <mark style="color:blue;">Object</mark>, target: <mark style="color:blue;">Object</mark>, currentVelocity: <mark style="color:blue;">Object</mark>, smoothTime: <mark style="color:blue;">Object</mark>, maxSpeed: <mark style="color:blue;">Object</mark>, deltaTime: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Smoothly damp an angle towards a target angle

#### function <mark style="color:yellow;">SmoothStep</mark>(a: <mark style="color:blue;">Object</mark>, b: <mark style="color:blue;">Object</mark>, t: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Smoothly step between two values

#### function <mark style="color:yellow;">BitwiseAnd</mark>(a: <mark style="color:blue;">Object</mark>, b: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Perform a bitwise AND operation

#### function <mark style="color:yellow;">BitwiseOr</mark>(a: <mark style="color:blue;">Object</mark>, b: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Perform a bitwise OR operation

#### function <mark style="color:yellow;">BitwiseXor</mark>(a: <mark style="color:blue;">Object</mark>, b: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Perform a bitwise XOR operation

#### function <mark style="color:yellow;">BitwiseNot</mark>(value: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Perform a bitwise NOT operation

#### function <mark style="color:yellow;">BitwiseLeftShift</mark>(value: <mark style="color:blue;">Object</mark>, shift: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Shift bits to the left

#### function <mark style="color:yellow;">BitwiseRightShift</mark>(value: <mark style="color:blue;">Object</mark>, shift: <mark style="color:blue;">Object</mark>) -> <mark style="color:blue;">Object</mark>
> Shift bits to the right


---

