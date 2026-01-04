using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomLogic
{
    /// <summary>
    /// Ordered collection of objects.
    /// </summary>
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
    [CLType(Name = "List", TypeParameters = new[] { "T" })]
    partial class CustomLogicListBuiltin : BuiltinClassInstance
    {
        public List<object> List = new List<object>();
        private readonly bool _isReadOnly;

        /// <summary>
        /// Creates an empty list.
        /// </summary>
        [CLConstructor]
        public CustomLogicListBuiltin()
        {
        }

        /// <summary>
        /// Creates a list with the specified values.
        /// </summary>
        /// <param name="parameterValues">The values to add to the list.</param>
        [CLConstructor]
        public CustomLogicListBuiltin(params object[] parameterValues)
        {
            foreach (var item in parameterValues) List.Add(item);
        }

        public CustomLogicListBuiltin(IEnumerable<object> enumerable, bool isReadOnly = false)
        {
            List = new List<object>(enumerable);
            _isReadOnly = isReadOnly;
        }

        /// <summary>
        /// The number of elements in the list.
        /// </summary>
        [CLProperty]
        public int Count => List.Count;

        /// <summary>
        /// Clear all list elements.
        /// </summary>
        [CLMethod]
        public void Clear()
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            List.Clear();
        }

        /// <summary>
        /// Get the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to get (negative indices count from the end).</param>
        [CLMethod(ReturnTypeArguments = new[] { "T" })]
        public object Get(int index)
        {
            if (index < 0) index += List.Count;
            return List[index];
        }

        /// <summary>
        /// Set the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to set (negative indices count from the end).</param>
        /// <param name="value">The value to set.</param>
        [CLMethod]
        public void Set(int index, object value)
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            if (index < 0) index += List.Count;
            List[index] = value;
        }

        /// <summary>
        /// Add an element to the end of the list.
        /// </summary>
        /// <param name="value">The element to add.</param>
        [CLMethod]
        public void Add(object value)
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            List.Add(value);
        }

        /// <summary>
        /// Insert an element at the specified index.
        /// </summary>
        /// <param name="index">The index at which to insert (negative indices count from the end).</param>
        /// <param name="value">The element to insert.</param>
        [CLMethod]
        public void InsertAt(int index, object value)
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            if (index < 0) index += List.Count;
            List.Insert(index, value);
        }

        /// <summary>
        /// Remove the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element to remove (negative indices count from the end).</param>
        [CLMethod]
        public void RemoveAt(int index)
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            if (index < 0) index += List.Count;
            List.RemoveAt(index);
        }

        /// <summary>
        /// Remove the first occurrence of the specified element.
        /// </summary>
        /// <param name="value">The element to remove.</param>
        [CLMethod]
        public void Remove(object value)
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            List.Remove(value);
        }

        /// <summary>
        /// Check if the list contains the specified element.
        /// </summary>
        /// <param name="value">The element to check for.</param>
        [CLMethod]
        public bool Contains(object value)
        {
            return List.Any(e => CustomLogicManager.Evaluator.CheckEquals(e, value));
        }

        /// <summary>
        /// Sort the list.
        /// </summary>
        [CLMethod]
        public void Sort()
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            List.Sort();
        }

        /// <summary>
        /// Sort the list using a custom method, expects a method with the signature int method(a,b).
        /// </summary>
        /// <param name="method">The comparison method that returns an int: negative if a &lt; b, 0 if a == b, positive if a &gt; b.</param>
        [CLMethod]
        public void SortCustom(UserMethod method)
        {
            if (_isReadOnly)
                throw new System.Exception("Cannot modify a read-only list.");
            List.Sort((a, b) => (int)CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { a, b }));
        }

        // Add linq operations using UserMethod as the function

        /// <summary>
        /// Filter the list using a custom method, expects a method with the signature bool method(element).
        /// </summary>
        /// <param name="method">The predicate method that returns true for elements to keep.</param>
        [CLMethod(ReturnTypeArguments = new[] { "T" })]
        public CustomLogicListBuiltin Filter(UserMethod method)
        {
            CustomLogicListBuiltin newList = new CustomLogicListBuiltin();
            newList.List = List.Where(e => (bool)CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { e })).ToList();
            return newList;
        }

        /// <summary>
        /// Map the list using a custom method, expects a method with the signature object method(element).
        /// </summary>
        /// <param name="method">The transformation method that returns the mapped value for each element.</param>
        [CLMethod(ReturnTypeArguments = new[] { "T" })]
        public CustomLogicListBuiltin Map(UserMethod method)
        {
            CustomLogicListBuiltin newList = new CustomLogicListBuiltin();
            newList.List = List.Select(e => CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { e })).ToList();
            return newList;
        }

        /// <summary>
        /// Reduce the list using a custom method, expects a method with the signature object method(acc, element).
        /// </summary>
        /// <param name="method">The accumulation method that combines the accumulator with each element.</param>
        /// <param name="initialValue">The initial accumulator value.</param>
        [CLMethod]
        public object Reduce(UserMethod method, object initialValue)
        {
            return List.Aggregate(initialValue, (acc, e) => CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { acc, e }));
        }

        /// <summary>
        /// Returns a randomized version of the list.
        /// </summary>
        [CLMethod(ReturnTypeArguments = new[] { "T" })]
        public CustomLogicListBuiltin Randomize()
        {
            CustomLogicListBuiltin newList = new CustomLogicListBuiltin();
            System.Random r = new System.Random();
            newList.List = List.OrderBy(x => (r.Next())).ToList();
            return newList;
        }

        /// <summary>
        /// Convert the list to a set.
        /// </summary>
        [CLMethod(ReturnTypeArguments = new[] { "T" })]
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
