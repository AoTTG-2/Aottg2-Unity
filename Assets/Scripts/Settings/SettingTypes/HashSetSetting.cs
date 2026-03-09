using System.Collections.Generic;
using SimpleJSONFixed;

namespace Settings
{
    class HashSetSetting<T> : TypedSetting<HashSet<T>>
    {
        public HashSetSetting(HashSet<T> defaultValue) : base(defaultValue)
        {
        }

        public HashSetSetting()
        {
            DefaultValue = new HashSet<T>();
            SetDefault();
        }

        public override void SetDefault()
        {
            Value = new HashSet<T>(DefaultValue);
        }

        public override void DeserializeFromJsonObject(JSONNode json)
        {
            HashSet<T> newValue = new HashSet<T>();
            JSONArray array = json.AsArray;
            foreach (JSONNode node in array)
            {
                if (typeof(T) == typeof(int))
                    newValue.Add((T)(object)node.AsInt);
                else if (typeof(T) == typeof(string))
                    newValue.Add((T)(object)node.Value);
            }
            Value = newValue;
        }

        public override JSONNode SerializeToJsonObject()
        {
            JSONArray array = new JSONArray();
            foreach (T item in Value)
            {
                if (typeof(T) == typeof(int))
                    array.Add((int)(object)item);
                else if (typeof(T) == typeof(string))
                    array.Add((string)(object)item);
            }
            return array;
        }

        public bool Contains(T item)
        {
            return Value.Contains(item);
        }

        public void Add(T item)
        {
            Value.Add(item);
        }

        public void Remove(T item)
        {
            Value.Remove(item);
        }

        public void Toggle(T item)
        {
            if (Value.Contains(item))
                Value.Remove(item);
            else
                Value.Add(item);
        }

        public void Clear()
        {
            Value.Clear();
        }
    }
}
