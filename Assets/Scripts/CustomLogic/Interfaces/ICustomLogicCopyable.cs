namespace CustomLogic
{
    internal interface ICustomLogicCopyable
    {
        /// <summary>
        /// Override to deepcopy object on assignment, used for structs. Ex: copy = original is equivalent to copy = original.__Copy__()
        /// </summary>
        /// <returns>A deep copy of the object.</returns>
        object __Copy__();
    }
}