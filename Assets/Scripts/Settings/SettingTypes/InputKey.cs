using System;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    class InputKey
    {
        protected KeyCode _key;
        protected bool _isSpecial;
        protected SpecialKey _special;
        protected bool _isModifier;
        protected KeyCode _modifier;
        protected HashSet<KeyCode> ModifierKeys = new HashSet<KeyCode> { KeyCode.LeftShift, KeyCode.LeftAlt, KeyCode.LeftControl, KeyCode.RightShift, KeyCode.RightAlt, KeyCode.RightControl };
        protected HashSet<string> AlphaDigits = new HashSet<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        public InputKey()
        {
        }

        public InputKey(string keyStr)
        {
            LoadFromString(keyStr);
        }

        public bool MatchesKeyCode(KeyCode key)
        {
            return !_isSpecial && !_isModifier && _key == key;
        }

        public bool ReadNextInput()
        {
            float leftStickX = Input.GetAxis("LeftStickHorizontal");
            float leftStickY = Input.GetAxis("LeftStickVertical");

            if (Mathf.Abs(leftStickX) != 0|| Mathf.Abs(leftStickY) != 0)
            {
                if (Mathf.Abs(leftStickX) > Mathf.Abs(leftStickY))
                {
                    _special = leftStickX > 0 ? SpecialKey.LeftStickRight : SpecialKey.LeftStickLeft;
                }
                else
                {
                    _special = leftStickY < 0 ? SpecialKey.LeftStickUp : SpecialKey.LeftStickDown;
                }
                _isSpecial = true;
                return true;
            }
            _isModifier = false;
            foreach (KeyCode key in ModifierKeys)
            {
                if (Input.GetKey(key))
                {
                    _modifier = key;
                    _isModifier = true;
                }
            }

            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (key == KeyCode.WheelDown || key == KeyCode.WheelUp)
                    continue;
                if (ModifierKeys.Contains(key) && Input.GetKeyUp(key))
                {
                    _isModifier = false;
                    _key = key;
                    _isSpecial = false;
                    return true;
                }
                else if (!ModifierKeys.Contains(key) && key == KeyCode.Mouse0 && Input.GetKeyUp(key))
                {
                    _key = key;
                    _isSpecial = false;
                    return true;
                }
                else if (!ModifierKeys.Contains(key) && key != KeyCode.Mouse0 && Input.GetKeyDown(key))
                {
                    _key = key;
                    _isSpecial = false;
                    return true;
                }
            }

            foreach (SpecialKey specialKey in Enum.GetValues(typeof(SpecialKey)))
            {
                if (GetSpecial(specialKey))
                {
                    _special = specialKey;
                    _isSpecial = true;
                    return true;
                }
            }

            return false;
        }

        public bool GetKeyDown()
        {
            if (_isSpecial)
                return GetModifier() && GetSpecial(_special);
            return GetModifier() && Input.GetKeyDown(_key);
        }

        public bool GetKey()
        {
            if (_isSpecial)
                return GetModifier() && GetSpecial(_special);
            return GetModifier() && Input.GetKey(_key);
        }

        public bool GetKeyUp()
        {
            if (_isSpecial)
                return GetModifier() && GetSpecial(_special);
            return GetModifier() && Input.GetKeyUp(_key);
        }

        public bool IsWheel()
        {
            return _isSpecial && (_special == SpecialKey.WheelDown || _special == SpecialKey.WheelUp);
        }

        public bool IsNone()
        {
            return _isSpecial && _special == SpecialKey.None;
        }

        public override string ToString()
        {
            string value = _isSpecial ? _special.ToString() : _key.ToString();
            if (value.StartsWith("Alpha"))
                value = value.Substring(5);
            if (_isModifier)
                value = _modifier.ToString() + "+" + value;
            return value;
        }

        public override bool Equals(object obj)
        {
            return ToString() == obj.ToString();
        }

        public void LoadFromString(string serializedKey)
        {
            _isModifier = false;
            string[] keyArray = serializedKey.Split('+');
            string keyString = keyArray[0];
            if (keyArray.Length > 1)
            {
                _modifier = keyString.ToEnum<KeyCode>();
                _isModifier = true;
                keyString = keyArray[1];
            }
            if (keyString.Length == 1 && AlphaDigits.Contains(keyString))
                keyString = "Alpha" + keyString;
            KeyCode key = keyString.ToEnum<KeyCode>();
            if (key != KeyCode.None && key != KeyCode.WheelDown && key != KeyCode.WheelUp)
            {
                _key = key;
                _isSpecial = false;
                return;
            }
            _special = keyString.ToEnum<SpecialKey>();
            _isSpecial = true;
        }

        protected bool GetModifier()
        {
            return !_isModifier || Input.GetKey(_modifier);
        }

        protected bool GetSpecial(SpecialKey specialKey)
        {
            switch (specialKey)
            {
                case SpecialKey.WheelUp:
                    return Input.GetAxis("Mouse ScrollWheel") > 0f;
                case SpecialKey.WheelDown:
                    return Input.GetAxis("Mouse ScrollWheel") < 0f;
                case SpecialKey.LeftStickUp:
                    return Input.GetAxis("LeftStickVertical") > Deadzone;
                case SpecialKey.LeftStickDown:
                    return Input.GetAxis("LeftStickVertical") < -Deadzone;
                case SpecialKey.LeftStickLeft:
                    return Input.GetAxis("LeftStickHorizontal") < -Deadzone;
                case SpecialKey.LeftStickRight:
                    return Input.GetAxis("LeftStickHorizontal") > Deadzone;
                default:
                    return false;
            }
        }
    }

    public enum SpecialKey
    {
        None,
        WheelUp,
        WheelDown,
        LeftStickUp,
        LeftStickDown,
        LeftStickLeft,
        LeftStickRight
    }
}
