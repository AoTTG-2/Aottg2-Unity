using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomLogic
{
    /// <summary>
    /// List class for Custom Logic.
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
    /// function Sum2(a, b) {
    ///     return a + b;
    /// }
    /// 
    /// function Filter(a) {
    ///     return a > 20;
    /// }
    /// 
    /// function TransformData(a) {
    ///     return a * 2;
    /// }
    /// </code>
    /// </summary>
    [CLType(Name = "List")]
    partial class CustomLogicListBuiltin : BuiltinClassInstance
    {
        public List<object> List = new List<object>();

        [CLConstructor]
        public CustomLogicListBuiltin()
        {
        }

        [CLConstructor]
        public CustomLogicListBuiltin(object[] parameterValues)
        {
            foreach (var item in parameterValues)   List.Add(item);
        }

        [CLProperty(readOnly: true, description: "The number of elements in the list")]
        public int Count => List.Count;

        [CLMethod(description: "Clear all list elements")]
        public void Clear()
        {
            List.Clear();
        }

        [CLMethod(description: "Get the element at the specified index")]
        public object Get(int index)
        {
            if (index < 0) index += List.Count;
            return List[index];
        }

        [CLMethod(description: "Set the element at the specified index")]
        public void Set(int index, object value)
        {
            if (index < 0) index += List.Count;
            List[index] = value;
        }

        [CLMethod(description: "Add an element to the end of the list")]
        public void Add(object value)
        {
            List.Add(value);
        }

        [CLMethod(description: "Insert an element at the specified index")]
        public void InsertAt(int index, object value)
        {
            if (index < 0) index += List.Count;
            List.Insert(index, value);
        }

        [CLMethod(description: "Remove the element at the specified index")]
        public void RemoveAt(int index)
        {
            if (index < 0) index += List.Count;
            List.RemoveAt(index);
        }

        [CLMethod(description: "Remove the first occurrence of the specified element")]
        public void Remove(object value)
        {
            List.Remove(value);
        }

        [CLMethod(description: "Check if the list contains the specified element")]
        public bool Contains(object value)
        {
            return List.Any(e => CustomLogicManager.Evaluator.CheckEquals(e, value));
        }

        [CLMethod(description: "Sort the list")]
        public void Sort()
        {
            List.Sort();
        }

        [CLMethod(description: "Sort the list using a custom method, expects a method with the signature int method(a,b)")]
        public void SortCustom(UserMethod method)
        {
            List.Sort((a, b) => (int)CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { a, b }));
        }

        // Add linq operations using UserMethod as the function

        [CLMethod(description: "Filter the list using a custom method, expects a method with the signature bool method(element)")]
        public CustomLogicListBuiltin Filter(UserMethod method)
        {
            CustomLogicListBuiltin newList = new CustomLogicListBuiltin();
            newList.List = List.Where(e => (bool)CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { e })).ToList();
            return newList;
        }

        [CLMethod(description: "Map the list using a custom method, expects a method with the signature object method(element)")]
        public CustomLogicListBuiltin Map(UserMethod method)
        {
            CustomLogicListBuiltin newList = new CustomLogicListBuiltin();
            newList.List = List.Select(e => CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { e })).ToList();
            return newList;
        }

        [CLMethod(description: "Reduce the list using a custom method, expects a method with the signature object method(acc, element)")]
        public object Reduce(UserMethod method, object initialValue)
        {
            return List.Aggregate(initialValue, (acc, e) => CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { acc, e }));
        }

        [CLMethod(description: "Returns a randomized version of the list.")]
        public CustomLogicListBuiltin Randomize()
        {
            CustomLogicListBuiltin newList = new CustomLogicListBuiltin();
            System.Random r = new System.Random();
            newList.List = List.OrderBy(x => (r.Next())).ToList();
            return newList;
        }

        [CLMethod(description: "Convert the list to a set")]
        public CustomLogicSetBuiltin ToSet()
        {
            CustomLogicSetBuiltin newSet = new CustomLogicSetBuiltin();
            foreach (var item in List)  newSet.Set.Add(item);
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
