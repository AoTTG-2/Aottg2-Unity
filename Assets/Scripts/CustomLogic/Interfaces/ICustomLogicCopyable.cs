namespace CustomLogic
{
    internal interface ICustomLogicCopyable
    {
        /// <summary>
        /// Overrides the assignment operator to create a deep copy of the object.
        /// </summary>
        /// <returns>A deep copy of the object.</returns>
        object __Copy__();
    }
}