namespace CustomLogic
{
    // todo: implement some kind of caching/pool for this
    // we really don't need to create a new list every time

    /// <summary>
    /// Range allows you to create lists of integers for convenient iteration, particularly in for loops.
    /// </summary>
    /// <code>
    /// for (a in Range(10))
    /// {
    ///     Game.Print(a);
    /// }
    ///     
    /// for (a in Range(1, 10))
    /// {
    ///     Game.Print(a);
    /// }
    ///     
    /// for (a in Range(1, 10, 2))
    /// {
    ///     Game.Print(a);
    /// }
    /// </code>
    [CLType(Name = "Range", InheritBaseMembers = false)]
    partial class CustomLogicRangeBuiltin : CustomLogicListBuiltin
    {
        /// <summary>
        /// Creates a range from 0 to end-1
        /// </summary>
        [CLConstructor]
        public CustomLogicRangeBuiltin(int end) : this(0, end, 1) { }

        /// <summary>
        /// Creates a range from start to end-1
        /// </summary>
        [CLConstructor]
        public CustomLogicRangeBuiltin(int start, int end) : this(start, end, 1) { }

        /// <summary>
        /// Creates a range from start to end-1 with the specified step
        /// </summary>
        [CLConstructor]
        public CustomLogicRangeBuiltin(int start, int end, int step)
        {
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
