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
    class CustomLogicInputBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicInputBuiltin(): base("Input")
        {
        }

        private KeybindSetting GetKeybind(string key)
        {
            string[] strArr = key.Split('/');
            KeybindSetting keybind = (KeybindSetting)((SaveableSettingsContainer)SettingsManager.InputSettings.Settings[strArr[0]]).Settings[strArr[1]];
            return keybind;
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            if (name == "GetKeyHold" || name == "GetKeyDown" || name == "GetKeyUp")
            {
                var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
                if (gameManager.GlobalPause)
                    return false;
                bool inMenu = InGameMenu.InMenu() || ChatManager.IsChatActive() || CustomLogicManager.Cutscene;
                if (inMenu)
                    return false;
                KeybindSetting keybind = GetKeybind((string)parameters[0]);
                if (name == "GetKeyDown")
                    return keybind.GetKeyDown(true);
                if (name == "GetKeyUp")
                    return keybind.GetKeyUp(true);
                if (name == "GetKeyHold")
                    return keybind.GetKey(true);
                return false;
            }
            if (name == "GetKeyName")
            {
                KeybindSetting keybind = GetKeybind((string)parameters[0]);
                return keybind.ToString();
            }
            if (name == "GetMouseAim")
            {
                RaycastHit hit;
                Ray ray = SceneLoader.CurrentCamera.Camera.ScreenPointToRay(CursorManager.GetInGameMousePosition());
                Vector3 target = ray.origin + ray.direction * 1000f;
                if (Physics.Raycast(ray, out hit, 1000f, Human.AimMask.value))
                    target = hit.point;
                return new CustomLogicVector3Builtin(target);
            }
            if (name == "GetMouseSpeed")
            {
                Vector3 speed = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);
                return new CustomLogicVector3Builtin(speed);
            }
            if (name == "GetMousePosition")
            {
                Vector3 position = Input.mousePosition;
                return new CustomLogicVector3Builtin(position);
            }
            if (name == "GetScreenDimensions")
            {
                Vector3 dimensions = new Vector3(Screen.width, Screen.height, 0f);
                return new CustomLogicVector3Builtin(dimensions);
            if (name == "SetKeyDefaultEnabled")
            {
                KeybindSetting keybind = GetKeybind((string)parameters[0]);
                bool enabled = (bool)parameters[1];
                if (enabled && CustomLogicManager.KeybindDefaultDisabled.Contains(keybind))
                    CustomLogicManager.KeybindDefaultDisabled.Remove(keybind);
                else if (!enabled && !CustomLogicManager.KeybindDefaultDisabled.Contains(keybind))
                    CustomLogicManager.KeybindDefaultDisabled.Add(keybind);
            }
            if (name == "SetKeyHold")
            {
                KeybindSetting keybind = GetKeybind((string)parameters[0]);
                bool enabled = (bool)parameters[1];
                if (!enabled && CustomLogicManager.KeybindHold.Contains(keybind))
                    CustomLogicManager.KeybindHold.Remove(keybind);
                else if (enabled && !CustomLogicManager.KeybindHold.Contains(keybind))
                    CustomLogicManager.KeybindHold.Add(keybind);
            }
            return base.CallMethod(name, parameters);
        }
    }
}
