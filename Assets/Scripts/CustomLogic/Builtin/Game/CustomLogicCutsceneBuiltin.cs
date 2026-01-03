using System.Collections;
using UI;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "Cutscene", Abstract = true, Static = true)]
    partial class CustomLogicCutsceneBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicCutsceneBuiltin()
        {
        }

        // Convert the above to CLMethod
        [CLMethod(Static = true, Description = "Start a cutscene")]
        public void Start(
            [CLParam("The name of the cutscene class to start.")]
            string name,
            [CLParam("If true, enables full cutscene mode.")]
            bool full)
        {
            CustomLogicManager._instance.StartCoroutine(StartCutscene(name, full));
        }

        [CLMethod(Static = true, Description = "Show a dialogue box")]
        public void ShowDialogue(
            [CLParam("The icon name to display.")]
            string icon,
            [CLParam("The title of the dialogue.")]
            string title,
            [CLParam("The content text of the dialogue.")]
            string content)
        {
            ((InGameMenu)UIManager.CurrentMenu).ShowCutsceneMenu(icon, title, content, CustomLogicManager.Cutscene);
        }

        [CLMethod(Static = true, Description = "Show a dialogue box for a certain amount of time")]
        public void ShowDialogueForTime(
            [CLParam("The icon name to display.")]
            string icon,
            [CLParam("The title of the dialogue.")]
            string title,
            [CLParam("The content text of the dialogue.")]
            string content,
            [CLParam("The duration in seconds to show the dialogue.")]
            float time)
        {
            CustomLogicManager._instance.StartCoroutine(routine_ShowDialogueForTime(icon, title, content, time));
        }

        [CLMethod(Static = true, Description = "Hide the dialogue box")]
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
