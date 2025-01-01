using ApplicationManagers;
using GameManagers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace CustomLogic
{
    [CLType(Name = "Math", Abstract = true, InheritBaseMembers = true, Static = true)]
    partial class CustomLogicMathBuiltin : CustomLogicClassInstanceBuiltin
    {
        public CustomLogicMathBuiltin() : base("Math")
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


        // Implement all static methods from CallMethod
        [CLMethod(description: "Clamp a value between a minimum and maximum value")]
        public object Clamp(object value, object min, object max)
        {
            if (value is int && min is int && max is int)
                return Mathf.Clamp((int)value, (int)min, (int)max);
            float fValue = value.UnboxToFloat();
            float fMin = min.UnboxToFloat();
            float fMax = max.UnboxToFloat();
            return Mathf.Clamp(fValue, fMin, fMax);
        }

        [CLMethod(description: "Get the maximum of two values")]
        public object Max(object a, object b)
        {
            if (a is int && b is int)
                return Mathf.Max((int)a, (int)b);
            float fA = a.UnboxToFloat();
            float fB = b.UnboxToFloat();
            return Mathf.Max(fA, fB);
        }

        [CLMethod(description: "Get the minimum of two values")]
        public object Min(object a, object b)
        {
            if (a is int && b is int)
                return Mathf.Min((int)a, (int)b);
            float fA = a.UnboxToFloat();
            float fB = b.UnboxToFloat();
            return Mathf.Min(fA, fB);
        }

        [CLMethod(description: "Raise a value to the power of another value")]
        public object Pow(object a, object b)
        {
            float fA = a.UnboxToFloat();
            float fB = b.UnboxToFloat();
            return Mathf.Pow(fA, fB);
        }

        [CLMethod(description: "Get the absolute value of a number")]
        public object Abs(object value)
        {
            if (value is int)
                return Mathf.Abs((int)value);
            float fValue = value.UnboxToFloat();
            return Mathf.Abs(fValue);
        }

        [CLMethod(description: "Get the square root of a number")]
        public object Sqrt(object value)
        {
            float fValue = value.UnboxToFloat();
            return Mathf.Sqrt(fValue);
        }

        [CLMethod(description: "Get the remainder of a division operation")]
        public object Mod(object a, object b)
        {
            int iA = a.UnboxToInt();
            int iB = b.UnboxToInt();
            return iA % iB;
        }

        [CLMethod(description: "Get the sine of an angle in degrees")]
        public object Sin(object angle)
        {
            float fAngle = angle.UnboxToFloat() * Mathf.Deg2Rad;
            return Mathf.Sin(fAngle);
        }

        [CLMethod(description: "Get the cosine of an angle in degrees")]
        public object Cos(object angle)
        {
            float fAngle = angle.UnboxToFloat() * Mathf.Deg2Rad;
            return Mathf.Cos(fAngle);
        }

        [CLMethod(description: "Get the tangent of an angle in degrees")]
        public object Tan(object angle)
        {
            float fAngle = angle.UnboxToFloat() * Mathf.Deg2Rad;
            return Mathf.Tan(fAngle);
        }

        [CLMethod(description: "Get the arcsine of a value in degrees")]
        public object Asin(object value)
        {
            float fValue = value.UnboxToFloat();
            return Mathf.Asin(fValue) * Mathf.Rad2Deg;
        }

        [CLMethod(description: "Get the arccosine of a value in degrees")]
        public object Acos(object value)
        {
            float fValue = value.UnboxToFloat();
            return Mathf.Acos(fValue) * Mathf.Rad2Deg;
        }

        [CLMethod(description: "Get the arctangent of a value in degrees")]
        public object Atan(object value)
        {
            float fValue = value.UnboxToFloat();
            return Mathf.Atan(fValue) * Mathf.Rad2Deg;
        }

        [CLMethod(description: "Get the arctangent of a value in degrees")]
        public object Atan2(object a, object b)
        {
            float fA = a.UnboxToFloat();
            float fB = b.UnboxToFloat();
            return Mathf.Atan2(fA, fB) * Mathf.Rad2Deg;
        }

        [CLMethod(description: "Get the smallest integer greater than or equal to a value")]
        public object Ceil(object value)
        {
            float fValue = value.UnboxToFloat();
            return Mathf.CeilToInt(fValue);
        }

        [CLMethod(description: "Get the largest integer less than or equal to a value")]
        public object Floor(object value)
        {
            float fValue = value.UnboxToFloat();
            return Mathf.FloorToInt(fValue);
        }

        [CLMethod(description: "Round a value to the nearest integer")]
        public object Round(object value)
        {
            float fValue = value.UnboxToFloat();
            return Mathf.RoundToInt(fValue);
        }

        [CLMethod(description: "Convert an angle from degrees to radians")]
        public object Deg2Rad(object angle)
        {
            float fAngle = angle.UnboxToFloat();
            return fAngle * Mathf.Deg2Rad;
        }

        [CLMethod(description: "Convert an angle from radians to degrees")]
        public object Rad2Deg(object angle)
        {
            float fAngle = angle.UnboxToFloat();
            return fAngle * Mathf.Rad2Deg;
        }

        [CLMethod(description: "Linearly interpolate between two values")]
        public object Lerp(object a, object b, object t)
        {
            float fA = a.UnboxToFloat();
            float fB = b.UnboxToFloat();
            float fT = t.UnboxToFloat();
            return Mathf.Lerp(fA, fB, fT);
        }

        [CLMethod(description: "Linearly interpolate between two values without clamping")]
        public object LerpUnclamped(object a, object b, object t)
        {
            float fA = a.UnboxToFloat();
            float fB = b.UnboxToFloat();
            float fT = t.UnboxToFloat();
            return Mathf.LerpUnclamped(fA, fB, fT);
        }

        [CLMethod(description: "Get the sign of a value")]
        public object Sign(object value)
        {
            float fValue = value.UnboxToFloat();
            return Mathf.Sign(fValue);
        }

        // Inverse lerp
        [CLMethod(description: "Get the inverse lerp of two values")]
        public object InverseLerp(object a, object b, object value)
        {
            float fA = a.UnboxToFloat();
            float fB = b.UnboxToFloat();
            float fValue = value.UnboxToFloat();
            return Mathf.InverseLerp(fA, fB, fValue);
        }

        // Lerp Angle
        [CLMethod(description: "Linearly interpolate between two angles")]
        public object LerpAngle(object a, object b, object t)
        {
            float fA = a.UnboxToFloat();
            float fB = b.UnboxToFloat();
            float fT = t.UnboxToFloat();
            return Mathf.LerpAngle(fA, fB, fT);
        }

        // Log
        [CLMethod(description: "Get the natural logarithm of a value")]
        public object Log(object value)
        {
            float fValue = value.UnboxToFloat();
            return Mathf.Log(fValue);
        }

        // Move Towards
        [CLMethod(description: "Move a value towards a target value")]
        public object MoveTowards(object current, object target, object maxDelta)
        {
            float fCurrent = current.UnboxToFloat();
            float fTarget = target.UnboxToFloat();
            float fMaxDelta = maxDelta.UnboxToFloat();
            return Mathf.MoveTowards(fCurrent, fTarget, fMaxDelta);
        }

        // Move Towards Angle
        [CLMethod(description: "Move an angle towards a target angle")]
        public object MoveTowardsAngle(object current, object target, object maxDelta)
        {
            float fCurrent = current.UnboxToFloat();
            float fTarget = target.UnboxToFloat();
            float fMaxDelta = maxDelta.UnboxToFloat();
            return Mathf.MoveTowardsAngle(fCurrent, fTarget, fMaxDelta);
        }

        // Ping Pong
        [CLMethod(description: "Get the ping pong value of a time value")]
        public object PingPong(object t, object length)
        {
            float fT = t.UnboxToFloat();
            float fLength = length.UnboxToFloat();
            return Mathf.PingPong(fT, fLength);
        }

        // Smooth Damp
        [CLMethod(description: "Smoothly damp a value towards a target value")]
        public object SmoothDamp(object current, object target, object currentVelocity, object smoothTime, object maxSpeed, object deltaTime)
        {
            float fCurrent = current.UnboxToFloat();
            float fTarget = target.UnboxToFloat();
            float fCurrentVelocity = currentVelocity.UnboxToFloat();
            float fSmoothTime = smoothTime.UnboxToFloat();
            float fMaxSpeed = maxSpeed.UnboxToFloat();
            float fDeltaTime = deltaTime.UnboxToFloat();
            return Mathf.SmoothDamp(fCurrent, fTarget, ref fCurrentVelocity, fSmoothTime, fMaxSpeed, fDeltaTime);
        }

        // exp
        [CLMethod(description: "Get the exponential value of a number")]
        public object Exp(object value)
        {
            float fValue = value.UnboxToFloat();
            return Mathf.Exp(fValue);
        }

        // Smooth Damp Angle
        [CLMethod(description: "Smoothly damp an angle towards a target angle")]
        public object SmoothDampAngle(object current, object target, object currentVelocity, object smoothTime, object maxSpeed, object deltaTime)
        {
            float fCurrent = current.UnboxToFloat();
            float fTarget = target.UnboxToFloat();
            float fCurrentVelocity = currentVelocity.UnboxToFloat();
            float fSmoothTime = smoothTime.UnboxToFloat();
            float fMaxSpeed = maxSpeed.UnboxToFloat();
            float fDeltaTime = deltaTime.UnboxToFloat();
            return Mathf.SmoothDampAngle(fCurrent, fTarget, ref fCurrentVelocity, fSmoothTime, fMaxSpeed, fDeltaTime);
        }

        // SmoothStep
        [CLMethod(description: "Smoothly step between two values")]
        public object SmoothStep(object a, object b, object t)
        {
            float fA = a.UnboxToFloat();
            float fB = b.UnboxToFloat();
            float fT = t.UnboxToFloat();
            return Mathf.SmoothStep(fA, fB, fT);
        }

        // Eventually also add these as symbols to the language.
        [CLMethod(description: "Perform a bitwise AND operation")]
        public object BitwiseAnd(object a, object b)
        {
            int iA = a.UnboxToInt();
            int iB = b.UnboxToInt();
            return iA & iB;
        }

        [CLMethod(description: "Perform a bitwise OR operation")]
        public object BitwiseOr(object a, object b)
        {
            int iA = a.UnboxToInt();
            int iB = b.UnboxToInt();
            return iA | iB;
        }

        [CLMethod(description: "Perform a bitwise XOR operation")]
        public object BitwiseXor(object a, object b)
        {
            int iA = a.UnboxToInt();
            int iB = b.UnboxToInt();
            return iA ^ iB;
        }

        [CLMethod(description: "Perform a bitwise NOT operation")]
        public object BitwiseNot(object value)
        {
            int iValue = value.UnboxToInt();
            return ~iValue;
        }

        [CLMethod(description: "Shift bits to the left")]
        public object BitwiseLeftShift(object value, object shift)
        {
            int iValue = value.UnboxToInt();
            int iShift = shift.UnboxToInt();
            return iValue << iShift;
        }

        [CLMethod(description: "Shift bits to the right")]
        public object BitwiseRightShift(object value, object shift)
        {
            int iValue = value.UnboxToInt();
            int iShift = shift.UnboxToInt();
            return iValue >> iShift;
        }

    }
}
