namespace CustomLogic
{
    internal interface ICustomLogicEquals
    {
        bool __Eq__(object self, object other);
        int __Hash__();
    }
}