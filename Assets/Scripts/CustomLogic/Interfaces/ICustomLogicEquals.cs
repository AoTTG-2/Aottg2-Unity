namespace CustomLogic
{
    internal interface ICustomLogicEquals
    {
        /// <summary>
        /// Overrides the equality comparison, used for == and != operators. Ex: a == b is equivalent to a.__Eq__(a, b)
        /// </summary>
        /// <param name="self">Reference to self</param>
        /// <param name="other">Reference to other</param>
        /// <returns>Whether the objects are equal</returns>
        bool __Eq__(object self, object other);

        /// <summary>
        /// Overrides hashing, used for GetHashCode - Used for Dictionaries/Sets. Ex: hash = obj.GetHashCode() is equivalent to hash = obj.__Hash__()
        /// </summary>
        /// <returns>Returns an integer representing the hashcode.</returns>
        int __Hash__();
    }
}