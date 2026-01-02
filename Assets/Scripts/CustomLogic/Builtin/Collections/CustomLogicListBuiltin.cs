using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomLogic
{
    /// <code>
    /// values = List(1,2,1,4,115);
    /// 
    /// # Common generic list operations Map, Filter, Reduce, and Sort are supported.
    /// # These methods take in a defined method with an expected signature and return type.
    /// 
    /// # Filter list to only unique values using set conversion.
    /// uniques = values.ToSet().ToList();
    /// 
    /// # Accumulate values in list using reduce.
    /// sum = values.Reduce(self.Sum2, 0);  # returns 123
    /// 
    /// # Filter list using predicate method.
    /// filteredList = values.Filter(self.Filter);  # returns List(115)
    /// 
    /// # Transform list using mapping method.
    /// newList = values.Map(self.TransformData);   # returns List(2,4,2,8,230)
    /// 
    /// function Sum2(a, b)
    /// {
    ///     return a + b;
    /// }
    /// 
    /// function Filter(a)
    /// {
    ///     return a > 20;
    /// }
    /// 
    /// function TransformData(a)
    /// {
    ///     return a * 2;
    /// }
    /// </code>
    [CLType(Name = "List", TypeParameters = new[] { "T" }, Description = "Ordered collection of objects.")]
    partial class CustomLogicListBuiltin : BuiltinClassInstance
    {
        public List<object> List = new List<object>();
        private readonly bool _isReadOnly;

        [CLConstructor("Creates an empty list.")]
        public CustomLogicListBuiltin()
        {
        }

        [CLConstructor("Creates a list with the specified values.")]
        public CustomLogicListBuiltin(
            [CLParam("The values to add to the list.")]
            params object[] parameterValues)
        {
            foreach (var item in parameterValues) List.Add(item);
        }

        public CustomLogicListBuiltin(IEnumerable<object> enumerable, bool isReadOnly = false)
        {
            List = new List<object>(enumerable);
            _isReadOnly = isReadOnly;
        }

        [CLProperty("The number of elements in the list.")]
        public int Count => List.Count;

        [CLMethod("Clear all list elements.")]
        public void Clear()
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            List.Clear();
        }

        [CLMethod(ReturnTypeArguments = new[] { "T" }, Description = "Get the element at the specified index.")]
        public object Get(
            [CLParam("The index of the element to get (negative indices count from the end).")]
            int index)
        {
            if (index < 0) index += List.Count;
            return List[index];
        }

        [CLMethod("Set the element at the specified index.")]
        public void Set(
            [CLParam("The index of the element to set (negative indices count from the end).")]
            int index,
            [CLParam("The value to set.", Type = "T")]
            object value)
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            if (index < 0) index += List.Count;
            List[index] = value;
        }

        [CLMethod("Add an element to the end of the list.")]
        public void Add(
            [CLParam("The element to add.", Type = "T")]
            object value)
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            List.Add(value);
        }

        [CLMethod("Insert an element at the specified index.")]
        public void InsertAt(
            [CLParam("The index at which to insert (negative indices count from the end).")]
            int index,
            [CLParam("The element to insert.", Type = "T")]
            object value)
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            if (index < 0) index += List.Count;
            List.Insert(index, value);
        }

        [CLMethod("Remove the element at the specified index.")]
        public void RemoveAt(
            [CLParam("The index of the element to remove (negative indices count from the end).")]
            int index)
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            if (index < 0) index += List.Count;
            List.RemoveAt(index);
        }

        [CLMethod("Remove the first occurrence of the specified element.")]
        public void Remove(
            [CLParam("The element to remove.", Type = "T")]
            object value)
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            List.Remove(value);
        }

        [CLMethod("Check if the list contains the specified element.")]
        public bool Contains(
            [CLParam("The element to check for.", Type = "T")]
            object value)
        {
            return List.Any(e => CustomLogicManager.Evaluator.CheckEquals(e, value));
        }

        [CLMethod("Sort the list.")]
        public void Sort()
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            List.Sort();
        }

        [CLMethod("Sort the list using a custom method, expects a method with the signature int method(a,b).")]
        public void SortCustom(
            [CLParam("The comparison method that returns an int: negative if a < b, 0 if a == b, positive if a > b.")]
            UserMethod method)
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            List.Sort((a, b) => (int)CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { a, b }));
        }

        // Add linq operations using UserMethod as the function

        [CLMethod(ReturnTypeArguments = new[] { "T" }, Description = "Filter the list using a custom method, expects a method with the signature bool method(element).")]
        public CustomLogicListBuiltin Filter(
            [CLParam("The predicate method that returns true for elements to keep.")]
            UserMethod method)
        {
            CustomLogicListBuiltin newList = new CustomLogicListBuiltin();
            newList.List = List.Where(e => (bool)CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { e })).ToList();
            return newList;
        }

        [CLMethod(ReturnTypeArguments = new[] { "T" }, Description = "Map the list using a custom method, expects a method with the signature object method(element).")]
        public CustomLogicListBuiltin Map(
            [CLParam("The transformation method that returns the mapped value for each element.")]
            UserMethod method)
        {
            CustomLogicListBuiltin newList = new CustomLogicListBuiltin();
            newList.List = List.Select(e => CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { e })).ToList();
            return newList;
        }

        [CLMethod(Description = "Reduce the list using a custom method, expects a method with the signature object method(acc, element).")]
        public object Reduce(
            [CLParam("The accumulation method that combines the accumulator with each element.")]
            UserMethod method,
            [CLParam("The initial accumulator value.", Type = "T")]
            object initialValue)
        {
            return List.Aggregate(initialValue, (acc, e) => CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { acc, e }));
        }

        [CLMethod(ReturnTypeArguments = new[] { "T" }, Description = "Returns a randomized version of the list.")]
        public CustomLogicListBuiltin Randomize()
        {
            CustomLogicListBuiltin newList = new CustomLogicListBuiltin();
            System.Random r = new System.Random();
            newList.List = List.OrderBy(x => (r.Next())).ToList();
            return newList;
        }

        [CLMethod(ReturnTypeArguments = new[] { "T" }, Description = "Convert the list to a set.")]
        public CustomLogicSetBuiltin ToSet()
        {
            CustomLogicSetBuiltin newSet = new CustomLogicSetBuiltin();
            foreach (var item in List) newSet.Set.Add(item);
            return newSet;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[");

            for (var i = 0; i < List.Count; i++)
            {
                if (List[i] is string str)
                    builder.Append($"\"{str}\"");
                else
                    builder.Append(List[i]);

                if (i != List.Count - 1)
                    builder.Append(", ");
            }

            builder.Append("]");
            return builder.ToString();
        }
    }
}
