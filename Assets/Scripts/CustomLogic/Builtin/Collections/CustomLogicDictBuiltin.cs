using System.Collections.Generic;
using System.Text;

namespace CustomLogic
{
    // TODO: Make dict lookup constant time
    // TODO: Keys and Values does not need to create a new list every time
    [CLType(Name = "Dict", TypeParameters = new[] { "K", "V" }, Description = "Collection of key-value pairs. Keys must be unique.")]
    partial class CustomLogicDictBuiltin : BuiltinClassInstance
    {
        public readonly Dictionary<object, object> Dict;

        [CLConstructor("Creates an empty dictionary.")]
        public CustomLogicDictBuiltin()
        {
            Dict = new Dictionary<object, object>();
        }

        [CLConstructor("Creates a dictionary with the specified capacity.")]
        public CustomLogicDictBuiltin(
            [CLParam("The initial capacity of the dictionary.")]
            int capacity)
        {
            Dict = new Dictionary<object, object>(capacity);
        }

        [CLProperty("Number of elements in the dictionary.")]
        public int Count => Dict.Count;

        [CLProperty(TypeArguments = new[] { "K" }, Description = "Keys in the dictionary.")]
        public CustomLogicListBuiltin Keys => new CustomLogicListBuiltin { List = new List<object>(Dict.Keys) };

        [CLProperty(TypeArguments = new[] { "V" }, Description = "Values in the dictionary.")]
        public CustomLogicListBuiltin Values => new CustomLogicListBuiltin { List = new List<object>(Dict.Values) };

        [CLMethod("Clears the dictionary.")]
        public void Clear()
        {
            Dict.Clear();
        }

        [CLMethod(ReturnTypeArguments = new[] { "V" }, Description = "Gets a value from the dictionary. Returns: The value associated with the key, or the default value if the key is not found.")]
        public object Get(
            [CLParam("The key of the value to get", Type = "K")]
            object key,
            [CLParam("The value to return if the key is not found", Type = "V")]
            object defaultValue = null)
        {
            if (Dict.ContainsKey(key))
                return Dict[key];
            return defaultValue;
        }

        [CLMethod("Sets a value in the dictionary.")]
        public void Set(
            [CLParam("The key of the value to set", Type = "K")]
            object key,
            [CLParam("The value to set", Type = "V")]
            object value)
        {
            if (Dict.ContainsKey(key))
                Dict[key] = value;
            else
                Dict.Add(key, value);
        }

        [CLMethod("Removes a value from the dictionary.")]
        public void Remove(
            [CLParam("The key of the value to remove", Type = "K")]
            object key)
        {
            if (Dict.ContainsKey(key))
                Dict.Remove(key);
        }

        [CLMethod(Description = "Checks if the dictionary contains a key. Returns: True if the dictionary contains the key, false otherwise.")]
        public bool Contains(
            [CLParam("The key to check", Type = "K")]
            object key)
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
