using CustomLogic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Builtin
{
    [CLType]
    class CustomLogicVector2Builtin : CustomLogicClassInstanceBuiltin, ICustomLogicMathOperators, ICustomLogicEquals, ICustomLogicCopyable
    {
        private Vector2 _value;
        public CustomLogicVector2Builtin(List<object> parameterValues, string name = "Vector2") : base(name)
        {
            float x = 0;
            float y = 0;

            if (parameterValues.Count == 1)
            {
                x = parameterValues[0].UnboxToFloat();
                y = x;
            }
            else if (parameterValues.Count > 1)
            {
                x = parameterValues[0].UnboxToFloat();
                y = parameterValues[1].UnboxToFloat();
            }

            _value = new Vector2(x, y);

        }

        public CustomLogicVector2Builtin(Vector2 value, string name = "Vector2") : base(name)
        {
            _value = value;
        }

        public CustomLogicVector2Builtin Copy()
        {
            Vector2 value = new Vector2(_value.x, _value.y);
            return new CustomLogicVector2Builtin(value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CustomLogicVector2Builtin))
                return false;
            var other = ((CustomLogicVector2Builtin)obj)._value;
            return _value == other;
        }

        #region Properties
        [CLProperty(description: "X axis of the vector")]
        public float X { get => _value.x; set => _value.x = value; }

        [CLProperty(description: "Y axis of the vector")]
        public float Y { get => _value.y; set => _value.y = value; }

        [CLProperty(description: "Normalized version of the vector")]
        public CustomLogicVector2Builtin Normalized => new CustomLogicVector2Builtin(_value.normalized);

        [CLProperty(description: "Magnitude of the vector")]
        public float Magnitude => _value.magnitude;

        [CLProperty(description: "SqrMagnitude of the vector")]
        public float SqrMagnitude => _value.sqrMagnitude;
        #endregion

        #region Static Properties
        [CLProperty(description: "Shorthand for writing Vector2(0, -1).")]
        public static CustomLogicVector2Builtin Down => new CustomLogicVector2Builtin(Vector2.down);
        
        [CLProperty(description: "Shorthand for writing Vector2(-1, 0).")]
        public static CustomLogicVector2Builtin Left => new CustomLogicVector2Builtin(Vector2.left);
        
        [CLProperty(description: "Shorthand for writing Vector2(float.NegativeInfinity, float.NegativeInfinity).")]
        public static CustomLogicVector2Builtin NegativeInfinity => new CustomLogicVector2Builtin(new Vector2(float.NegativeInfinity, float.NegativeInfinity));
        
        [CLProperty(description: "Shorthand for writing Vector2(1, 1).")]
        public static CustomLogicVector2Builtin One => new CustomLogicVector2Builtin(Vector2.one);
        
        [CLProperty(description: "Shorthand for writing Vector2(1, 0).")]
        public static CustomLogicVector2Builtin Right => new CustomLogicVector2Builtin(Vector2.right);
        
        [CLProperty(description: "Shorthand for writing Vector2(0, 1).")]
        public static CustomLogicVector2Builtin Up => new CustomLogicVector2Builtin(Vector2.up);
        
        [CLProperty(description: "Shorthand for writing Vector2(0, 0).")]
        public static CustomLogicVector2Builtin Zero => new CustomLogicVector2Builtin(Vector2.zero);
        #endregion

        #region Methods
        [CLMethod(description: "Returns true if the given vector is exactly equal to this vector.")]
        public bool Equals(CustomLogicVector2Builtin other) => _value == other._value;

        [CLMethod(description: "Makes this vector have a magnitude of 1.")]
        public void Normalize() => _value.Normalize();

        [CLMethod(description: "Set x and y components of an existing Vector2.")]
        public void Set(float newX, float newY) => _value.Set(newX, newY);

        [CLMethod(description: "Returns a formatted string for this vector.")]
        public override string ToString() => _value.ToString();
        #endregion

        #region Operations
        [CLMethod(description:"Addition")]
        public object __Add__(object other)
        {
            if (other is CustomLogicVector2Builtin == false)
                throw new Exception("Invalid operation, rhs was null.");

            return new CustomLogicVector2Builtin(this._value + ((CustomLogicVector2Builtin)other)._value);
        }

        [CLMethod(description: "Subtraction")]
        public object __Sub__(object other)
        {
            if (other is CustomLogicVector2Builtin == false)
                throw new Exception("Invalid operation, rhs was null.");

            return new CustomLogicVector2Builtin(this._value - ((CustomLogicVector2Builtin)other)._value);
        }

        [CLMethod(description: "Multiplication")]
        public object __Mul__(object other)
        {
            if (other is CustomLogicVector2Builtin == false)
                throw new Exception("Invalid operation, rhs was null.");

            return new CustomLogicVector2Builtin(this._value * ((CustomLogicVector2Builtin)other)._value);
        }

        [CLMethod(description: "Division")]
        public object __Div__(object other)
        {
            if (other is CustomLogicVector2Builtin == false)
                throw new Exception("Invalid operation, rhs was null.");

            return new CustomLogicVector2Builtin(this._value / ((CustomLogicVector2Builtin)other)._value);
        }

        [CLMethod(description: "Equals")]
        public bool __Eq__(object other) => Equals(other);

        [CLMethod(description: "GetHashCode")]
        public int __Hash__() => this._value.GetHashCode();
        
        [CLMethod(description: "Copy")]
        public virtual object __Copy__() => Copy();
        #endregion

        #region Static Methods
        [CLMethod(description: "Returns the angle in degrees between from and to.")]
        public static float Angle(CustomLogicVector2Builtin from, CustomLogicVector2Builtin to) => Vector2.Angle(from._value, to._value);

        [CLMethod(description: "Returns a copy of vector with its magnitude clamped to maxLength.")]
        public static CustomLogicVector2Builtin ClampMagnitude(CustomLogicVector2Builtin vector, float maxLength) => new CustomLogicVector2Builtin(Vector2.ClampMagnitude(vector._value, maxLength));

        [CLMethod(description: "Returns the distance between a and b.")]
        public static float Distance(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => Vector2.Distance(a._value, b._value);

        [CLMethod(description: "Dot Product of two vectors.")]
        public static float Dot(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => Vector2.Dot(a._value, b._value);

        [CLMethod(description: "Linearly interpolates between vectors a and b by t.")]
        public static CustomLogicVector2Builtin Lerp(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b, float t) => new CustomLogicVector2Builtin(Vector2.Lerp(a._value, b._value, t));

        [CLMethod(description: "Linearly interpolates between vectors a and b by t.")]
        public static CustomLogicVector2Builtin LerpUnclamped(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b, float t) => new CustomLogicVector2Builtin(Vector2.LerpUnclamped(a._value, b._value, t));

        [CLMethod(description: "Returns a vector that is made from the largest components of two vectors.")]
        public static CustomLogicVector2Builtin Max(CustomLogicVector2Builtin lhs, CustomLogicVector2Builtin rhs) => new CustomLogicVector2Builtin(Vector2.Max(lhs._value, rhs._value));

        [CLMethod(description: "Returns a vector that is made from the smallest components of two vectors.")]
        public static CustomLogicVector2Builtin Min(CustomLogicVector2Builtin lhs, CustomLogicVector2Builtin rhs) => new CustomLogicVector2Builtin(Vector2.Min(lhs._value, rhs._value));

        [CLMethod(description: "Moves a point current towards target.")]
        public static CustomLogicVector2Builtin MoveTowards(CustomLogicVector2Builtin current, CustomLogicVector2Builtin target, float maxDistanceDelta) => new CustomLogicVector2Builtin(Vector2.MoveTowards(current._value, target._value, maxDistanceDelta));

        [CLMethod(description: "Returns the 2D vector perpendicular to this 2D vector. The result is always rotated 90-degrees in a counter-clockwise direction for a 2D coordinate system where the positive Y axis goes up.")]
        public static CustomLogicVector2Builtin Perpendicular(CustomLogicVector2Builtin inDirection) => new CustomLogicVector2Builtin(Vector2.Perpendicular(inDirection._value));

        [CLMethod(description: "Reflects a vector off the surface defined by a normal.")]
        public static CustomLogicVector2Builtin Reflect(CustomLogicVector2Builtin inDirection, CustomLogicVector2Builtin inNormal) => new CustomLogicVector2Builtin(Vector2.Reflect(inDirection._value, inNormal._value));

        [CLMethod(description: "Multiplies two vectors component-wise.")]
        public static CustomLogicVector2Builtin Scale(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => new CustomLogicVector2Builtin(Vector2.Scale(a._value, b._value));

        [CLMethod(description: "Gets the signed angle in degrees between from and to.")]
        public static float SignedAngle(CustomLogicVector2Builtin from, CustomLogicVector2Builtin to) => Vector2.SignedAngle(from._value, to._value);

        [CLMethod(description: "Gradually changes a vector towards a desired goal over time.")]
        public static CustomLogicVector2Builtin SmoothDamp(CustomLogicVector2Builtin current, CustomLogicVector2Builtin target, ref CustomLogicVector2Builtin currentVelocity, float smoothTime, float maxSpeed) => new CustomLogicVector2Builtin(Vector2.SmoothDamp(current._value, target._value, ref currentVelocity._value, smoothTime, maxSpeed));
        #endregion
    }
}
