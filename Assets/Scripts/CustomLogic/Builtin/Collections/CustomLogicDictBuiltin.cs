using System.Collections.Generic;
using System.Text;

namespace CustomLogic
{
    // TODO: Make dict lookup constant time
    // TODO: Keys and Values does not need to create a new list every time
    /// <summary>
    /// Collection of key-value pairs.
    /// Keys must be unique.
    /// </summary>
    [CLType(Name = "Dict", TypeParameters = new[] { "K", "V" })]
    partial class CustomLogicDictBuiltin : BuiltinClassInstance
    {
        public readonly Dictionary<object, object> Dict;

        /// <summary>
        /// Creates an empty dictionary
        /// </summary>
        [CLConstructor]
        public CustomLogicDictBuiltin()
        {
            Dict = new Dictionary<object, object>();
        }

        /// <summary>
        /// Creates a dictionary with the specified capacity
        /// </summary>
        [CLConstructor]
        public CustomLogicDictBuiltin(int capacity)
        {
            Dict = new Dictionary<object, object>(capacity);
        }

        [CLProperty(description: "Number of elements in the dictionary")]
        public int Count => Dict.Count;

        [CLProperty(description: "Keys in the dictionary", TypeArguments = new[] { "K" })]
        public CustomLogicListBuiltin Keys => new CustomLogicListBuiltin { List = new List<object>(Dict.Keys) };

        [CLProperty(description: "Values in the dictionary", TypeArguments = new[] { "V" })]
        public CustomLogicListBuiltin Values => new CustomLogicListBuiltin { List = new List<object>(Dict.Values) };

        [CLMethod(description: "Clears the dictionary")]
        public void Clear()
        {
            Dict.Clear();
        }

        /// <summary>
        /// Gets a value from the dictionary
        /// </summary>
        /// <param name="key">The key of the value to get</param>
        /// <param name="defaultValue">The value to return if the key is not found</param>
        /// <returns>The value associated with the key, or the default value if the key is not found</returns>
        [CLMethod(ReturnTypeArguments = new[] { "V" }, ParameterTypeArguments = new[] { "K", "V" })]
        public object Get(object key, object defaultValue = null)
        {
            if (Dict.ContainsKey(key))
                return Dict[key];
            return defaultValue;
        }

        /// <summary>
        /// Sets a value in the dictionary
        /// </summary>
        /// <param name="key">The key of the value to set</param>
        /// <param name="value">The value to set</param>
        [CLMethod(ParameterTypeArguments = new[] { "K", "V" })]
        public void Set(object key, object value)
        {
            if (Dict.ContainsKey(key))
                Dict[key] = value;
            else
                Dict.Add(key, value);
        }

        /// <summary>
        /// Removes a value from the dictionary
        /// </summary>
        /// <param name="key">The key of the value to remove</param>
        [CLMethod(ParameterTypeArguments = new[] { "K" })]
        public void Remove(object key)
        {
            if (Dict.ContainsKey(key))
                Dict.Remove(key);
        }

        /// <summary>
        /// Checks if the dictionary contains a key
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>True if the dictionary contains the key, false otherwise</returns>
        [CLMethod(ParameterTypeArguments = new[] { "K" })]
        public bool Contains(object key)
        {
            return Dict.ContainsKey(key);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("{");

            var i = 0;
            foreach (var (key, value) in Dict)
            {
                Append(key);
                builder.Append(": ");
                Append(value);

                if (i != Dict.Count - 1)
                    builder.Append(", ");

                i++;
            }

            builder.Append("}");
            return builder.ToString();

            void Append(object obj)
            {
                if (obj is string str)
                {
                    builder.Append($"\"{str}\"");
                    return;
                }

                builder.Append(obj);
            }
        }
    }
}
