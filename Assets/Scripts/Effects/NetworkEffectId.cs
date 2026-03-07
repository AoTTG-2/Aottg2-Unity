using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Effects
{
    static class NetworkEffectId
    {
        private static readonly Dictionary<string, int> _nameToId = new Dictionary<string, int>();
        private static readonly Dictionary<int, string> _idToName = new Dictionary<int, string>();

        static NetworkEffectId()
        {
            foreach (var field in typeof(EffectPrefabs).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.FieldType == typeof(string))
                {
                    string value = (string)field.GetValue(null);
                    Register(value);
                }
            }
        }

        private static void Register(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;
            if (_nameToId.ContainsKey(name))
                return;
            int id = Animator.StringToHash(name);
            if (_idToName.ContainsKey(id))
            {
                Debug.LogError("NetworkEffectId hash collision: '" + name + "' and '" + _idToName[id] + "' both hash to " + id);
                return;
            }
            _nameToId[name] = id;
            _idToName[id] = name;
        }

        public static bool TryGetId(string name, out int id)
        {
            return _nameToId.TryGetValue(name, out id);
        }

        public static bool TryGetName(int id, out string name)
        {
            return _idToName.TryGetValue(id, out name);
        }
    }
}
