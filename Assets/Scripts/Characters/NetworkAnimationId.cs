using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Characters
{
    static class NetworkAnimationId
    {
        private static readonly Dictionary<string, int> _nameToId = new Dictionary<string, int>();
        private static readonly Dictionary<int, string> _idToName = new Dictionary<int, string>();

        static NetworkAnimationId()
        {
            RegisterStaticStringFields(typeof(BasicTitanAnimations));
            RegisterStaticStringFields(typeof(AnnieAnimations));
            RegisterStaticStringFields(typeof(ErenAnimations));
            RegisterStaticStringFields(typeof(WallColossalAnimations));
            RegisterStaticStringFields(typeof(HumanAnimations));
            RegisterStaticStringFields(typeof(HorseAnimations));
            RegisterStaticStringFields(typeof(TitanSounds));
            RegisterStaticStringFields(typeof(HumanSounds));
            // ArmoredAnimations uses property getters with inline strings
            Register("Amarture_VER2|dt_idle");
            Register("Amarture_VER2|dt_run");
            Register("Amarture_VER2|dt_die");
            Register("Amarture_VER2|dt_exhaust");
            Register("Amarture_VER2|dt_attack_swipe_L");
        }

        private static void RegisterStaticStringFields(System.Type type)
        {
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
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
                Debug.LogError("NetworkAnimationId hash collision: '" + name + "' and '" + _idToName[id] + "' both hash to " + id);
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
