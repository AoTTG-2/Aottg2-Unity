using System.Collections.Generic;
using System.Text;

namespace CustomLogic
{
    /// <summary>
    /// Collection of key-value pairs. Keys must be unique.
    /// </summary>
    [CLType(Name = "Dict", TypeParameters = new[] { "K", "V" })]
    partial class CustomLogicDictBuiltin : BuiltinClassInstance
    {
        private readonly Dictionary<object, object> _dict;

        private CustomLogicListBuiltin _cachedKeys;
        private CustomLogicListBuiltin _cachedValues;

        private int _version;
        private int _listsVersion;

        /// <summary>
        /// Creates an empty dictionary.
        /// </summary>
        [CLConstructor]
        public CustomLogicDictBuiltin()
        {
            _dict = new Dictionary<object, object>();
        }

        /// <summary>
        /// Creates a dictionary with the specified capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity of the dictionary.</param>
        [CLConstructor]
        public CustomLogicDictBuiltin(int capacity)
        {
            _dict = new Dictionary<object, object>(capacity);
        }

        /// <summary>
        /// Number of elements in the dictionary.
        /// </summary>
        [CLProperty]
        public int Count => _dict.Count;

        /// <summary>
        /// Keys snapshot. Returns a stable snapshot list of all keys. 
        /// The returned list is read-only - any attempt to modify it will throw an exception. 
        /// The snapshot remains unchanged even if the dictionary is mutated later. 
        /// After dictionary mutations, accessing Keys again creates a new snapshot object. 
        /// Access is O(1) when the dictionary has not changed. 
        /// Rebuild after mutations is O(n) and allocates new snapshot objects.
        /// </summary>
        [CLProperty(TypeArguments = new[] { "K" })]
        public CustomLogicListBuiltin Keys
        {
            get
            {
                EnsureListsUpToDate();
                return _cachedKeys;
            }
        }

        /// <summary>
        /// Values snapshot. Returns a stable snapshot list of all values. 
        /// The returned list is read-only - any attempt to modify it will throw an exception. 
        /// The snapshot remains unchanged even if the dictionary is mutated later. 
        /// After dictionary mutations (Set/Remove/Clear), accessing Values again creates a new snapshot object. 
        /// Access is O(1) when the dictionary has not changed. 
        /// Rebuild after mutations is O(n) and allocates new snapshot objects.
        /// </summary>
        [CLProperty(TypeArguments = new[] { "V" })]
        public CustomLogicListBuiltin Values
        {
            get
            {
                EnsureListsUpToDate();
                return _cachedValues;
            }
        }

        /// <summary>
        /// Clears the dictionary.
        /// </summary>
        [CLMethod]
        public void Clear()
        {
            if (_dict.Count == 0) return;
            _dict.Clear();
            InvalidateCache();
        }

        /// <summary>
        /// Gets a value from the dictionary.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="defaultValue">The value to return if the key is not found.</param>
        /// <returns>The value associated with the key, or defaultValue if the key is not found. If the stored value is null, Get returns null (even if defaultValue is provided).</returns>
        [CLMethod(ReturnTypeArguments = new[] { "V" })]
        public object Get(object key, object defaultValue = null)
        {
            return _dict.TryGetValue(key, out var value) ? value : defaultValue;
        }

        /// <summary>
        /// Sets the value for the key. Overwrites the existing value if the key is already present. Do not mutate key objects after inserting.
        /// </summary>
        /// <param name="key">The key of the value to set.</param>
        /// <param name="value">The value to set.</param>
        [CLMethod]
        public void Set(object key, object value)
        {
            _dict[key] = value;
            InvalidateCache();
        }

        /// <summary>
        /// Removes a value from the dictionary.
        /// </summary>
        /// <param name="key">The key of the value to remove.</param>
        [CLMethod]
        public void Remove(object key)
        {
            if (_dict.Remove(key))
                InvalidateCache();
        }

        /// <summary>
        /// Checks if the dictionary contains a key.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the dictionary contains the key, false otherwise.</returns>
        [CLMethod]
        public bool Contains(object key)
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
