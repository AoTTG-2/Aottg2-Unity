using ApplicationManagers;
using GameManagers;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Static = true)]
    class CustomLogicTimeBuiltin : CustomLogicClassInstanceBuiltin
    {
        public CustomLogicTimeBuiltin() : base("Time")
        {
        }

        [CLProperty("Gets the fixed tick time.")]
        public static float TickTime => Time.fixedDeltaTime;

        [CLProperty("Gets the frame time.")]
        public static float FrameTime => Time.deltaTime;

        [CLProperty("Gets the game time.")]
        public static float GameTime => CustomLogicManager.Evaluator.CurrentTime;

        [CLProperty("Gets or sets the time scale.")]
        public static float TimeScale
        {
            get => Time.timeScale;
            set => Time.timeScale = value;
        }
    }
}