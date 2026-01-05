using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of line texture mode values for LineRenderer.
    /// </summary>
    [CLType(Name = "LineTextureModeEnum", Static = true, Abstract = true)]
    partial class CustomLogicLineTextureModeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicLineTextureModeEnum() { }

        /// <summary>
        /// Stretch: texture is stretched along the entire line.
        /// </summary>
        [CLProperty]
        public static int Stretch => (int)LineTextureMode.Stretch;

        /// <summary>
        /// Tile: texture is tiled along the line.
        /// </summary>
        [CLProperty]
        public static int Tile => (int)LineTextureMode.Tile;

        /// <summary>
        /// DistributePerSegment: texture is distributed per segment.
        /// </summary>
        [CLProperty]
        public static int DistributePerSegment => (int)LineTextureMode.DistributePerSegment;

        /// <summary>
        /// RepeatPerSegment: texture is repeated per segment.
        /// </summary>
        [CLProperty]
        public static int RepeatPerSegment => (int)LineTextureMode.RepeatPerSegment;
    }
}
