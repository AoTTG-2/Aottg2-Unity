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

        [CLProperty("The value of PI.")]
        public static float PI => Mathf.PI;

        [CLProperty("The value of Infinity.")]
        public static float Infinity => Mathf.Infinity;

        [CLProperty("The value of Negative Infinity.")]
        public static float NegativeInfinity => Mathf.NegativeInfinity;

        [CLProperty("The value of Rad2Deg constant.")]
        public static float Rad2DegConstant => Mathf.Rad2Deg;

        [CLProperty("The value of Deg2Rad constant.")]
        public static float Deg2RadConstant => Mathf.Deg2Rad;

        [CLProperty("The value of Epsilon.")]
        public static float Epsilon => Mathf.Epsilon;

        [CLMethod(Static = true, Description = "Clamp a value between a minimum and maximum value. Returns: The clamped value. Will be the same type as the inputs")]
        public object Clamp(
            [CLParam("The value to clamp. Can be int or float")]
            object value,
            [CLParam("The minimum value. Can be int or float")]
            object min,
            [CLParam("The maximum value. Can be int or float")]
            object max)
        {
            if (value is int vInt && min is int minInt && max is int maxInt)
                return Mathf.Clamp(vInt, minInt, maxInt);

            float fValue = CustomLogicEvaluator.ConvertTo<float>(value);
            float fMin = CustomLogicEvaluator.ConvertTo<float>(min);
            float fMax = CustomLogicEvaluator.ConvertTo<float>(max);
            return Mathf.Clamp(fValue, fMin, fMax);
        }

        [CLMethod(Static = true, Description = "Get the maximum of two values. Returns: The maximum of the two values. Will be the same type as the inputs")]
        public object Max(
            [CLParam("The first value. Can be int or float")]
            object a,
            [CLParam("The second value. Can be int or float")]
            object b)
        {
            if (a is int aInt && b is int bInt)
                return Mathf.Max(aInt, bInt);

            float fA = CustomLogicEvaluator.ConvertTo<float>(a);
            float fB = CustomLogicEvaluator.ConvertTo<float>(b);
            return Mathf.Max(fA, fB);
        }

        [CLMethod(Static = true, Description = "Get the minimum of two values. Returns: The minimum of the two values. Will be the same type as the inputs")]
        public object Min(
            [CLParam("The first value. Can be int or float")]
            object a,
            [CLParam("The second value. Can be int or float")]
            object b)
        {
            if (a is int aInt && b is int bInt)
                return Mathf.Min(aInt, bInt);

            float fA = CustomLogicEvaluator.ConvertTo<float>(a);
            float fB = CustomLogicEvaluator.ConvertTo<float>(b);
            return Mathf.Min(fA, fB);
        }

        [CLMethod(Static = true, Description = "Raise a value to the power of another value")]
        public float Pow(
            [CLParam("The base value.")]
            float a,
            [CLParam("The exponent value.")]
            float b)
        {
            return Mathf.Pow(a, b);
        }

        [CLMethod(Static = true, Description = "Get the absolute value of a number. Returns: The absolute value. Will be the same type as the input")]
        public object Abs(
            [CLParam("The number. Can be int or float")]
            object value)
        {
            if (value is int vInt)
                return Mathf.Abs(vInt);

            float fValue = CustomLogicEvaluator.ConvertTo<float>(value);
            return Mathf.Abs(fValue);
        }

        [CLMethod(Static = true, Description = "Get the square root of a number")]
        public float Sqrt(
            [CLParam("The value to get the square root of.")]
            float value)
        {
            return Mathf.Sqrt(value);
        }

        [CLMethod(Static = true, Description = "Modulo for floats")]
        public object Repeat(
            [CLParam("The value to repeat.")]
            object value,
            [CLParam("The maximum value to repeat to.")]
            object max)
        {
            float fValue = value.UnboxToFloat();
            float fMax = max.UnboxToFloat();
            return Mathf.Repeat(fValue, fMax);
        }

        [CLMethod(Static = true, Description = "Get the remainder of a division operation")]
        public int Mod(
            [CLParam("The dividend.")]
            int a,
            [CLParam("The divisor.")]
            int b)
            => a % b;

        [CLMethod(Static = true, Description = "Get the sine of an angle. Returns: Value between -1 and 1")]
        public float Sin(
            [CLParam("The angle in degrees")]
            float angle)
        {
            float fAngle = angle * Mathf.Deg2Rad;
            return Mathf.Sin(fAngle);
        }

        [CLMethod(Static = true, Description = "Get the cosine of an angle. Returns: Value between -1 and 1")]
        public float Cos(
            [CLParam("The angle in degrees")]
            float angle)
        {
            float fAngle = angle * Mathf.Deg2Rad;
            return Mathf.Cos(fAngle);
        }

        [CLMethod(Static = true, Description = "Get the tangent of an angle in radians")]
        public float Tan(
            [CLParam("The angle in degrees")]
            float angle)
        {
            float fAngle = angle * Mathf.Deg2Rad;
            return Mathf.Tan(fAngle);
        }

        [CLMethod(Static = true, Description = "Get the arcsine of a value in degrees")]
        public float Asin(
            [CLParam("The value (must be between -1 and 1).")]
            float value)
        {
            return Mathf.Asin(value) * Mathf.Rad2Deg;
        }

        [CLMethod(Static = true, Description = "Get the arccosine of a value in degrees")]
        public float Acos(
            [CLParam("The value (must be between -1 and 1).")]
            float value)
        {
            return Mathf.Acos(value) * Mathf.Rad2Deg;
        }

        [CLMethod(Static = true, Description = "Get the arctangent of a value in degrees")]
        public float Atan(
            [CLParam("The value to get the arctangent of.")]
            float value)
        {
            return Mathf.Atan(value) * Mathf.Rad2Deg;
        }

        [CLMethod(Static = true, Description = "Get the arctangent of a value in degrees")]
        public float Atan2(
            [CLParam("The Y component.")]
            float a,
            [CLParam("The X component.")]
            float b)
        {
            return Mathf.Atan2(a, b) * Mathf.Rad2Deg;
        }

        [CLMethod(Static = true, Description = "Get the smallest integer greater than or equal to a value")]
        public int Ceil(
            [CLParam("The value to round up.")]
            float value)
        {
            return Mathf.CeilToInt(value);
        }

        [CLMethod(Static = true, Description = "Get the largest integer less than or equal to a value")]
        public int Floor(
            [CLParam("The value to round down.")]
            float value)
        {
            return Mathf.FloorToInt(value);
        }

        [CLMethod(Static = true, Description = "Round a value to the nearest integer")]
        public int Round(
            [CLParam("The value to round.")]
            float value)
        {
            return Mathf.RoundToInt(value);
        }

        [CLMethod(Static = true, Description = "Convert an angle from degrees to radians")]
        public float Deg2Rad(
            [CLParam("The angle in degrees.")]
            float angle)
        {
            return angle * Mathf.Deg2Rad;
        }

        [CLMethod(Static = true, Description = "Convert an angle from radians to degrees")]
        public float Rad2Deg(
            [CLParam("The angle in radians.")]
            float angle)
        {
            return angle * Mathf.Rad2Deg;
        }

        [CLMethod(Static = true, Description = "Linearly interpolate between two values")]
        public float Lerp(
            [CLParam("The start value.")]
            float a,
            [CLParam("The end value.")]
            float b,
            [CLParam("The interpolation factor (clamped between 0 and 1).")]
            float t)
        {
            return Mathf.Lerp(a, b, t);
        }

        [CLMethod(Static = true, Description = "Linearly interpolate between two values without clamping")]
        public float LerpUnclamped(
            [CLParam("The start value.")]
            float a,
            [CLParam("The end value.")]
            float b,
            [CLParam("The interpolation factor (not clamped).")]
            float t)
        {
            return Mathf.LerpUnclamped(a, b, t);
        }

        [CLMethod(Static = true, Description = "Get the sign of a value")]
        public float Sign(
            [CLParam("The value to get the sign of.")]
            float value)
            => Mathf.Sign(value);

        [CLMethod(Static = true, Description = "Get the inverse lerp of two values")]
        public float InverseLerp(
            [CLParam("The start value.")]
            float a,
            [CLParam("The end value.")]
            float b,
            [CLParam("The value to find the interpolation factor for.")]
            float value)
        {
            return Mathf.InverseLerp(a, b, value);
        }

        [CLMethod(Static = true, Description = "Linearly interpolate between two angles")]
        public float LerpAngle(
            [CLParam("The start angle in degrees.")]
            float a,
            [CLParam("The end angle in degrees.")]
            float b,
            [CLParam("The interpolation factor (clamped between 0 and 1).")]
            float t)
        {
            return Mathf.LerpAngle(a, b, t);
        }

        [CLMethod(Static = true, Description = "Get the natural logarithm of a value")]
        public float Log(
            [CLParam("The value to get the logarithm of.")]
            float value)
        {
            return Mathf.Log(value);
        }

        [CLMethod(Static = true, Description = "Move a value towards a target value")]
        public float MoveTowards(
            [CLParam("The current value.")]
            float current,
            [CLParam("The target value.")]
            float target,
            [CLParam("The maximum change allowed.")]
            float maxDelta)
        {
            return Mathf.MoveTowards(current, target, maxDelta);
        }

        [CLMethod(Static = true, Description = "Move an angle towards a target angle")]
        public float MoveTowardsAngle(
            [CLParam("The current angle in degrees.")]
            float current,
            [CLParam("The target angle in degrees.")]
            float target,
            [CLParam("The maximum change in degrees allowed.")]
            float maxDelta)
        {
            return Mathf.MoveTowardsAngle(current, target, maxDelta);
        }

        [CLMethod(Static = true, Description = "Get the ping pong value of a time value")]
        public float PingPong(
            [CLParam("The time value.")]
            float t,
            [CLParam("The length of the ping pong cycle.")]
            float length)
            => Mathf.PingPong(t, length);

        [CLMethod(Static = true, Description = "Get the exponential value of a number")]
        public float Exp(
            [CLParam("The value to exponentiate.")]
            float value)
            => Mathf.Exp(value);

        [CLMethod(Static = true, Description = "Smoothly step between two values")]
        public float SmoothStep(
            [CLParam("The start value.")]
            float a,
            [CLParam("The end value.")]
            float b,
            [CLParam("The interpolation factor (clamped between 0 and 1).")]
            float t)
            => Mathf.SmoothStep(a, b, t);

        [CLMethod(Static = true, Description = "Perform a bitwise AND operation")]
        public int BitwiseAnd(
            [CLParam("The first integer.")]
            int a,
            [CLParam("The second integer.")]
            int b)
            => a & b;

        [CLMethod(Static = true, Description = "Perform a bitwise OR operation")]
        public int BitwiseOr(
            [CLParam("The first integer.")]
            int a,
            [CLParam("The second integer.")]
            int b)
            => a | b;

        [CLMethod(Static = true, Description = "Perform a bitwise XOR operation")]
        public int BitwiseXor(
            [CLParam("The first integer.")]
            int a,
            [CLParam("The second integer.")]
            int b)
            => a ^ b;

        [CLMethod(Static = true, Description = "Perform a bitwise NOT operation")]
        public int BitwiseNot(
            [CLParam("The integer to negate.")]
            int value)
            => ~value;

        [CLMethod(Static = true, Description = "Shift bits to the left")]
        public int BitwiseLeftShift(
            [CLParam("The integer to shift.")]
            int value,
            [CLParam("The number of bits to shift left.")]
            int shift)
            => value << shift;

        [CLMethod(Static = true, Description = "Shift bits to the right")]
        public int BitwiseRightShift(
            [CLParam("The integer to shift.")]
            int value,
            [CLParam("The number of bits to shift right.")]
            int shift)
            => value >> shift;
    }
}
