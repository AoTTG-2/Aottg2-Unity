using System;
using System.Collections;
using UI;
using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Provides methods to control in-game cutscenes and dialogues from custom logic scripts.
    /// </summary>
    [CLType(Name = "Cutscene", Abstract = true, Static = true)]
    partial class CustomLogicCutsceneBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicCutsceneBuiltin()
        {
        }

        // Convert the above to CLMethod
        /// <summary>
        /// Start a cutscene.
        /// </summary>
        /// <param name="name">The name of the cutscene class to start.</param>
        /// <param name="full">If true, enables full cutscene mode.</param>
        [CLMethod(Static = true)]
        public void Start(
            string name,
            bool full)
        {
            CustomLogicManager._instance.StartCoroutine(StartCutscene(name, full));
        }

        /// <summary>
        /// Show a dialogue box.
        /// </summary>
        /// <param name="icon">The icon name to display.</param>
        /// <param name="title">The title of the dialogue.</param>
        /// <param name="content">The content text of the dialogue.</param>
        [CLMethod(Static = true)]
        public void ShowDialogue(
            [CLParam(Enum = new Type[] { typeof(CustomLogicProfileIconEnum) })] string icon,
            string title,
            string content)
        {
            ((InGameMenu)UIManager.CurrentMenu).ShowCutsceneMenu(icon, title, content, CustomLogicManager.Cutscene);
        }

        /// <summary>
        /// Show a dialogue box for a certain amount of time.
        /// </summary>
        /// <param name="icon">The icon name to display.</param>
        /// <param name="title">The title of the dialogue.</param>
        /// <param name="content">The content text of the dialogue.</param>
        /// <param name="time">The duration in seconds to show the dialogue.</param>
        [CLMethod(Static = true)]
        public void ShowDialogueForTime(
            [CLParam(Enum = new Type[] { typeof(CustomLogicProfileIconEnum) })] string icon,
            string title,
            string content,
            float time)
        {
            CustomLogicManager._instance.StartCoroutine(routine_ShowDialogueForTime(icon, title, content, time));
        }

        /// <summary>
        /// Hide the dialogue box.
        /// </summary>
        [CLMethod(Static = true)]
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
