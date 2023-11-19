using ApplicationManagers;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicCutsceneBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicCutsceneBuiltin(): base("Cutscene")
        {
        }

        public override object CallMethod(string methodName, List<object> parameters)
        {
            if (methodName == "Start")
            {
                string name = (string)parameters[0];
                bool full = (bool)parameters[1];
                CustomLogicManager._instance.StartCoroutine(StartCutscene(name, full));
            }
            else if (methodName == "ShowDialogue")
            {
                string icon = (string)parameters[0];
                string title = (string)parameters[1];
                string content = (string)parameters[2];
                ((InGameMenu)UIManager.CurrentMenu).ShowCutsceneMenu(icon, title, content);
            }
            else if (methodName == "HideDialogue")
            {
                ((InGameMenu)UIManager.CurrentMenu).HideCutsceneMenu();
            }
            return null;
        }

        private IEnumerator StartCutscene(string name, bool full)
        {
            var instance = CustomLogicManager.Evaluator.CreateClassInstance(name, new List<object>(), true);
            CustomLogicManager.ToggleCutscene(full);
            yield return CustomLogicManager.Evaluator.EvaluateMethod(instance, "Start", new List<object>());
            CustomLogicManager.ToggleCutscene(false);
            ((InGameMenu)UIManager.CurrentMenu).HideCutsceneMenu();
        }
    }
}
