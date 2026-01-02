namespace CustomLogic
{
    // todo: implement some kind of caching/pool for this
    // we really don't need to create a new list every time

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
    [CLType(Name = "Range", InheritBaseMembers = false, Description = "Range allows you to create lists of integers for convenient iteration, particularly in for loops.")]
    partial class CustomLogicRangeBuiltin : CustomLogicListBuiltin
    {
        [CLConstructor("Creates a range from 0 to end-1.")]
        public CustomLogicRangeBuiltin(
            [CLParam("The end value (exclusive).")]
            int end) : this(0, end, 1) { }

        [CLConstructor("Creates a range from start to end-1.")]
        public CustomLogicRangeBuiltin(
            [CLParam("The start value.")]
            int start,
            [CLParam("The end value (exclusive).")]
            int end) : this(start, end, 1) { }

        [CLConstructor("Creates a range from start to end-1 with the specified step.")]
        public CustomLogicRangeBuiltin(
            [CLParam("The start value.")]
            int start,
            [CLParam("The end value (exclusive).")]
            int end,
            [CLParam("The step value.")]
            int step)
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
