using System.Collections.Generic;
using System.Text;

namespace CustomLogic
{
    // TODO: Make dict lookup constant time
    [CLType(Name = "Dict")]
    partial class CustomLogicDictBuiltin : BuiltinClassInstance
    {
        public Dictionary<object, object> Dict = new Dictionary<object, object>();

        [CLConstructor]
        public CustomLogicDictBuiltin()
        {
        }

        // Add CLProperties
        [CLProperty(description: "Number of elements in the dictionary")]
        public int Count => Dict.Count;

        [CLProperty(description: "Keys in the dictionary")]
        public CustomLogicListBuiltin Keys => new CustomLogicListBuiltin { List = new List<object>(Dict.Keys) };

        [CLProperty(description: "Values in the dictionary")]
        public CustomLogicListBuiltin Values => new CustomLogicListBuiltin { List = new List<object>(Dict.Values) };

        // Add CLMethods
        [CLMethod(description: "Clears the dictionary")]
        public void Clear()
        {
            Dict.Clear();
        }

        [CLMethod(description: "Gets a value from the dictionary")]
        public object Get(object key, object defaultValue = null)
        {
            if (Dict.ContainsKey(key))
                return Dict[key];
            return defaultValue;
        }

        [CLMethod(description: "Sets a value in the dictionary")]
        public void Set(object key, object value)
        {
            if (Dict.ContainsKey(key))
                Dict[key] = value;
            else
                Dict.Add(key, value);
        }

        [CLMethod(description: "Removes a value from the dictionary")]
        public void Remove(object key)
        {
            if (Dict.ContainsKey(key))
                Dict.Remove(key);
        }

        [CLMethod(description: "Checks if the dictionary contains a key")]
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
