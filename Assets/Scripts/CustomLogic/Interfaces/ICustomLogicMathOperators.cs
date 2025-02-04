namespace CustomLogic
{
    internal interface ICustomLogicMathOperators
    {
        /// <summary>
        /// Override to implement addition, used for + operator. Ex: a + b is equivalent to a.__Add__(a, b)
        /// </summary>
        /// <param name="self">Reference to self</param>
        /// <param name="other">Reference to other</param>
        /// <returns>The sum of self and other</returns>
        object __Add__(object self, object other);

        /// <summary>
        /// Override to implement subtraction, used for - operator. Ex: a - b is equivalent to a.__Sub__(a, b)
        /// </summary>
        /// <param name="self">Reference to self</param>
        /// <param name="other">Reference to other</param>
        /// <returns>The subtraction of self and other</returns>
        object __Sub__(object self, object other);

        /// <summary>
        /// Override to implement multiplication, used for * operator. Ex: a * b is equivalent to a.__Mul__(a, b)
        /// </summary>
        /// <param name="self">Reference to self</param>
        /// <param name="other">Reference to other</param>
        /// <returns>The multiplication of self and other</returns>
        object __Mul__(object self, object other);

        /// <summary>
        /// Override to implement division, used for / operator. Ex: a / b is equivalent to a.__Div__(a, b)
        /// </summary>
        /// <param name="self">Reference to self</param>
        /// <param name="other">Reference to other</param>
        /// <returns>The division of self and other</returns>
        object __Div__(object self, object other);
    }
}