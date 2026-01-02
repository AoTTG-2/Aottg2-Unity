using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "Time", Static = true, Abstract = true, Description = "Time functions.")]
    partial class CustomLogicTimeBuiltin : BuiltinClassInstance
    {
        [CLConstructor("Creates a new Time instance.")]
        public CustomLogicTimeBuiltin() { }

        [CLProperty("Time between each tick (0.02 seconds)")]
        public static float TickTime => Time.fixedDeltaTime;

        [CLProperty("Time between each frame.")]
        public static float FrameTime => Time.deltaTime;

        [CLProperty("Time since start of the round.")]
        public static float GameTime => CustomLogicManager.Evaluator.CurrentTime;

        [CLProperty("Unity's Time.time")]
        public static float EngineTime => Time.time;

        [CLProperty("Changes the timescale of the game.")]
        public static float TimeScale
        {
            get => Time.timeScale;
            set => Time.timeScale = value;
        }
    }
}
