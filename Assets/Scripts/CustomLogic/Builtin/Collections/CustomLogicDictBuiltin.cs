using System.Collections.Generic;
using System.Text;

namespace CustomLogic
{
    [CLType(Name = "Dict", TypeParameters = new[] { "K", "V" }, Description = "Collection of key-value pairs. Keys must be unique.")]
    partial class CustomLogicDictBuiltin : BuiltinClassInstance
    {
        private readonly Dictionary<object, object> _dict;

        private CustomLogicListBuiltin _cachedKeys;
        private CustomLogicListBuiltin _cachedValues;

        private int _version;
        private int _listsVersion;

        [CLConstructor("Creates an empty dictionary.")]
        public CustomLogicDictBuiltin()
        {
            _dict = new Dictionary<object, object>();
        }

        [CLConstructor("Creates a dictionary with the specified capacity.")]
        public CustomLogicDictBuiltin(
            [CLParam("The initial capacity of the dictionary.")]
            int capacity)
        {
            _dict = new Dictionary<object, object>(capacity);
        }

        [CLProperty("Number of elements in the dictionary.")]
        public int Count => _dict.Count;

        [CLProperty(TypeArguments = new[] { "K" }, Description = "Keys snapshot. Returns a stable snapshot list of all keys. The returned list is read-only - any attempt to modify it will throw an exception. The snapshot remains unchanged even if the dictionary is mutated later. After dictionary mutations, accessing Keys again creates a new snapshot object. Access is O(1) when the dictionary has not changed. Rebuild after mutations is O(n) and allocates new snapshot objects. Calling Keys/Values after frequent mutations will allocate.")]
        public CustomLogicListBuiltin Keys
        {
            get
            {
                EnsureListsUpToDate();
                return _cachedKeys;
            }
        }

        [CLProperty(TypeArguments = new[] { "V" }, Description = "Values snapshot. Returns a stable snapshot list of all values. The returned list is read-only - any attempt to modify it will throw an exception. The snapshot remains unchanged even if the dictionary is mutated later. After dictionary mutations (Set/Remove/Clear), accessing Values again creates a new snapshot object. Access is O(1) when the dictionary has not changed. Rebuild after mutations is O(n) and allocates new snapshot objects. Calling Keys/Values after frequent mutations will allocate.")]
        public CustomLogicListBuiltin Values
        {
            get
            {
                EnsureListsUpToDate();
                return _cachedValues;
            }
        }

        [CLMethod("Clears the dictionary.")]
        public void Clear()
        {
            if (_dict.Count == 0) return;
            _dict.Clear();
            InvalidateCache();
        }

        [CLMethod(ReturnTypeArguments = new[] { "V" }, Description = "Gets a value from the dictionary. Returns the value associated with the key, or defaultValue if the key is not found. If the stored value is null, Get returns null (even if defaultValue is provided).")]
        public object Get(
            [CLParam("The key of the value to get", Type = "K")]
            object key,
            [CLParam("The value to return if the key is not found", Type = "V")]
            object defaultValue = null)
        {
            return _dict.TryGetValue(key, out var value) ? value : defaultValue;
        }

        [CLMethod(Description = "Sets the value for the key. Overwrites the existing value if the key is already present. Do not mutate key objects after inserting.")]
        public void Set(
            [CLParam("The key of the value to set", Type = "K")]
            object key,
            [CLParam("The value to set", Type = "V")]
            object value)
        {
            _dict[key] = value;
            InvalidateCache();
        }

        [CLMethod("Removes a value from the dictionary.")]
        public void Remove(
            [CLParam("The key of the value to remove", Type = "K")]
            object key)
        {
            if (_dict.Remove(key))
                InvalidateCache();
        }

        [CLMethod(Description = "Checks if the dictionary contains a key. Returns: True if the dictionary contains the key, false otherwise.")]
        public bool Contains(
            [CLParam("The key to check", Type = "K")]
            object key)
        {
            return _dict.ContainsKey(key);
        }

        public override string ToString()
        {
            if (_dict.Count == 0) return "{}";

            var builder = new StringBuilder();
            builder.Append("{");

            var i = 0;
            var count = _dict.Count;
            foreach (var kvp in _dict)
            {
                Append(kvp.Key);
                builder.Append(": ");
                Append(kvp.Value);

                if (i < count - 1) builder.Append(", ");
                i++;
            }

            builder.Append("}");
            return builder.ToString();

            void Append(object obj)
            {
                if (obj is string str)
                {
                    builder.Append('"').Append(str).Append('"');
                    return;
                }

                builder.Append(obj);
            }
        }

        private void InvalidateCache()
        {
            unchecked { _version++; }
        }

        private void EnsureListsUpToDate()
        {
            if (_listsVersion == _version && _cachedKeys != null) return;

            // Rebuild as NEW snapshots to avoid mutation, so previously returned Keys/Values references stay stable.
            // Create readonly lists to prevent mutations and enforce snapshot semantics.
            _cachedKeys = new CustomLogicListBuiltin(_dict.Keys, true);
            _cachedValues = new CustomLogicListBuiltin(_dict.Values, true);
            _listsVersion = _version;
        }
    }
}
