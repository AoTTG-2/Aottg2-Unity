namespace CustomLogic
{
    internal interface ICustomLogicToString
    {
        /// <summary>
        /// Override to implement the string representation of the object, used for Print() function. Ex: Print(obj) will use obj.__Str__()
        /// </summary>
        /// <returns>A string representation of the object</returns>
        string __Str__();
    }
}