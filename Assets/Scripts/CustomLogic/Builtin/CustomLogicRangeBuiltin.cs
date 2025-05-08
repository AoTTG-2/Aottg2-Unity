namespace CustomLogic
{
    // todo: implement some kind of caching/pool for this
    // we really don't need to create a new list every time

    /// <summary>
    /// Inherits from List. Allows you to create lists of integers for convenient iteration, particularly in for loops.
    /// <code>
    /// for (a in Range(10))
    ///     Game.Print(a);
    ///     
    /// for (a in Range(1, 10))
    ///     Game.Print(a);
    ///     
    /// for (a in Range(1, 10, 2))
    ///     Game.Print(a);
    /// 
    /// </code>
    /// </summary>
    [CLType(Name = "Range", InheritBaseMembers = false)]
    partial class CustomLogicRangeBuiltin : CustomLogicListBuiltin
    {
        /// <summary>
        /// The constructor for the Range builtin class has multiple overloads.
        /// Range(n) creates a range from 0 to n-1.
        /// Range(a, n) creates a range from a to n-1.
        /// Range(a, n, step) creates a range from a to n-1 with the specified step.
        /// </summary>
        [CLConstructor]
        public CustomLogicRangeBuiltin(object[] parameterValues)
        {
            if (parameterValues.Length < 1)
                return;

            int start, end, step;

            if (parameterValues.Length == 1)
            {
                // Case for Range(n)
                start = 0;
                end = parameterValues[0].UnboxToInt();
                step = 1;
            }
            else if (parameterValues.Length == 2)
            {
                // Case for Range(a, n)
                start = parameterValues[0].UnboxToInt();
                end = parameterValues[1].UnboxToInt();
                step = 1;
            }
            else
            {
                // Case for Range(a, n, step)
                start = parameterValues[0].UnboxToInt();
                end = parameterValues[1].UnboxToInt();
                step = parameterValues[2].UnboxToInt();
            }

            if (step == 0)
                return;

            if (step > 0)
            {
                for (int i = start; i < end; i += step)
                    List.Add(i);
            }
            else
            {
                for (int i = start; i > end; i += step)
                    List.Add(i);
            }
        }
    }
}
