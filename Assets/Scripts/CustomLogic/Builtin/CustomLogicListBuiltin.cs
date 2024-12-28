using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomLogic
{
    [CLType(InheritBaseMembers = true)]
    class CustomLogicListBuiltin : CustomLogicClassInstanceBuiltin
    {
        public List<object> List = new List<object>();

        public CustomLogicListBuiltin(string type): base(type)
        {
        }

        public CustomLogicListBuiltin(): base("List")
        {
        }

        public CustomLogicListBuiltin(List<object> list) : base("List")
        {
            List = list;
        }

        public CustomLogicListBuiltin(CustomLogicSetBuiltin set) : base("List")
        {
            List = set.Set.ToList();
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
            return List[index];
        }

        [CLMethod(description: "Set the element at the specified index")]
        public void Set(int index, object value)
        {
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
            List.Insert(index, value);
        }

        [CLMethod(description: "Remove the element at the specified index")]
        public void RemoveAt(int index)
        {
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
            List.Sort((a, b) => (int)CustomLogicManager.Evaluator.EvaluateMethod(method, new List<object>() { a, b }));
        }

        // Add linq operations using UserMethod as the function

        [CLMethod(description: "Filter the list using a custom method, expects a method with the signature bool method(element)")]
        public CustomLogicListBuiltin Filter(UserMethod method)
        {
            var newList = List.Where(e => (bool)CustomLogicManager.Evaluator.EvaluateMethod(method, new List<object>() { e })).ToList();
            return new CustomLogicListBuiltin(newList);
        }

        [CLMethod(description: "Map the list using a custom method, expects a method with the signature object method(element)")]
        public CustomLogicListBuiltin Map(UserMethod method)
        {
            var newList = List.Select(e => CustomLogicManager.Evaluator.EvaluateMethod(method, new List<object>() { e })).ToList();
            return new CustomLogicListBuiltin(newList);
        }

        [CLMethod(description: "Reduce the list using a custom method, expects a method with the signature object method(acc, element)")]
        public object Reduce(UserMethod method, object initialValue)
        {
            return List.Aggregate(initialValue, (acc, e) => CustomLogicManager.Evaluator.EvaluateMethod(method, new List<object>() { acc, e }));
        }

        [CLMethod(description: "Randomize the list")]
        public void Randomize()
        {
            System.Random r = new System.Random();
            List = List.OrderBy(x => (r.Next())).ToList();
        }

        [CLMethod(description: "Convert the list to a set")]
        public CustomLogicSetBuiltin ToSet()
        {
            return new CustomLogicSetBuiltin(this);
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
