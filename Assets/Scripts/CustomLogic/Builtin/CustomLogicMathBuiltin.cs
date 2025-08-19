using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Math functions. Note that parameter types can be either int or float unless otherwise specified.
    /// Functions may return int or float depending on the parameter types given.
    /// </summary>
    [CLType(Name = "Math", Abstract = true, Static = true)]
    partial class CustomLogicMathBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicMathBuiltin()
        {
        }

        [CLProperty(description: "The value of PI")]
        public static float PI => Mathf.PI;

        [CLProperty(description: "The value of Infinity")]
        public static float Infinity => Mathf.Infinity;

        [CLProperty(description: "The value of Negative Infinity")]
        public static float NegativeInfinity => Mathf.NegativeInfinity;

        [CLProperty(description: "The value of Rad2Deg constant")]
        public static float Rad2DegConstant => Mathf.Rad2Deg;

        [CLProperty(description: "The value of Deg2Rad constant")]
        public static float Deg2RadConstant => Mathf.Deg2Rad;

        [CLProperty(description: "The value of Epsilon")]
        public static float Epsilon => Mathf.Epsilon;

        /// <summary>
        /// Clamp a value between a minimum and maximum value
        /// </summary>
        /// <param name="value">The value to clamp. Can be int or float</param>
        /// <param name="min">The minimum value. Can be int or float</param>
        /// <param name="max">The maximum value. Can be int or float</param>
        /// <returns>The clamped value. Will be the same type as the inputs</returns>
        [CLMethod]
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
        /// Get the maximum of two values
        /// </summary>
        /// <param name="a">The first value. Can be int or float</param>
        /// <param name="b">The second value. Can be int or float</param>
        /// <returns>The maximum of the two values. Will be the same type as the inputs</returns>
        [CLMethod]
        public object Max(object a, object b)
        {
            if (a is int aInt && b is int bInt)
                return Mathf.Max(aInt, bInt);

            float fA = CustomLogicEvaluator.ConvertTo<float>(a);
            float fB = CustomLogicEvaluator.ConvertTo<float>(b);
            return Mathf.Max(fA, fB);
        }

        /// <summary>
        /// Get the minimum of two values
        /// </summary>
        /// <param name="a">The first value. Can be int or float</param>
        /// <param name="b">The second value. Can be int or float</param>
        /// <returns>The minimum of the two values. Will be the same type as the inputs</returns>
        [CLMethod]
        public object Min(object a, object b)
        {
            if (a is int aInt && b is int bInt)
                return Mathf.Min(aInt, bInt);

            float fA = CustomLogicEvaluator.ConvertTo<float>(a);
            float fB = CustomLogicEvaluator.ConvertTo<float>(b);
            return Mathf.Min(fA, fB);
        }

        [CLMethod(description: "Raise a value to the power of another value")]
        public float Pow(float a, float b)
        {
            return Mathf.Pow(a, b);
        }

        /// <summary>
        /// Get the absolute value of a number
        /// </summary>
        /// <param name="value">The number. Can be int or float</param>
        /// <returns>The absolute value. Will be the same type as the input</returns>
        [CLMethod]
        public object Abs(object value)
        {
            if (value is int vInt)
                return Mathf.Abs(vInt);

            float fValue = CustomLogicEvaluator.ConvertTo<float>(value);
            return Mathf.Abs(fValue);
        }

        [CLMethod(description: "Get the square root of a number")]
        public float Sqrt(float value)
        {
            return Mathf.Sqrt(value);
        }

        // Repeat
        [CLMethod(description: "Modulo for floats")]
        public object Repeat(object value, object max)
        {
            float fValue = value.UnboxToFloat();
            float fMax = max.UnboxToFloat();
            return Mathf.Repeat(fValue, fMax);
        }

        [CLMethod(description: "Get the remainder of a division operation")]
        public int Mod(int a, int b) => a % b;

        /// <summary>
        /// Get the sine of an angle
        /// </summary>
        /// <param name="angle">The angle in degrees</param>
        /// <returns>Value between -1 and 1</returns>
        [CLMethod]
        public float Sin(float angle)
        {
            float fAngle = angle * Mathf.Deg2Rad;
            return Mathf.Sin(fAngle);
        }

        /// <summary>
        /// Get the cosine of an angle
        /// </summary>
        /// <param name="angle">The angle in degrees</param>
        /// <returns>Value between -1 and 1</returns>
        [CLMethod]
        public float Cos(float angle)
        {
            float fAngle = angle * Mathf.Deg2Rad;
            return Mathf.Cos(fAngle);
        }

        /// <summary>
        /// Get the tangent of an angle in radians
        /// </summary>
        /// <param name="angle">The angle in degrees</param>
        [CLMethod]
        public float Tan(float angle)
        {
            float fAngle = angle * Mathf.Deg2Rad;
            return Mathf.Tan(fAngle);
        }

        [CLMethod(description: "Get the arcsine of a value in degrees")]
        public float Asin(float value)
        {
            return Mathf.Asin(value) * Mathf.Rad2Deg;
        }

        [CLMethod(description: "Get the arccosine of a value in degrees")]
        public float Acos(float value)
        {
            return Mathf.Acos(value) * Mathf.Rad2Deg;
        }

        [CLMethod(description: "Get the arctangent of a value in degrees")]
        public float Atan(float value)
        {
            return Mathf.Atan(value) * Mathf.Rad2Deg;
        }

        [CLMethod(description: "Get the arctangent of a value in degrees")]
        public float Atan2(float a, float b)
        {
            return Mathf.Atan2(a, b) * Mathf.Rad2Deg;
        }

        [CLMethod(description: "Get the smallest integer greater than or equal to a value")]
        public int Ceil(float value)
        {
            return Mathf.CeilToInt(value);
        }

        [CLMethod(description: "Get the largest integer less than or equal to a value")]
        public int Floor(float value)
        {
            return Mathf.FloorToInt(value);
        }

        [CLMethod(description: "Round a value to the nearest integer")]
        public int Round(float value)
        {
            return Mathf.RoundToInt(value);
        }

        [CLMethod(description: "Convert an angle from degrees to radians")]
        public float Deg2Rad(float angle)
        {
            return angle * Mathf.Deg2Rad;
        }

        [CLMethod(description: "Convert an angle from radians to degrees")]
        public float Rad2Deg(float angle)
        {
            return angle * Mathf.Rad2Deg;
        }

        [CLMethod(description: "Linearly interpolate between two values")]
        public float Lerp(float a, float b, float t)
        {
            return Mathf.Lerp(a, b, t);
        }

        [CLMethod(description: "Linearly interpolate between two values without clamping")]
        public float LerpUnclamped(float a, float b, float t)
        {
            return Mathf.LerpUnclamped(a, b, t);
        }

        [CLMethod(description: "Get the sign of a value")]
        public float Sign(float value) => Mathf.Sign(value);

        // Inverse lerp
        [CLMethod(description: "Get the inverse lerp of two values")]
        public float InverseLerp(float a, float b, float value)
        {
            return Mathf.InverseLerp(a, b, value);
        }

        // Lerp Angle
        [CLMethod(description: "Linearly interpolate between two angles")]
        public float LerpAngle(float a, float b, float t)
        {
            return Mathf.LerpAngle(a, b, t);
        }

        // Log
        [CLMethod(description: "Get the natural logarithm of a value")]
        public float Log(float value)
        {
            return Mathf.Log(value);
        }

        // Move Towards
        [CLMethod(description: "Move a value towards a target value")]
        public float MoveTowards(float current, float target, float maxDelta)
        {
            return Mathf.MoveTowards(current, target, maxDelta);
        }

        // Move Towards Angle
        [CLMethod(description: "Move an angle towards a target angle")]
        public float MoveTowardsAngle(float current, float target, float maxDelta)
        {
            return Mathf.MoveTowardsAngle(current, target, maxDelta);
        }

        // Ping Pong
        [CLMethod(description: "Get the ping pong value of a time value")]
        public float PingPong(float t, float length) => Mathf.PingPong(t, length);

        // exp
        [CLMethod(description: "Get the exponential value of a number")]
        public float Exp(float value) => Mathf.Exp(value);

        // TODO: I don't think SmoothDamp and SmoothDampAngle actually work because the cl doesn't support ref parameters

        // Smooth Damp
        [CLMethod(description: "Smoothly damp a value towards a target value")]
        public float SmoothDamp(float current, float target, float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
        {
            return Mathf.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        // Smooth Damp Angle
        [CLMethod(description: "Smoothly damp an angle towards a target angle")]
        public float SmoothDampAngle(float current, float target, float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
        {
            return Mathf.SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        // SmoothStep
        [CLMethod(description: "Smoothly step between two values")]
        public float SmoothStep(float a, float b, float t) => Mathf.SmoothStep(a, b, t);

        // Eventually also add these as symbols to the language.
        [CLMethod(description: "Perform a bitwise AND operation")]
        public int BitwiseAnd(int a, int b) => a & b;

        [CLMethod(description: "Perform a bitwise OR operation")]
        public int BitwiseOr(int a, int b) => a | b;

        [CLMethod(description: "Perform a bitwise XOR operation")]
        public int BitwiseXor(int a, int b) => a ^ b;

        [CLMethod(description: "Perform a bitwise NOT operation")]
        public int BitwiseNot(int value) => ~value;

        [CLMethod(description: "Shift bits to the left")]
        public int BitwiseLeftShift(int value, int shift) => value << shift;

        [CLMethod(description: "Shift bits to the right")]
        public int BitwiseRightShift(int value, int shift) => value >> shift;
    }
}
