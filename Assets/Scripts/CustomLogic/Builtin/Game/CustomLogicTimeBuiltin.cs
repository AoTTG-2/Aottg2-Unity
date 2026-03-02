using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Time functions.
    /// </summary>
    [CLType(Name = "Time", Static = true, Abstract = true)]
    partial class CustomLogicTimeBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicTimeBuiltin(){}

        /// <summary>
        /// Time between each tick (0.02 seconds).
        /// </summary>
        [CLProperty]
        public static float TickTime => Time.fixedDeltaTime;

        /// <summary>
        /// Time between each frame.
        /// </summary>
        [CLProperty]
        public static float FrameTime => Time.deltaTime;

        /// <summary>
        /// Time since start of the round.
        /// </summary>
        [CLProperty]
        public static float GameTime => CustomLogicManager.Evaluator.CurrentTime;

        /// <summary>
        /// Unity's Time.time.
        /// </summary>
        [CLProperty]
        public static float EngineTime => Time.time;

        /// <summary>
        /// Changes the timescale of the game.
        /// </summary>
        [CLProperty]
        public static float TimeScale
        {
            get => Time.timeScale;
            set => Time.timeScale = value;
        }
    }
}
