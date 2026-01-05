using Settings;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of camera input mode values.
    /// </summary>
    [CLType(Name = "CameraModeEnum", Static = true, Abstract = true)]
    partial class CustomLogicCameraModeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicCameraModeEnum() { }

        private static readonly string _tps = CameraInputMode.TPS.ToString();
        private static readonly string _original = CameraInputMode.Original.ToString();
        private static readonly string _fps = CameraInputMode.FPS.ToString();

        /// <summary>
        /// TPS camera mode.
        /// </summary>
        [CLProperty]
        public static string TPS => _tps;

        /// <summary>
        /// Original camera mode.
        /// </summary>
        [CLProperty]
        public static string Original => _original;

        /// <summary>
        /// FPS camera mode.
        /// </summary>
        [CLProperty]
        public static string FPS => _fps;
    }
}
