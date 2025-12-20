using System.Collections.Generic;
using System.Linq;

namespace CustomLogic
{
    /// <summary>
    /// Collection of unique elements
    /// </summary>
    [CLType(Name = "Set", TypeParameters = new[] { "T" })]
    partial class CustomLogicSetBuiltin : BuiltinClassInstance
    {
        public HashSet<object> Set = new HashSet<object>();

        [CLConstructor]
        public CustomLogicSetBuiltin()
        {
        }

        [CLConstructor]
        public CustomLogicSetBuiltin(params object[] parameterValues)
        {
            foreach (var item in parameterValues) Set.Add(item);
        }

        [CLProperty(readOnly: true, description: "The number of elements in the set")]
        public int Count => Set.Count;
        [CLMethod(description: "Clear all set elements")]
        public void Clear()
        {
            Set.Clear();
        }
        [CLMethod(description: "Check if the set contains the specified element", ParameterTypeArguments = new[] { "T" })]
        public bool Contains(object value)
        {
            return Set.Contains(value);
        }
        [CLMethod(description: "Add an element to the set", ParameterTypeArguments = new[] { "T" })]
        public void Add(object value)
        {
            Set.Add(value);
        }
        [CLMethod(description: "Remove the element from the set", ParameterTypeArguments = new[] { "T" })]
        public void Remove(object value)
        {
            Set.Remove(value);
        }

        [CLMethod(description: "Union with another set", ParameterTypeArguments = new[] { "Set<T>" })]
        public void Union(CustomLogicSetBuiltin set)
        {
            Set.UnionWith(set.Set);
        }

        [CLMethod(description: "Intersect with another set", ParameterTypeArguments = new[] { "Set<T>" })]
        public void Intersect(CustomLogicSetBuiltin set)
        {
            Set.IntersectWith(set.Set);
        }

        [CLMethod(description: "Difference with another set", ParameterTypeArguments = new[] { "Set<T>" })]
        public void Difference(CustomLogicSetBuiltin set)
        {
            Set.ExceptWith(set.Set);
        }

        [CLMethod(description: "Check if the set is a subset of another set", ParameterTypeArguments = new[] { "Set<T>" })]
        public bool IsSubsetOf(CustomLogicSetBuiltin set)
        {
            return Set.IsSubsetOf(set.Set);
        }

        [CLMethod(description: "Check if the set is a superset of another set", ParameterTypeArguments = new[] { "Set<T>" })]
        public bool IsSupersetOf(CustomLogicSetBuiltin set)
        {
            return Set.IsSupersetOf(set.Set);
        }

        [CLMethod(description: "Check if the set is a proper subset of another set", ParameterTypeArguments = new[] { "Set<T>" })]
        public bool IsProperSubsetOf(CustomLogicSetBuiltin set)
        {
            return Set.IsProperSubsetOf(set.Set);
        }

        [CLMethod(description: "Check if the set is a proper superset of another set", ParameterTypeArguments = new[] { "Set<T>" })]
        public bool IsProperSupersetOf(CustomLogicSetBuiltin set)
        {
            return Set.IsProperSupersetOf(set.Set);
        }

        [CLMethod(description: "Check if the set overlaps with another set", ParameterTypeArguments = new[] { "Set<T>" })]
        public bool Overlaps(CustomLogicSetBuiltin set)
        {
            return Set.Overlaps(set.Set);
        }

        [CLMethod(description: "Check if the set has the same elements as another set", ParameterTypeArguments = new[] { "Set<T>" })]
        public bool SetEquals(CustomLogicSetBuiltin set)
        {
            return Set.SetEquals(set.Set);
        }

        [CLMethod(description: "Convert the set to a list", ReturnTypeArguments = new[] { "T" })]
        public CustomLogicListBuiltin ToList()
        {
            CustomLogicListBuiltin newList = new CustomLogicListBuiltin();
            foreach (var item in Set) newList.Add(item);
            return newList;
        }
    }
}
