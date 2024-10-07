using System.Collections.Generic;
using SimpleJSONFixed;
using UnityEngine;

namespace Settings
{
    class KeybindSetting: BaseSetting
    {
        public List<InputKey> InputKeys = new List<InputKey>();
        protected string[] _defaultKeyStrings;

        public KeybindSetting(string[] defaultKeyStrings)
        {
            _defaultKeyStrings = defaultKeyStrings;
            SetDefault();
        }

        public bool ContainsEnter()
        {
            return Contains(KeyCode.KeypadEnter) || Contains(KeyCode.Return);
        }

        public override void SetDefault()
        {
            LoadFromStringArray(_defaultKeyStrings);
        }

        protected void LoadFromStringArray(string[] keyStrings)
        {
            InputKeys.Clear();
            foreach (string keyStr in keyStrings)
            {
                InputKey key = new InputKey(keyStr);
                InputKeys.Add(key);
            }
        }

        public override string ToString()
        {
            List<string> keys = new List<string>();
            foreach (InputKey key in InputKeys)
            {
                if (!key.IsNone())
                    keys.Add(key.ToString());
            }
            if (keys.Count == 0)
                return "None";
            return string.Join(" / ", keys.ToArray());
        }

        public bool Contains(InputKey key)
        {
            foreach (InputKey inputKey in InputKeys)
            {
                if (inputKey.Equals(key))
                    return true;
            }
            return false;
        }

        public bool Contains(KeyCode key)
        {
            foreach (InputKey inputKey in InputKeys)
            {
                if (inputKey.MatchesKeyCode(key))
                    return true;
            }
            return false;
        }

        public bool GetKeyDown()
        {
            foreach (InputKey key in InputKeys)
            {
                if (key.GetKeyDown())
                    return true;
            }
            return false;
        }

        public bool GetKey()
        {
            foreach (InputKey key in InputKeys)
            {
                if (key.GetKey())
                    return true;
            }
            return false;
        }

        public bool GetKeyUp()
        {
            foreach (InputKey key in InputKeys)
            {
                if (key.GetKeyUp())
                    return true;
            }
            return false;
        }

        public override JSONNode SerializeToJsonObject()
        {
            JSONArray array = new JSONArray();
            foreach (InputKey key in InputKeys)
                array.Add(new JSONString(key.ToString()));
            return array;
        }

        public override void DeserializeFromJsonObject(JSONNode json)
        {
            List<string> keyStrings = new List<string>();
            JSONArray array = json.AsArray;
            foreach (JSONString data in array)
            {
                keyStrings.Add(data.Value);
            }
            LoadFromStringArray(keyStrings.ToArray());
        }
    }
}