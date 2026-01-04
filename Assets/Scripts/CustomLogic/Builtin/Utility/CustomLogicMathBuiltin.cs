using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "Math", Abstract = true, Static = true)]
    partial class CustomLogicMathBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicMathBuiltin()
        {
        }

        /// <summary>
        /// The value of PI.
        /// </summary>
        [CLProperty]
        public static float PI => Mathf.PI;

        /// <summary>
        /// The value of Infinity.
        /// </summary>
        [CLProperty]
        public static float Infinity => Mathf.Infinity;

        /// <summary>
        /// The value of Negative Infinity.
        /// </summary>
        [CLProperty]
        public static float NegativeInfinity => Mathf.NegativeInfinity;

        /// <summary>
        /// The value of Rad2Deg constant.
        /// </summary>
        [CLProperty]
        public static float Rad2DegConstant => Mathf.Rad2Deg;

        /// <summary>
        /// The value of Deg2Rad constant.
        /// </summary>
        [CLProperty]
        public static float Deg2RadConstant => Mathf.Deg2Rad;

        /// <summary>
        /// The value of Epsilon.
        /// </summary>
        [CLProperty]
        public static float Epsilon => Mathf.Epsilon;

        /// <summary>
        /// Clamp a value between a minimum and maximum value.
        /// </summary>
        /// <param name="value">The value to clamp. Can be int or float.</param>
        /// <param name="min">The minimum value. Can be int or float.</param>
        /// <param name="max">The maximum value. Can be int or float.</param>
        /// <returns>The clamped value. Will be the same type as the inputs.</returns>
        [CLMethod(Static = true)]
        public object Clamp(object value, object min, object max)
        {
            if (value is int vInt && min is int minInt && max is int maxInt)
                return Mathf.Clamp(vInt, minInt, maxInt);

            float fValue = CustomLogicEvaluator.ConvertTo<float>(value);
            float fMin = CustomLogicEvaluator.ConvertTo<float>(min);
            float fMax = CustomLogicEvaluator.ConvertTo<float>(max);
            return Mathf.Clamp(fValue, fMin, fMax);
        }

        /// <summary>
        /// Get the maximum of two values.
        /// </summary>
        /// <param name="a">The first value. Can be int or float.</param>
        /// <param name="b">The second value. Can be int or float.</param>
        /// <returns>The maximum of the two values. Will be the same type as the inputs.</returns>
        [CLMethod(Static = true)]
        public object Max(object a, object b)
        {
            if (a is int aInt && b is int bInt)
                return Mathf.Max(aInt, bInt);

            float fA = CustomLogicEvaluator.ConvertTo<float>(a);
            float fB = CustomLogicEvaluator.ConvertTo<float>(b);
            return Mathf.Max(fA, fB);
        }

        /// <summary>
        /// Get the minimum of two values.
        /// </summary>
        /// <param name="a">The first value. Can be int or float.</param>
        /// <param name="b">The second value. Can be int or float.</param>
        /// <returns>The minimum of the two values. Will be the same type as the inputs.</returns>
        [CLMethod(Static = true)]
        public object Min(object a, object b)
        {
            if (a is int aInt && b is int bInt)
                return Mathf.Min(aInt, bInt);

            float fA = CustomLogicEvaluator.ConvertTo<float>(a);
            float fB = CustomLogicEvaluator.ConvertTo<float>(b);
            return Mathf.Min(fA, fB);
        }

        /// <summary>
        /// Raise a value to the power of another value.
        /// </summary>
        /// <param name="a">The base value.</param>
        /// <param name="b">The exponent value.</param>
        /// <returns>The result of raising a to the power of b.</returns>
        [CLMethod(Static = true)]
        public float Pow(float a, float b) => Mathf.Pow(a, b);

        /// <summary>
        /// Get the absolute value of a number.
        /// </summary>
        /// <param name="value">The number. Can be int or float.</param>
        /// <returns>The absolute value. Will be the same type as the input.</returns>
        [CLMethod(Static = true)]
        public object Abs(object value)
        {
            if (value is int vInt)
                return Mathf.Abs(vInt);

            float fValue = CustomLogicEvaluator.ConvertTo<float>(value);
            return Mathf.Abs(fValue);
        }

        /// <summary>
        /// Get the square root of a number.
        /// </summary>
        /// <param name="value">The value to get the square root of.</param>
        /// <returns>The square root of the value.</returns>
        [CLMethod(Static = true)]
        public float Sqrt(float value) => Mathf.Sqrt(value);

        /// <summary>
        /// Modulo for floats.
        /// </summary>
        /// <param name="value">The value to repeat.</param>
        /// <param name="max">The maximum value to repeat to.</param>
        /// <returns>The repeated value.</returns>
        [CLMethod(Static = true)]
        public object Repeat(object value, object max)
        {
            float fValue = value.UnboxToFloat();
            float fMax = max.UnboxToFloat();
            return Mathf.Repeat(fValue, fMax);
        }

        /// <summary>
        /// Get the remainder of a division operation.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The remainder of the division.</returns>
        [CLMethod(Static = true)]
        public int Mod(int a, int b) => a % b;

        /// <summary>
        /// Get the sine of an angle.
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <returns>Value between -1 and 1.</returns>
        [CLMethod(Static = true)]
        public float Sin(float angle)
        {
            float fAngle = angle * Mathf.Deg2Rad;
            return Mathf.Sin(fAngle);
        }

        /// <summary>
        /// Get the cosine of an angle.
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <returns>Value between -1 and 1.</returns>
        [CLMethod(Static = true)]
        public float Cos(float angle)
        {
            float fAngle = angle * Mathf.Deg2Rad;
            return Mathf.Cos(fAngle);
        }

        /// <summary>
        /// Get the tangent of an angle in radians.
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <returns>The tangent of the angle.</returns>
        [CLMethod(Static = true)]
        public float Tan(float angle)
        {
            float fAngle = angle * Mathf.Deg2Rad;
            return Mathf.Tan(fAngle);
        }

        /// <summary>
        /// Get the arcsine of a value in degrees.
        /// </summary>
        /// <param name="value">The value (must be between -1 and 1).</param>
        /// <returns>The arcsine in degrees.</returns>
        [CLMethod(Static = true)]
        public float Asin(float value) => Mathf.Asin(value) * Mathf.Rad2Deg;

        /// <summary>
        /// Get the arccosine of a value in degrees.
        /// </summary>
        /// <param name="value">The value (must be between -1 and 1).</param>
        /// <returns>The arccosine in degrees.</returns>
        [CLMethod(Static = true)]
        public float Acos(float value) => Mathf.Acos(value) * Mathf.Rad2Deg;

        /// <summary>
        /// Get the arctangent of a value in degrees.
        /// </summary>
        /// <param name="value">The value to get the arctangent of.</param>
        /// <returns>The arctangent in degrees.</returns>
        [CLMethod(Static = true)]
        public float Atan(float value) => Mathf.Atan(value) * Mathf.Rad2Deg;

        /// <summary>
        /// Get the arctangent of a value in degrees.
        /// </summary>
        /// <param name="a">The Y component.</param>
        /// <param name="b">The X component.</param>
        /// <returns>The arctangent in degrees.</returns>
        [CLMethod(Static = true)]
        public float Atan2(float a, float b) => Mathf.Atan2(a, b) * Mathf.Rad2Deg;

        /// <summary>
        /// Get the smallest integer greater than or equal to a value.
        /// </summary>
        /// <param name="value">The value to round up.</param>
        /// <returns>The smallest integer greater than or equal to the value.</returns>
        [CLMethod(Static = true)]
        public int Ceil(float value) => Mathf.CeilToInt(value);

        /// <summary>
        /// Get the largest integer less than or equal to a value.
        /// </summary>
        /// <param name="value">The value to round down.</param>
        /// <returns>The largest integer less than or equal to the value.</returns>
        [CLMethod(Static = true)]
        public int Floor(float value) => Mathf.FloorToInt(value);

        /// <summary>
        /// Round a value to the nearest integer.
        /// </summary>
        /// <param name="value">The value to round.</param>
        /// <returns>The rounded integer value.</returns>
        [CLMethod(Static = true)]
        public int Round(float value) => Mathf.RoundToInt(value);

        /// <summary>
        /// Convert an angle from degrees to radians.
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <returns>The angle in radians.</returns>
        [CLMethod(Static = true)]
        public float Deg2Rad(float angle) => angle * Mathf.Deg2Rad;

        /// <summary>
        /// Convert an angle from radians to degrees.
        /// </summary>
        /// <param name="angle">The angle in radians.</param>
        /// <returns>The angle in degrees.</returns>
        [CLMethod(Static = true)]
        public float Rad2Deg(float angle) => angle * Mathf.Rad2Deg;

        /// <summary>
        /// Linearly interpolate between two values.
        /// </summary>
        /// <param name="a">The start value.</param>
        /// <param name="b">The end value.</param>
        /// <param name="t">The interpolation factor (clamped between 0 and 1).</param>
        /// <returns>The interpolated value.</returns>
        [CLMethod(Static = true)]
        public float Lerp(float a, float b, float t) => Mathf.Lerp(a, b, t);

        /// <summary>
        /// Linearly interpolate between two values without clamping.
        /// </summary>
        /// <param name="a">The start value.</param>
        /// <param name="b">The end value.</param>
        /// <param name="t">The interpolation factor (not clamped).</param>
        /// <returns>The interpolated value.</returns>
        [CLMethod(Static = true)]
        public float LerpUnclamped(float a, float b, float t) => Mathf.LerpUnclamped(a, b, t);

        /// <summary>
        /// Get the sign of a value.
        /// </summary>
        /// <param name="value">The value to get the sign of.</param>
        /// <returns>The sign of the value (-1, 0, or 1).</returns>
        [CLMethod(Static = true)]
        public float Sign(float value) => Mathf.Sign(value);

        /// <summary>
        /// Get the inverse lerp of two values.
        /// </summary>
        /// <param name="a">The start value.</param>
        /// <param name="b">The end value.</param>
        /// <param name="value">The value to find the interpolation factor for.</param>
        /// <returns>The interpolation factor.</returns>
        [CLMethod(Static = true)]
        public float InverseLerp(float a, float b, float value) => Mathf.InverseLerp(a, b, value);

        /// <summary>
        /// Linearly interpolate between two angles.
        /// </summary>
        /// <param name="a">The start angle in degrees.</param>
        /// <param name="b">The end angle in degrees.</param>
        /// <param name="t">The interpolation factor (clamped between 0 and 1).</param>
        /// <returns>The interpolated angle.</returns>
        [CLMethod(Static = true)]
        public float LerpAngle(float a, float b, float t) => Mathf.LerpAngle(a, b, t);

        /// <summary>
        /// Get the natural logarithm of a value.
        /// </summary>
        /// <param name="value">The value to get the logarithm of.</param>
        /// <returns>The natural logarithm of the value.</returns>
        [CLMethod(Static = true)]
        public float Log(float value) => Mathf.Log(value);

        /// <summary>
        /// Move a value towards a target value.
        /// </summary>
        /// <param name="current">The current value.</param>
        /// <param name="target">The target value.</param>
        /// <param name="maxDelta">The maximum change allowed.</param>
        /// <returns>The new value moved towards the target.</returns>
        [CLMethod(Static = true)]
        public float MoveTowards(float current, float target, float maxDelta) => Mathf.MoveTowards(current, target, maxDelta);

        /// <summary>
        /// Move an angle towards a target angle.
        /// </summary>
        /// <param name="current">The current angle in degrees.</param>
        /// <param name="target">The target angle in degrees.</param>
        /// <param name="maxDelta">The maximum change in degrees allowed.</param>
        /// <returns>The new angle moved towards the target.</returns>
        [CLMethod(Static = true)]
        public float MoveTowardsAngle(float current, float target, float maxDelta) => Mathf.MoveTowardsAngle(current, target, maxDelta);

        /// <summary>
        /// Get the ping pong value of a time value.
        /// </summary>
        /// <param name="t">The time value.</param>
        /// <param name="length">The length of the ping pong cycle.</param>
        /// <returns>The ping pong value.</returns>
        [CLMethod(Static = true)]
        public float PingPong(float t, float length) => Mathf.PingPong(t, length);

        /// <summary>
        /// Get the exponential value of a number.
        /// </summary>
        /// <param name="value">The value to exponentiate.</param>
        /// <returns>The exponential value.</returns>
        [CLMethod(Static = true)]
        public float Exp(float value) => Mathf.Exp(value);

        /// <summary>
        /// Smoothly step between two values.
        /// </summary>
        /// <param name="a">The start value.</param>
        /// <param name="b">The end value.</param>
        /// <param name="t">The interpolation factor (clamped between 0 and 1).</param>
        /// <returns>The smoothly interpolated value.</returns>
        [CLMethod(Static = true)]
        public float SmoothStep(float a, float b, float t) => Mathf.SmoothStep(a, b, t);

        /// <summary>
        /// Perform a bitwise AND operation.
        /// </summary>
        /// <param name="a">The first integer.</param>
        /// <param name="b">The second integer.</param>
        /// <returns>The result of the bitwise AND operation.</returns>
        [CLMethod(Static = true)]
        public int BitwiseAnd(int a, int b) => a & b;

        /// <summary>
        /// Perform a bitwise OR operation.
        /// </summary>
        /// <param name="a">The first integer.</param>
        /// <param name="b">The second integer.</param>
        /// <returns>The result of the bitwise OR operation.</returns>
        [CLMethod(Static = true)]
        public int BitwiseOr(int a, int b) => a | b;

        /// <summary>
        /// Perform a bitwise XOR operation.
        /// </summary>
        /// <param name="a">The first integer.</param>
        /// <param name="b">The second integer.</param>
        /// <returns>The result of the bitwise XOR operation.</returns>
        [CLMethod(Static = true)]
        public int BitwiseXor(int a, int b) => a ^ b;

        /// <summary>
        /// Perform a bitwise NOT operation.
        /// </summary>
        /// <param name="value">The integer to negate.</param>
        /// <returns>The result of the bitwise NOT operation.</returns>
        [CLMethod(Static = true)]
        public int BitwiseNot(int value) => ~value;

        /// <summary>
        /// Shift bits to the left.
        /// </summary>
        /// <param name="value">The integer to shift.</param>
        /// <param name="shift">The number of bits to shift left.</param>
        /// <returns>The result of the left shift operation.</returns>
        [CLMethod(Static = true)]
        public int BitwiseLeftShift(int value, int shift) => value << shift;

        /// <summary>
        /// Shift bits to the right.
        /// </summary>
        /// <param name="value">The integer to shift.</param>
        /// <param name="shift">The number of bits to shift right.</param>
        /// <returns>The result of the right shift operation.</returns>
        [CLMethod(Static = true)]
        public int BitwiseRightShift(int value, int shift) => value >> shift;
    }
}
