﻿using UnityEngine;
using Utility;

namespace CustomLogic
{
    [CLType(Static = true)]
    class CustomLogicVector2Builtin : CustomLogicClassInstanceBuiltin, ICustomLogicMathOperators, ICustomLogicEquals, ICustomLogicCopyable
    {
        private Vector2 _value;
        
        public CustomLogicVector2Builtin(object[] parameterValues) : base("Vector2")
        {
            float x = 0;
            float y = 0;

            if (parameterValues.Length == 1)
            {
                x = parameterValues[0].UnboxToFloat();
                y = x;
            }
            else if (parameterValues.Length > 1)
            {
                x = parameterValues[0].UnboxToFloat();
                y = parameterValues[1].UnboxToFloat();
            }

            _value = new Vector2(x, y);

        }

        public CustomLogicVector2Builtin(Vector2 value) : base("Vector2")
        {
            _value = value;
        }

        /// <inheritdoc cref="Vector2.x"/>
        [CLProperty]
        public float X
        {
            get => _value.x;
            set => _value.x = value;
        }
        
        /// <inheritdoc cref="Vector2.y"/>
        [CLProperty]
        public float Y
        {
            get => _value.y;
            set => _value.y = value;
        }
        
        /// <inheritdoc cref="Vector2.normalized"/>
        [CLProperty] public CustomLogicVector2Builtin Normalized => _value.normalized;
        
        /// <inheritdoc cref="Vector2.magnitude"/>
        [CLProperty] public float Magnitude => _value.magnitude;
        
        /// <inheritdoc cref="Vector2.sqrMagnitude"/>
        [CLProperty] public float SqrMagnitude => _value.sqrMagnitude;
        
        /// <inheritdoc cref="Vector2.zero"/>
        [CLProperty] public static CustomLogicVector2Builtin Zero => Vector2.zero;
        
        /// <inheritdoc cref="Vector2.one"/>
        [CLProperty] public static CustomLogicVector2Builtin One => Vector2.one;
        
        /// <inheritdoc cref="Vector2.up"/>
        [CLProperty] public static CustomLogicVector2Builtin Up => Vector2.up;
        
        /// <inheritdoc cref="Vector2.down"/>
        [CLProperty] public static CustomLogicVector2Builtin Down => Vector2.down;
        
        /// <inheritdoc cref="Vector2.left"/>
        [CLProperty] public static CustomLogicVector2Builtin Left => Vector2.left;
        
        /// <inheritdoc cref="Vector2.right"/>
        [CLProperty] public static CustomLogicVector2Builtin Right => Vector2.right;
        
        /// <inheritdoc cref="Vector2.positiveInfinity"/>
        [CLProperty] public static CustomLogicVector2Builtin NegativeInfinity => Vector2.negativeInfinity;
        
        /// <inheritdoc cref="Vector2.positiveInfinity"/>
        [CLProperty] public static CustomLogicVector2Builtin PositiveInfinity => Vector2.positiveInfinity;
        
        /// <inheritdoc cref="Vector2.Angle"/>
        [CLMethod] public static float Angle(CustomLogicVector2Builtin from, CustomLogicVector2Builtin to) => Vector2.Angle(from, to);
        
        /// <inheritdoc cref="Vector2.ClampMagnitude"/>
        [CLMethod] public static CustomLogicVector2Builtin ClampMagnitude(CustomLogicVector2Builtin vector, float maxLength) => Vector2.ClampMagnitude(vector, maxLength);
        
        /// <inheritdoc cref="Vector2.Distance"/>
        [CLMethod] public static float Distance(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => Vector2.Distance(a, b);
        
        /// <inheritdoc cref="Vector2.Dot"/>
        [CLMethod] public static float Dot(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => Vector2.Dot(a, b);
        
        /// <inheritdoc cref="Vector2.Lerp"/>
        [CLMethod] public static CustomLogicVector2Builtin Lerp(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b, float t) => Vector2.Lerp(a, b, t);
        
        /// <inheritdoc cref="Vector2.LerpUnclamped"/>
        [CLMethod] public static CustomLogicVector2Builtin LerpUnclamped(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b, float t) => Vector2.LerpUnclamped(a, b, t);
        
        /// <inheritdoc cref="Vector2.Max"/>
        [CLMethod] public static CustomLogicVector2Builtin Max(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => Vector2.Max(a, b);
        
        /// <inheritdoc cref="Vector2.Min"/>
        [CLMethod] public static CustomLogicVector2Builtin Min(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => Vector2.Min(a, b);
        
        /// <inheritdoc cref="Vector2.MoveTowards"/>
        [CLMethod]
        public static CustomLogicVector2Builtin MoveTowards(CustomLogicVector2Builtin current, CustomLogicVector2Builtin target, float maxDistanceDelta) => Vector2.MoveTowards(current, target, maxDistanceDelta);
        
        /// <inheritdoc cref="Vector2.Reflect"/>
        [CLMethod]
        public static CustomLogicVector2Builtin Reflect(CustomLogicVector2Builtin inDirection, CustomLogicVector2Builtin inNormal) => Vector2.Reflect(inDirection, inNormal);
        
        /// <inheritdoc cref="Vector2.SignedAngle"/>
        [CLMethod]
        public static float SignedAngle(CustomLogicVector2Builtin from, CustomLogicVector2Builtin to) => Vector2.SignedAngle(from, to);
        
        /// <inheritdoc cref="Vector2.SmoothDamp(Vector2, Vector2, ref Vector2, float, float, float)"/>
        [CLMethod]
        public static CustomLogicVector2Builtin SmoothDamp(CustomLogicVector2Builtin current, CustomLogicVector2Builtin target, CustomLogicVector2Builtin currentVelocity, float smoothTime, float maxSpeed) => Vector2.SmoothDamp(current, target, ref currentVelocity._value, smoothTime, maxSpeed);
        
        /// <inheritdoc cref="Vector2.Set"/>
        [CLMethod] public void Set(float x, float y) => _value.Set(x, y);
        
        /// <inheritdoc cref="Vector2.Normalize"/>
        [CLMethod] public void Normalize() => _value.Normalize();
        
        public override string ToString()
        {
            return _value.ToString();
        }
        
        [CLMethod]
        public virtual object __Copy__()
        {
            var value = new Vector2(_value.x, _value.y);
            return new CustomLogicVector2Builtin(value);
        }

        [CLMethod]
        public object __Add__(object other)
        {
            if (other is CustomLogicVector2Builtin v3)
                return new CustomLogicVector2Builtin(_value + v3._value);
            
            throw CustomLogicUtils.OperatorException(nameof(__Add__), this, other);
        }
        
        [CLMethod]
        public object __Sub__(object other)
        {
            if (other is CustomLogicVector2Builtin v3)
                return new CustomLogicVector2Builtin(_value - v3._value);
            
            throw CustomLogicUtils.OperatorException(nameof(__Sub__), this, other);
        }
        
        [CLMethod]
        public object __Mul__(object other)
        {
            return other switch
            {
                float f => new CustomLogicVector2Builtin(_value * f),
                CustomLogicVector2Builtin v3 => new CustomLogicVector2Builtin(Util.MultiplyVectors(this, v3)),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Mul__), this, other)
            };
        }
        
        [CLMethod]
        public object __Div__(object other)
        {
            return other switch
            {
                float f => new CustomLogicVector2Builtin(_value / f),
                CustomLogicVector2Builtin v3 => new CustomLogicVector2Builtin(Util.DivideVectors(this, v3)),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Div__), this, other)
            };
        }

        [CLMethod]
        public bool __Eq__(object other)
        {
            if (other is not CustomLogicVector2Builtin v2)
                return false;
            
            return _value == v2._value;
        }

        [CLMethod]
        public int __Hash__() => _value.GetHashCode();

        public static implicit operator Vector2(CustomLogicVector2Builtin value) => value._value;
        public static implicit operator CustomLogicVector2Builtin(Vector2 value) => new CustomLogicVector2Builtin(value);
    }
}