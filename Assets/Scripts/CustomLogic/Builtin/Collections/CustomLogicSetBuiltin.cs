using System.Collections.Generic;
using System.Linq;

namespace CustomLogic
{
    /// <summary>
    /// Collection of unique elements.
    /// </summary>
    [CLType(Name = "Set", TypeParameters = new[] { "T" })]
    partial class CustomLogicSetBuiltin : BuiltinClassInstance
    {
        public HashSet<object> Set = new HashSet<object>();

        /// <summary>
        /// Creates an empty set.
        /// </summary>
        [CLConstructor]
        public CustomLogicSetBuiltin()
        {
        }

        /// <summary>
        /// Creates a set with the specified values.
        /// </summary>
        /// <param name="parameterValues">The values to add to the set.</param>
        [CLConstructor]
        public CustomLogicSetBuiltin(params object[] parameterValues)
        {
            foreach (var item in parameterValues) Set.Add(item);
        }

        /// <summary>
        /// The number of elements in the set.
        /// </summary>
        [CLProperty(readOnly: true)]
        public int Count => Set.Count;
        /// <summary>
        /// Clear all set elements.
        /// </summary>
        [CLMethod]
        public void Clear()
        {
            Set.Clear();
        }
        /// <summary>
        /// Check if the set contains the specified element.
        /// </summary>
        /// <param name="value">The element to check for.</param>
        [CLMethod]
        public bool Contains(object value)
        {
            return Set.Contains(value);
        }
        /// <summary>
        /// Add an element to the set.
        /// </summary>
        /// <param name="value">The element to add.</param>
        [CLMethod]
        public void Add(object value)
        {
            Set.Add(value);
        }
        /// <summary>
        /// Remove the element from the set.
        /// </summary>
        /// <param name="value">The element to remove.</param>
        [CLMethod]
        public void Remove(object value)
        {
            Set.Remove(value);
        }

        /// <summary>
        /// Union with another set (adds all elements from the other set to this set).
        /// </summary>
        /// <param name="set">The set to union with.</param>
        [CLMethod]
        public void Union(CustomLogicSetBuiltin set)
        {
            Set.UnionWith(set.Set);
        }

        /// <summary>
        /// Intersect with another set (keeps only elements that are in both sets).
        /// </summary>
        /// <param name="set">The set to intersect with.</param>
        [CLMethod]
        public void Intersect(CustomLogicSetBuiltin set)
        {
            Set.IntersectWith(set.Set);
        }

        /// <summary>
        /// Difference with another set (removes all elements that are in the other set).
        /// </summary>
        /// <param name="set">The set to compute the difference with.</param>
        [CLMethod]
        public void Difference(CustomLogicSetBuiltin set)
        {
            Set.ExceptWith(set.Set);
        }

        /// <summary>
        /// Check if the set is a subset of another set.
        /// </summary>
        /// <param name="set">The set to check against.</param>
        [CLMethod]
        public bool IsSubsetOf(CustomLogicSetBuiltin set)
        {
            return Set.IsSubsetOf(set.Set);
        }

        /// <summary>
        /// Check if the set is a superset of another set.
        /// </summary>
        /// <param name="set">The set to check against.</param>
        [CLMethod]
        public bool IsSupersetOf(CustomLogicSetBuiltin set)
        {
            return Set.IsSupersetOf(set.Set);
        }

        /// <summary>
        /// Check if the set is a proper subset of another set (subset but not equal).
        /// </summary>
        /// <param name="set">The set to check against.</param>
        [CLMethod]
        public bool IsProperSubsetOf(CustomLogicSetBuiltin set)
        {
            return Set.IsProperSubsetOf(set.Set);
        }

        /// <summary>
        /// Check if the set is a proper superset of another set (superset but not equal).
        /// </summary>
        /// <param name="set">The set to check against.</param>
        [CLMethod]
        public bool IsProperSupersetOf(CustomLogicSetBuiltin set)
        {
            return Set.IsProperSupersetOf(set.Set);
        }

        /// <summary>
        /// Check if the set overlaps with another set (has at least one element in common).
        /// </summary>
        /// <param name="set">The set to check against.</param>
        [CLMethod]
        public bool Overlaps(CustomLogicSetBuiltin set)
        {
            return Set.Overlaps(set.Set);
        }

        /// <summary>
        /// Check if the set has the same elements as another set.
        /// </summary>
        /// <param name="set">The set to compare with.</param>
        [CLMethod]
        public bool SetEquals(CustomLogicSetBuiltin set)
        {
            return Set.SetEquals(set.Set);
        }

        /// <summary>
        /// Convert the set to a list.
        /// </summary>
        [CLMethod(ReturnTypeArguments = new[] { "T" })]
        public CustomLogicListBuiltin ToList()
        {
            CustomLogicListBuiltin newList = new CustomLogicListBuiltin();
            foreach (var item in Set) newList.Add(item);
            return newList;
        }
    }
}
