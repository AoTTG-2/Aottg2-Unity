using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "Time", Static = true, Abstract = true)]
    partial class CustomLogicTimeBuiltin : CustomLogicClassInstanceBuiltin
    {
        public CustomLogicTimeBuiltin() : base("Time") { }

        [CLProperty("Time between each tick (0.02 seconds)")]
        public static float TickTime => Time.fixedDeltaTime;

        [CLProperty("Time between each frame.")]
        public static float FrameTime => Time.deltaTime;

        [CLProperty("Time since start of the round.")]
        public static float GameTime => CustomLogicManager.Evaluator.CurrentTime;

        [CLProperty("Changes the timescale of the game.")]
        public static float TimeScale
        {
            get => Time.timeScale;
            set => Time.timeScale = value;
        }
    }
}