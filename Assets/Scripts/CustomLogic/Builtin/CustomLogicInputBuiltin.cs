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
                string key = (string)parameters[0];
                string[] strArr = key.Split('/');
                KeybindSetting keybind = (KeybindSetting)((SaveableSettingsContainer)SettingsManager.InputSettings.Settings[strArr[0]]).Settings[strArr[1]];
                if (name == "GetKeyDown")
                    return keybind.GetKeyDown();
                if (name == "GetKeyUp")
                    return keybind.GetKeyUp();
                if (name == "GetKeyHold")
                    return keybind.GetKey();
                return false;
            }
            if (name == "GetKeyName")
            {
                string key = (string)parameters[0];
                string[] strArr = key.Split('/');
                KeybindSetting keybind = (KeybindSetting)((SaveableSettingsContainer)SettingsManager.InputSettings.Settings[strArr[0]]).Settings[strArr[1]];
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
            if (name == "GetMousePosition")
            {
                position = Input.mousePosition;
                return new CustomLogicVector3Builtin(position);
            }
            return base.CallMethod(name, parameters);
        }
    }
}
