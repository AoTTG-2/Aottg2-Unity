using ApplicationManagers;
using Characters;
using GameManagers;
using Settings;
using UI;
using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Reading player key inputs. Note that inputs are best handled in OnFrame rather than OnTick, due to being updated every frame and not every physics tick.
    /// </summary>
    [CLType(Name = "Input", Abstract = true, Static = true)]
    partial class CustomLogicInputBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicInputBuiltin()
        {
        }

        private static KeybindSetting GetKeybind(string key)
        {
            string[] strArr = key.Split('/');
            KeybindSetting keybind = (KeybindSetting)((SaveableSettingsContainer)SettingsManager.InputSettings.Settings[strArr[0]]).Settings[strArr[1]];
            return keybind;
        }

        private static KeyCode? GetCustomKeyCode(string key)
        {
            if (!key.StartsWith("CustomKey/"))
                return null;
            
            string keyCodeStr = key.Substring("CustomKey/".Length);
            if (System.Enum.TryParse(keyCodeStr, out KeyCode keyCode))
                return keyCode;
            
            return null;
        }

        private static bool CanKey()
        {
            var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
            if (gameManager.GlobalPause)
                return false;
            bool inMenu = InGameMenu.InMenu() || ChatManager.IsChatActive() || CustomLogicManager.Cutscene;
            return !inMenu;
        }

        [CLMethod(description: "Gets the key name the player assigned to the key setting")]
        public static string GetKeyName(string key)
        {
            KeyCode? customKey = GetCustomKeyCode(key);
            if (customKey.HasValue)
                return customKey.Value.ToString();
            
            KeybindSetting keybind = GetKeybind(key);
            return keybind.ToString();
        }

        [CLMethod(description: "Returns true if the key is being held down")]
        public static bool GetKeyHold(string key)
        {
            KeyCode? customKey = GetCustomKeyCode(key);
            if (customKey.HasValue)
                return Input.GetKey(customKey.Value) && CanKey();
            
            KeybindSetting keybind = GetKeybind(key);
            return keybind.GetKey(true) && CanKey();
        }

        [CLMethod(description: "Returns true if the key was pressed down this frame")]
        public static bool GetKeyDown(string key)
        {
            KeyCode? customKey = GetCustomKeyCode(key);
            if (customKey.HasValue)
                return Input.GetKeyDown(customKey.Value) && CanKey();
            
            KeybindSetting keybind = GetKeybind(key);
            return keybind.GetKeyDown(true) && CanKey();
        }

        [CLMethod(description: "Returns true if the key was released this frame")]
        public static bool GetKeyUp(string key)
        {
            KeyCode? customKey = GetCustomKeyCode(key);
            if (customKey.HasValue)
                return Input.GetKeyUp(customKey.Value) && CanKey();
            
            KeybindSetting keybind = GetKeybind(key);
            return keybind.GetKeyUp(true) && CanKey();
        }

        [CLMethod(description: "Returns the position the player is aiming at")]
        public static CustomLogicVector3Builtin GetMouseAim()
        {
            RaycastHit hit;
            Ray ray = SceneLoader.CurrentCamera.Camera.ScreenPointToRay(CursorManager.GetInGameMousePosition());
            Vector3 target = ray.origin + ray.direction * 1000f;
            if (Physics.Raycast(ray, out hit, 1000f, Human.AimMask.value))
                target = hit.point;
            return new CustomLogicVector3Builtin(target);
        }

        [CLMethod(description: "Returns the ray the player is aiming at")]
        public static CustomLogicVector3Builtin GetCursorAimDirection()
        {
            Ray ray = SceneLoader.CurrentCamera.Camera.ScreenPointToRay(CursorManager.GetInGameMousePosition());
            return new CustomLogicVector3Builtin(ray.direction);
        }

        [CLMethod(description: "Returns the speed of the mouse")]
        public static CustomLogicVector3Builtin GetMouseSpeed()
        {
            Vector3 speed = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);
            return new CustomLogicVector3Builtin(speed);
        }

        [CLMethod(description: "Returns the position of the mouse")]
        public static CustomLogicVector3Builtin GetMousePosition()
        {
            Vector3 position = Input.mousePosition;
            return new CustomLogicVector3Builtin(position);
        }

        [CLMethod(description: "Returns the dimensions of the screen")]
        public static CustomLogicVector3Builtin GetScreenDimensions()
        {
            Vector3 dimensions = new Vector3(Screen.width, Screen.height, 0f);
            return new CustomLogicVector3Builtin(dimensions);
        }

        [CLMethod(description: "Sets whether the key is enabled by default")]
        public static void SetKeyDefaultEnabled(string key, bool enabled)
        {
            KeybindSetting keybind = GetKeybind(key);
            if (enabled && CustomLogicManager.KeybindDefaultDisabled.Contains(keybind))
                CustomLogicManager.KeybindDefaultDisabled.Remove(keybind);
            else if (!enabled && !CustomLogicManager.KeybindDefaultDisabled.Contains(keybind))
                CustomLogicManager.KeybindDefaultDisabled.Add(keybind);
        }

        [CLMethod(description: "Sets whether the key is being held down")]
        public static void SetKeyHold(string key, bool enabled)
        {
            KeybindSetting keybind = GetKeybind(key);
            if (!enabled && CustomLogicManager.KeybindHold.Contains(keybind))
                CustomLogicManager.KeybindHold.Remove(keybind);
            else if (enabled && !CustomLogicManager.KeybindHold.Contains(keybind))
                CustomLogicManager.KeybindHold.Add(keybind);
        }

        [CLMethod(description: "Sets whether all keys in the specified category are enabled by default. Valid categories: General, Human, Titan, Interaction")]
        public static void SetCategoryKeysEnabled(string category, bool enabled)
        {
            if (!SettingsManager.InputSettings.Settings.Contains(category))
            {
                Debug.LogError($"Invalid input category: {category}. Valid categories are: General, Human, Titan, Interaction");
                return;
            }

            SaveableSettingsContainer categoryContainer = (SaveableSettingsContainer)SettingsManager.InputSettings.Settings[category];
            foreach (System.Collections.DictionaryEntry settingPair in categoryContainer.Settings)
            {
                if (settingPair.Value is KeybindSetting keybind)
                {
                    if (enabled && CustomLogicManager.KeybindDefaultDisabled.Contains(keybind))
                        CustomLogicManager.KeybindDefaultDisabled.Remove(keybind);
                    else if (!enabled && !CustomLogicManager.KeybindDefaultDisabled.Contains(keybind))
                        CustomLogicManager.KeybindDefaultDisabled.Add(keybind);
                }
            }
        }

        [CLMethod(description: "Sets whether all General category keys are enabled by default")]
        public static void SetGeneralKeysEnabled(bool enabled)
        {
            SetCategoryKeysEnabled("General", enabled);
        }

        [CLMethod(description: "Sets whether all Interaction category keys are enabled by default")]
        public static void SetInteractionKeysEnabled(bool enabled)
        {
            SetCategoryKeysEnabled("Interaction", enabled);
        }

        [CLMethod(description: "Sets whether all Titan category keys are enabled by default")]
        public static void SetTitanKeysEnabled(bool enabled)
        {
            SetCategoryKeysEnabled("Titan", enabled);
        }

        [CLMethod(description: "Sets whether all Human category keys are enabled by default")]
        public static void SetHumanKeysEnabled(bool enabled)
        {
            SetCategoryKeysEnabled("Human", enabled);
        }
    }
}
