using System.Collections.Generic;
using System.Linq;

namespace CustomLogic
{
    [CLType(Name = "Set", TypeParameters = new[] { "T" }, Description = "Collection of unique elements.")]
    partial class CustomLogicSetBuiltin : BuiltinClassInstance
    {
        public HashSet<object> Set = new HashSet<object>();

        [CLConstructor("Creates an empty set.")]
        public CustomLogicSetBuiltin()
        {
        }

        [CLConstructor("Creates a set with the specified values.")]
        public CustomLogicSetBuiltin(
            [CLParam("The values to add to the set.")]
            params object[] parameterValues)
        {
            foreach (var item in parameterValues) Set.Add(item);
        }

        [CLProperty(readOnly: true, Description = "The number of elements in the set.")]
        public int Count => Set.Count;
        [CLMethod("Clear all set elements.")]
        public void Clear()
        {
            Set.Clear();
        }
        [CLMethod("Check if the set contains the specified element.")]
        public bool Contains(
            [CLParam("The element to check for.", Type = "T")]
            object value)
        {
            return Set.Contains(value);
        }
        [CLMethod("Add an element to the set.")]
        public void Add(
            [CLParam("The element to add.", Type = "T")]
            object value)
        {
            Set.Add(value);
        }
        [CLMethod("Remove the element from the set.")]
        public void Remove(
            [CLParam("The element to remove.", Type = "T")]
            object value)
        {
            Set.Remove(value);
        }

        [CLMethod("Union with another set (adds all elements from the other set to this set).")]
        public void Union(
            [CLParam("The set to union with.", Type = "Set<T>")]
            CustomLogicSetBuiltin set)
        {
            Set.UnionWith(set.Set);
        }

        [CLMethod("Intersect with another set (keeps only elements that are in both sets).")]
        public void Intersect(
            [CLParam("The set to intersect with.", Type = "Set<T>")]
            CustomLogicSetBuiltin set)
        {
            Set.IntersectWith(set.Set);
        }

        [CLMethod("Difference with another set (removes all elements that are in the other set).")]
        public void Difference(
            [CLParam("The set to compute the difference with.", Type = "Set<T>")]
            CustomLogicSetBuiltin set)
        {
            Set.ExceptWith(set.Set);
        }

        [CLMethod("Check if the set is a subset of another set.")]
        public bool IsSubsetOf(
            [CLParam("The set to check against.", Type = "Set<T>")]
            CustomLogicSetBuiltin set)
        {
            return Set.IsSubsetOf(set.Set);
        }

        [CLMethod("Check if the set is a superset of another set.")]
        public bool IsSupersetOf(
            [CLParam("The set to check against.", Type = "Set<T>")]
            CustomLogicSetBuiltin set)
        {
            return Set.IsSupersetOf(set.Set);
        }

        [CLMethod("Check if the set is a proper subset of another set (subset but not equal).")]
        public bool IsProperSubsetOf(
            [CLParam("The set to check against.", Type = "Set<T>")]
            CustomLogicSetBuiltin set)
        {
            return Set.IsProperSubsetOf(set.Set);
        }

        [CLMethod("Check if the set is a proper superset of another set (superset but not equal).")]
        public bool IsProperSupersetOf(
            [CLParam("The set to check against.", Type = "Set<T>")]
            CustomLogicSetBuiltin set)
        {
            return Set.IsProperSupersetOf(set.Set);
        }

        [CLMethod("Check if the set overlaps with another set (has at least one element in common).")]
        public bool Overlaps(
            [CLParam("The set to check against.", Type = "Set<T>")]
            CustomLogicSetBuiltin set)
        {
            return Set.Overlaps(set.Set);
        }

        [CLMethod("Check if the set has the same elements as another set.")]
        public bool SetEquals(
            [CLParam("The set to compare with.", Type = "Set<T>")]
            CustomLogicSetBuiltin set)
        {
            return Set.SetEquals(set.Set);
        }

        [CLMethod(ReturnTypeArguments = new[] { "T" }, Description = "Convert the set to a list.")]
        public CustomLogicListBuiltin ToList()
        {
            CustomLogicListBuiltin newList = new CustomLogicListBuiltin();
            foreach (var item in Set) newList.Add(item);
            return newList;
        }
    }
}
