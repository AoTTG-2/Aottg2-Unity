using System.Collections;
using System.Collections.Generic;
using SimpleJSONFixed;

namespace Settings
{
    class DictionarySetting<K, V>
        : TypedSetting<Dictionary<K, V>>,
            IEnumerable<KeyValuePair<K, V>>
        where K : BaseSetting, new()
        where V : BaseSetting, new()
    {
        public DictionarySetting(Dictionary<K, V> defaultValue)
        {
            DefaultValue = new Dictionary<K, V>(defaultValue);
            SetDefault();
        }

        public DictionarySetting()
        {
            DefaultValue = new Dictionary<K, V>();
            SetDefault();
        }

        public DictionarySetting(K defaultKey, V defaultValue)
        {
            DictionarySetting<K, V> settings = new DictionarySetting<K, V>();
            JSONNode json = defaultValue.SerializeToJsonObject();
        }

        public override void DeserializeFromJsonObject(JSONNode json)
        {
            Dictionary<K, V> settings = new Dictionary<K, V>();
            JSONObject obj = json.AsObject;

            foreach (KeyValuePair<string, JSONNode> node in obj)
            {
                K key = new K();
                V value = new V();

                value.DeserializeFromJsonObject(node.Value);
                key.DeserializeFromJsonObject(node.Key);

                settings.Add(key, value);
            }

            Value = settings;
        }

        public override JSONNode SerializeToJsonObject()
        {
            JSONObject obj = new JSONObject();

            foreach (KeyValuePair<K, V> kv in DefaultValue)
                obj.Add(kv.Key.SerializeToJsonString(), kv.Value.SerializeToJsonObject());

            return obj;
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach (KeyValuePair<K, V> kv in Value)
            {
                yield return kv;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<K, V>>)this).GetEnumerator();
        }
    }
}
