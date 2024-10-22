using ApplicationManagers;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicCutsceneBuiltin : CustomLogicBaseBuiltin
    {
        public CustomLogicCutsceneBuiltin() : base("Cutscene")
        {
        }

        public override object CallMethod(string methodName, List<object> parameters)
        {
            if (methodName == "Start")
            {
                string name = (string)parameters[0];
                bool full = (bool)parameters[1];
                StartCutscene(name, full).Forget();
                return null;
            }
            if (methodName == "ShowDialogue")
            {
                string icon = (string)parameters[0];
                string title = (string)parameters[1];
                string content = (string)parameters[2];
                ((InGameMenu)UIManager.CurrentMenu).ShowCutsceneMenu(icon, title, content, CustomLogicManager.Cutscene);
                return null;
            }
            if (methodName == "ShowDialogueForTime")
            {
                string icon = (string)parameters[0];
                string title = (string)parameters[1];
                string content = (string)parameters[2];
                float time = (float)parameters[3];
                CustomLogicManager._instance.StartCoroutine(ShowDialogueForTime(icon, title, content, time));
                return null;
            }
            if (methodName == "HideDialogue")
            {
                ((InGameMenu)UIManager.CurrentMenu).HideCutsceneMenu();
                return null;
            }
            return base.CallMethod(methodName, parameters);
        }

        private IEnumerator ShowDialogueForTime(string icon, string title, string content, float time)
        {
            ((InGameMenu)UIManager.CurrentMenu).ShowCutsceneMenu(icon, title, content, CustomLogicManager.Cutscene);
            yield return new WaitForSeconds(time);
            ((InGameMenu)UIManager.CurrentMenu).HideCutsceneMenu();
        }

        private static async UniTaskVoid StartCutscene(string name, bool full)
        {
            var instance = CustomLogicManager.Evaluator.CreateClassInstance(name, new List<object>());
            CustomLogicManager.ToggleCutscene(full);
            await CustomLogicManager.Evaluator.EvaluateMethod(instance, "Start");
            CustomLogicManager.ToggleCutscene(false);
            ((InGameMenu)UIManager.CurrentMenu).HideCutsceneMenu();
        }
    }
}
