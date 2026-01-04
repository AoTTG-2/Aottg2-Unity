using ApplicationManagers;
using Characters;
using GameManagers;
using Settings;
using UI;
using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Reading player key inputs. Note that inputs are best handled in OnFrame rather than OnTick,
    /// due to being updated every frame and not every physics tick.
    /// </summary>
    [CLType(Name = "Input", Abstract = true, Static = true)]
    partial class CustomLogicInputBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicInputBuiltin(){}

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

        /// <summary>
        /// Gets the key name the player assigned to the key setting.
        /// </summary>
        /// <param name="key">The key setting path (e.g., "General/Attack" or "CustomKey/Space").</param>
        /// <returns>The key name.</returns>
        [CLMethod]
        public static string GetKeyName(string key)
        {
            KeyCode? customKey = GetCustomKeyCode(key);
            if (customKey.HasValue)
                return customKey.Value.ToString();
            
            KeybindSetting keybind = GetKeybind(key);
            return keybind.ToString();
        }

        /// <summary>
        /// Returns true if the key is being held down.
        /// </summary>
        /// <param name="key">The key setting path (e.g., "General/Attack" or "CustomKey/Space").</param>
        /// <returns>True if the key is being held down, false otherwise.</returns>
        [CLMethod]
        public static bool GetKeyHold(string key)
        {
            KeyCode? customKey = GetCustomKeyCode(key);
            if (customKey.HasValue)
                return Input.GetKey(customKey.Value) && CanKey();
            
            KeybindSetting keybind = GetKeybind(key);
            return keybind.GetKey(true) && CanKey();
        }

        /// <summary>
        /// Returns true if the key was pressed down this frame.
        /// </summary>
        /// <param name="key">The key setting path (e.g., "General/Attack" or "CustomKey/Space").</param>
        /// <returns>True if the key was pressed down this frame, false otherwise.</returns>
        [CLMethod]
        public static bool GetKeyDown(string key)
        {
            KeyCode? customKey = GetCustomKeyCode(key);
            if (customKey.HasValue)
                return Input.GetKeyDown(customKey.Value) && CanKey();
            
            KeybindSetting keybind = GetKeybind(key);
            return keybind.GetKeyDown(true) && CanKey();
        }

        /// <summary>
        /// Returns true if the key was released this frame.
        /// </summary>
        /// <param name="key">The key setting path (e.g., "General/Attack" or "CustomKey/Space").</param>
        /// <returns>True if the key was released this frame, false otherwise.</returns>
        [CLMethod]
        public static bool GetKeyUp(string key)
        {
            KeyCode? customKey = GetCustomKeyCode(key);
            if (customKey.HasValue)
                return Input.GetKeyUp(customKey.Value) && CanKey();
            
            KeybindSetting keybind = GetKeybind(key);
            return keybind.GetKeyUp(true) && CanKey();
        }

        /// <summary>
        /// Returns the position the player is aiming at.
        /// </summary>
        /// <returns>The aim position.</returns>
        [CLMethod]
        public static CustomLogicVector3Builtin GetMouseAim()
        {
            RaycastHit hit;
            Ray ray = SceneLoader.CurrentCamera.Camera.ScreenPointToRay(CursorManager.GetInGameMousePosition());
            Vector3 target = ray.origin + ray.direction * 1000f;
            if (Physics.Raycast(ray, out hit, 1000f, Human.AimMask.value))
                target = hit.point;
            return new CustomLogicVector3Builtin(target);
        }

        /// <summary>
        /// Returns the ray the player is aiming at.
        /// </summary>
        /// <returns>The aim direction vector.</returns>
        [CLMethod]
        public static CustomLogicVector3Builtin GetCursorAimDirection()
        {
            Ray ray = SceneLoader.CurrentCamera.Camera.ScreenPointToRay(CursorManager.GetInGameMousePosition());
            return new CustomLogicVector3Builtin(ray.direction);
        }

        /// <summary>
        /// Returns the speed of the mouse.
        /// </summary>
        /// <returns>The mouse speed vector.</returns>
        [CLMethod]
        public static CustomLogicVector3Builtin GetMouseSpeed()
        {
            Vector3 speed = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);
            return new CustomLogicVector3Builtin(speed);
        }

        /// <summary>
        /// Returns the position of the mouse.
        /// </summary>
        /// <returns>The mouse position.</returns>
        [CLMethod]
        public static CustomLogicVector3Builtin GetMousePosition()
        {
            Vector3 position = Input.mousePosition;
            return new CustomLogicVector3Builtin(position);
        }

        /// <summary>
        /// Returns the dimensions of the screen.
        /// </summary>
        /// <returns>The screen dimensions.</returns>
        [CLMethod]
        public static CustomLogicVector3Builtin GetScreenDimensions()
        {
            Vector3 dimensions = new Vector3(Screen.width, Screen.height, 0f);
            return new CustomLogicVector3Builtin(dimensions);
        }

        /// <summary>
        /// Sets whether the key is enabled by default.
        /// </summary>
        /// <param name="key">The key setting path (e.g., "General/Attack" or "CustomKey/Space").</param>
        /// <param name="enabled">Whether the key should be enabled by default.</param>
        [CLMethod]
        public static void SetKeyDefaultEnabled(string key, bool enabled)
        {
            KeybindSetting keybind = GetKeybind(key);
            if (enabled && CustomLogicManager.KeybindDefaultDisabled.Contains(keybind))
                CustomLogicManager.KeybindDefaultDisabled.Remove(keybind);
            else if (!enabled && !CustomLogicManager.KeybindDefaultDisabled.Contains(keybind))
                CustomLogicManager.KeybindDefaultDisabled.Add(keybind);
        }

        /// <summary>
        /// Sets whether the key is being held down.
        /// </summary>
        /// <param name="key">The key setting path (e.g., "General/Attack" or "CustomKey/Space").</param>
        /// <param name="enabled">Whether the key should be held down.</param>
        [CLMethod]
        public static void SetKeyHold(string key, bool enabled)
        {
            KeybindSetting keybind = GetKeybind(key);
            if (!enabled && CustomLogicManager.KeybindHold.Contains(keybind))
                CustomLogicManager.KeybindHold.Remove(keybind);
            else if (enabled && !CustomLogicManager.KeybindHold.Contains(keybind))
                CustomLogicManager.KeybindHold.Add(keybind);
        }

        /// <summary>
        /// Sets whether all keys in the specified category are enabled by default.
        /// </summary>
        /// <param name="category">The category name.</param>
        /// <param name="enabled">Whether the keys should be enabled by default.</param>
        [CLMethod]
        public static void SetCategoryKeysEnabled(
            [CLParam(Enum = typeof(CustomLogicInputCategoryEnum))] string category,
            bool enabled)
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

        /// <summary>
        /// Sets whether all General category keys are enabled by default.
        /// </summary>
        /// <param name="enabled">Whether the keys should be enabled by default.</param>
        [CLMethod]
        public static void SetGeneralKeysEnabled(bool enabled)
        {
            SetCategoryKeysEnabled("General", enabled);
        }

        /// <summary>
        /// Sets whether all Interaction category keys are enabled by default.
        /// </summary>
        /// <param name="enabled">Whether the keys should be enabled by default.</param>
        [CLMethod]
        public static void SetInteractionKeysEnabled(bool enabled)
        {
            SetCategoryKeysEnabled("Interaction", enabled);
        }

        /// <summary>
        /// Sets whether all Titan category keys are enabled by default.
        /// </summary>
        /// <param name="enabled">Whether the keys should be enabled by default.</param>
        [CLMethod]
        public static void SetTitanKeysEnabled(bool enabled)
        {
            SetCategoryKeysEnabled("Titan", enabled);
        }

        /// <summary>
        /// Sets whether all Human category keys are enabled by default.
        /// </summary>
        /// <param name="enabled">Whether the keys should be enabled by default.</param>
        [CLMethod]
        public static void SetHumanKeysEnabled(bool enabled)
        {
            SetCategoryKeysEnabled("Human", enabled);
        }
    }
}
