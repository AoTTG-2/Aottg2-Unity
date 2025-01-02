using System.Collections.Generic;
using System.Linq;

namespace CustomLogic
{
    [CLType(Name = "Set")]
    partial class CustomLogicSetBuiltin : BuiltinClassInstance
    {
        public HashSet<object> Set = new HashSet<object>();

        [CLConstructor]
        public CustomLogicSetBuiltin()
        {
        }

        [CLConstructor]
        public CustomLogicSetBuiltin(CustomLogicListBuiltin list)
        {
            foreach (var item in list.List)
            {
                Set.Add(item);
            }
        }

        [CLProperty(readOnly: true, description: "The number of elements in the set")]
        public int Count => Set.Count;
        [CLMethod(description: "Clear all set elements")]
        public void Clear()
        {
            Set.Clear();
        }
        [CLMethod(description: "Check if the set contains the specified element")]
        public bool Contains(object value)
        {
            return Set.Contains(value);
        }
        [CLMethod(description: "Add an element to the set")]
        public void Add(object value)
        {
            Set.Add(value);
        }
        [CLMethod(description: "Remove the element from the set")]
        public void Remove(object value)
        {
            Set.Remove(value);
        }

        [CLMethod(description: "Union with another set")]
        public void Union(CustomLogicSetBuiltin set)
        {
            Set.UnionWith(set.Set);
        }

        [CLMethod(description: "Intersect with another set")]
        public void Intersect(CustomLogicSetBuiltin set)
        {
            Set.IntersectWith(set.Set);
        }

        [CLMethod(description: "Difference with another set")]
        public void Difference(CustomLogicSetBuiltin set)
        {
            Set.ExceptWith(set.Set);
        }

        [CLMethod(description: "Check if the set is a subset of another set")]
        public bool IsSubsetOf(CustomLogicSetBuiltin set)
        {
            return Set.IsSubsetOf(set.Set);
        }

        [CLMethod(description: "Check if the set is a superset of another set")]
        public bool IsSupersetOf(CustomLogicSetBuiltin set)
        {
            return Set.IsSupersetOf(set.Set);
        }

        [CLMethod(description: "Check if the set is a proper subset of another set")]
        public bool IsProperSubsetOf(CustomLogicSetBuiltin set)
        {
            return Set.IsProperSubsetOf(set.Set);
        }

        [CLMethod(description: "Check if the set is a proper superset of another set")]
        public bool IsProperSupersetOf(CustomLogicSetBuiltin set)
        {
            return Set.IsProperSupersetOf(set.Set);
        }

        [CLMethod(description: "Check if the set overlaps with another set")]
        public bool Overlaps(CustomLogicSetBuiltin set)
        {
            return Set.Overlaps(set.Set);
        }

        [CLMethod(description: "Check if the set has the same elements as another set")]
        public bool SetEquals(CustomLogicSetBuiltin set)
        {
            return Set.SetEquals(set.Set);
        }

        [CLMethod(description: "Convert the set to a list")]
        public CustomLogicListBuiltin ToList()
        {
            return new CustomLogicListBuiltin(Set.ToList());
        }
    }
}
