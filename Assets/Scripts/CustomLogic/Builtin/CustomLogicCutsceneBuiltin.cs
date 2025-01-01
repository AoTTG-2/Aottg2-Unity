using ApplicationManagers;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "Cutscene", Abstract = true, Static = true, InheritBaseMembers = true)]
    partial class CustomLogicCutsceneBuiltin : CustomLogicClassInstanceBuiltin
    {
        public CustomLogicCutsceneBuiltin() : base("Cutscene")
        {
        }

        // Convert the above to CLMethod
        [CLMethod("Start a cutscene")]
        public void Start(string name, bool full)
        {
            CustomLogicManager._instance.StartCoroutine(StartCutscene(name, full));
        }

        [CLMethod("Show a dialogue box")]
        public void ShowDialogue(string icon, string title, string content)
        {
            ((InGameMenu)UIManager.CurrentMenu).ShowCutsceneMenu(icon, title, content, CustomLogicManager.Cutscene);
        }

        [CLMethod("Show a dialogue box for a certain amount of time")]
        public void ShowDialogueForTime(string icon, string title, string content, float time)
        {
            CustomLogicManager._instance.StartCoroutine(routine_ShowDialogueForTime(icon, title, content, time));
        }

        [CLMethod("Hide the dialogue box")]
        public void HideDialogue()
        {
            ((InGameMenu)UIManager.CurrentMenu).HideCutsceneMenu();
        }


        private IEnumerator routine_ShowDialogueForTime(string icon, string title, string content, float time)
        {
            ((InGameMenu)UIManager.CurrentMenu).ShowCutsceneMenu(icon, title, content, CustomLogicManager.Cutscene);
            yield return new WaitForSeconds(time);
            ((InGameMenu)UIManager.CurrentMenu).HideCutsceneMenu();
        }

        private IEnumerator StartCutscene(string name, bool full)
        {
            var instance = CustomLogicManager.Evaluator.CreateClassInstance(name, CustomLogicEvaluator.EmptyArgs, true);
            CustomLogicManager.ToggleCutscene(full);
            yield return CustomLogicManager.Evaluator.EvaluateMethod(instance, "Start");
            CustomLogicManager.ToggleCutscene(false);
            ((InGameMenu)UIManager.CurrentMenu).HideCutsceneMenu();
        }
    }
}
