using ApplicationManagers;
using Characters;
using GameManagers;
using Settings;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    [CLType(Abstract = true, Static = true, InheritBaseMembers = true)]
    class CustomLogicInputBuiltin: CustomLogicClassInstanceBuiltin
    {
        public CustomLogicInputBuiltin(): base("Input")
        {
        }

        private static KeybindSetting GetKeybind(string key)
        {
            string[] strArr = key.Split('/');
            KeybindSetting keybind = (KeybindSetting)((SaveableSettingsContainer)SettingsManager.InputSettings.Settings[strArr[0]]).Settings[strArr[1]];
            return keybind;
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
            KeybindSetting keybind = GetKeybind(key);
            return keybind.ToString();
        }

        [CLMethod(description: "Returns true if the key is being held down")]
        public static bool GetKeyHold(string key)
        {
            KeybindSetting keybind = GetKeybind(key);
            return keybind.GetKey(true) && CanKey();
        }

        [CLMethod(description: "Returns true if the key was pressed down this frame")]
        public static bool GetKeyDown(string key)
        {
            KeybindSetting keybind = GetKeybind(key);
            return keybind.GetKeyDown(true) && CanKey();
        }

        [CLMethod(description: "Returns true if the key was released this frame")]
        public static bool GetKeyUp(string key)
        {
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
    }
}
