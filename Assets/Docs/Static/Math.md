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
|Function|Returns|Description|
|---|---|---|
|Clamp(value : Object, min : Object, max : Object)|Object|Clamp a value between a minimum and maximum value|
|Max(a : Object, b : Object)|Object|Get the maximum of two values|
|Min(a : Object, b : Object)|Object|Get the minimum of two values|
|Pow(a : Object, b : Object)|Object|Raise a value to the power of another value|
|Abs(value : Object)|Object|Get the absolute value of a number|
|Sqrt(value : Object)|Object|Get the square root of a number|
|Mod(a : Object, b : Object)|Object|Get the remainder of a division operation|
|Sin(angle : Object)|Object|Get the sine of an angle in degrees|
|Cos(angle : Object)|Object|Get the cosine of an angle in degrees|
|Tan(angle : Object)|Object|Get the tangent of an angle in degrees|
|Asin(value : Object)|Object|Get the arcsine of a value in degrees|
|Acos(value : Object)|Object|Get the arccosine of a value in degrees|
|Atan(value : Object)|Object|Get the arctangent of a value in degrees|
|Atan2(a : Object, b : Object)|Object|Get the arctangent of a value in degrees|
|Ceil(value : Object)|Object|Get the smallest integer greater than or equal to a value|
|Floor(value : Object)|Object|Get the largest integer less than or equal to a value|
|Round(value : Object)|Object|Round a value to the nearest integer|
|Deg2Rad(angle : Object)|Object|Convert an angle from degrees to radians|
|Rad2Deg(angle : Object)|Object|Convert an angle from radians to degrees|
|Lerp(a : Object, b : Object, t : Object)|Object|Linearly interpolate between two values|
|LerpUnclamped(a : Object, b : Object, t : Object)|Object|Linearly interpolate between two values without clamping|
|Sign(value : Object)|Object|Get the sign of a value|
|InverseLerp(a : Object, b : Object, value : Object)|Object|Get the inverse lerp of two values|
|LerpAngle(a : Object, b : Object, t : Object)|Object|Linearly interpolate between two angles|
|Log(value : Object)|Object|Get the natural logarithm of a value|
|MoveTowards(current : Object, target : Object, maxDelta : Object)|Object|Move a value towards a target value|
|MoveTowardsAngle(current : Object, target : Object, maxDelta : Object)|Object|Move an angle towards a target angle|
|PingPong(t : Object, length : Object)|Object|Get the ping pong value of a time value|
|SmoothDamp(current : Object, target : Object, currentVelocity : Object, smoothTime : Object, maxSpeed : Object, deltaTime : Object)|Object|Smoothly damp a value towards a target value|
|Exp(value : Object)|Object|Get the exponential value of a number|
|SmoothDampAngle(current : Object, target : Object, currentVelocity : Object, smoothTime : Object, maxSpeed : Object, deltaTime : Object)|Object|Smoothly damp an angle towards a target angle|
|SmoothStep(a : Object, b : Object, t : Object)|Object|Smoothly step between two values|
|BitwiseAnd(a : Object, b : Object)|Object|Perform a bitwise AND operation|
|BitwiseOr(a : Object, b : Object)|Object|Perform a bitwise OR operation|
|BitwiseXor(a : Object, b : Object)|Object|Perform a bitwise XOR operation|
|BitwiseNot(value : Object)|Object|Perform a bitwise NOT operation|
|BitwiseLeftShift(value : Object, shift : Object)|Object|Shift bits to the left|
|BitwiseRightShift(value : Object, shift : Object)|Object|Shift bits to the right|
