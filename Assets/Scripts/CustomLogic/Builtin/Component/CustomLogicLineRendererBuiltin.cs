using System;
using System.Collections.Generic;
using ApplicationManagers;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    /// <summary>
    /// Represents a LineRenderer.
    /// </summary>
    [CLType(Name = "LineRenderer", Static = true, IsComponent = true)]
    partial class CustomLogicLineRendererBuiltin : BuiltinClassInstance
    {
        public LineRenderer Value = null;

        /// <summary>
        /// Default constructor, creates a black line with a width of 1.
        /// </summary>
        [CLConstructor]
        public CustomLogicLineRendererBuiltin()
            : this(Color.black, 1f) { }

        /// <summary>
        /// Creates a line with the given color and width.
        /// </summary>
        /// <param name="color">The color of the line.</param>
        /// <param name="width">The width of the line.</param>
        [CLConstructor]
        public CustomLogicLineRendererBuiltin(CustomLogicColorBuiltin color, float width = 1f)
            : this(color.Value.ToColor(), width) { }

        private CustomLogicLineRendererBuiltin(Color color, float width)
        {
            Value = new GameObject().AddComponent<LineRenderer>();
            Value.material = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Map, "Materials/TransparentMaterial", true);
            Value.material.color = color;
            Value.startWidth = width;
            Value.endWidth = width;
            Value.positionCount = 0;
            Value.enabled = false;
        }

        /// <summary>
        /// Remove the line renderer (can also be done by removing all references to this object).
        /// </summary>
        [CLMethod]
        public void Destroy()
        {
            GameObject.Destroy(Value.gameObject);
        }

        ~CustomLogicLineRendererBuiltin()
        {
            // Remove object when reference is lost
            if (Value.gameObject != null)
            {
                GameObject.Destroy(Value.gameObject);
            }
        }

        /// <summary>
        /// The width of the line at the start.
        /// </summary>
        [CLProperty]
        public float StartWidth
        {
            get => Value.startWidth;
            set => Value.startWidth = value;
        }

        /// <summary>
        /// The width of the line at the end.
        /// </summary>
        [CLProperty]
        public float EndWidth
        {
            get => Value.endWidth;
            set => Value.endWidth = value;
        }

        /// <summary>
        /// The color of the line.
        /// </summary>
        [CLProperty]
        public CustomLogicColorBuiltin LineColor
        {
            get => new CustomLogicColorBuiltin(new Color255(Value.material.color));
            set => Value.material.color = value.Value.ToColor();
        }

        /// <summary>
        /// The number of points in the line.
        /// </summary>
        [CLProperty]
        public int PositionCount
        {
            get => Value.positionCount;
            set => Value.positionCount = value;
        }

        /// <summary>
        /// Is the line renderer enabled.
        /// </summary>
        [CLProperty]
        public new bool Enabled
        {
            get => Value.enabled;
            set => Value.enabled = value;
        }

        /// <summary>
        /// Is the line renderer a loop.
        /// </summary>
        [CLProperty]
        public bool Loop
        {
            get => Value.loop;
            set => Value.loop = value;
        }

        /// <summary>
        /// The number of corner vertices.
        /// </summary>
        [CLProperty]
        public int NumCornerVertices
        {
            get => Value.numCornerVertices;
            set => Value.numCornerVertices = value;
        }

        /// <summary>
        /// The number of end cap vertices.
        /// </summary>
        [CLProperty]
        public int NumCapVertices
        {
            get => Value.numCapVertices;
            set => Value.numCapVertices = value;
        }

        /// <summary>
        /// The alignment of the line renderer.
        /// </summary>
        [CLProperty]
        public string Alignment
        {
            get => Value.alignment.ToString();
            set => Value.alignment = (LineAlignment)System.Enum.Parse(typeof(LineAlignment), value);
        }

        /// <summary>
        /// The texture mode of the line renderer.
        /// </summary>
        [CLProperty]
        public string TextureMode
        {
            get => Value.textureMode.ToString();
            set => Value.textureMode = (LineTextureMode)System.Enum.Parse(typeof(LineTextureMode), value);
        }

        /// <summary>
        /// Is the line renderer in world space.
        /// </summary>
        [CLProperty]
        public bool UseWorldSpace
        {
            get => Value.useWorldSpace;
            set => Value.useWorldSpace = value;
        }

        /// <summary>
        /// Does the line renderer cast shadows.
        /// </summary>
        [CLProperty]
        public string ShadowCastingMode
        {
            get => Value.shadowCastingMode.ToString();
            set => Value.shadowCastingMode = (UnityEngine.Rendering.ShadowCastingMode)System.Enum.Parse(typeof(UnityEngine.Rendering.ShadowCastingMode), value);
        }

        /// <summary>
        /// Does the line renderer receive shadows.
        /// </summary>
        [CLProperty]
        public bool ReceiveShadows
        {
            get => Value.receiveShadows;
            set => Value.receiveShadows = value;
        }

        /// <summary>
        /// The gradient of the line renderer.
        /// </summary>
        [CLProperty(TypeArguments = new[] { "Color" })]
        public CustomLogicListBuiltin ColorGradient
        {
            get
            {
                CustomLogicListBuiltin colors = new CustomLogicListBuiltin();
                foreach (var color in Value.colorGradient.colorKeys)
                {
                    colors.List.Add(new CustomLogicColorBuiltin(new Color255(color.color)));
                }
                return colors;
            }
            set
            {
                var colors = new List<GradientColorKey>();
                foreach (var color in value.List)
                {
                    // Assert type is cl color
                    if (!(color is CustomLogicColorBuiltin))
                    {
                        Debug.LogError("Invalid color type");
                        return;
                    }

                    CustomLogicColorBuiltin clColor = (CustomLogicColorBuiltin)color;

                    colors.Add(new GradientColorKey(clColor.Value.ToColor(), 0f));
                }
                Value.colorGradient.colorKeys = colors.ToArray();
            }
        }

        /// <summary>
        /// The alpha gradient of the line renderer.
        /// </summary>
        [CLProperty(TypeArguments = new[] { "float" })]
        public CustomLogicListBuiltin AlphaGradient
        {
            get
            {
                CustomLogicListBuiltin alphas = new CustomLogicListBuiltin();
                foreach (var alpha in Value.colorGradient.alphaKeys)
                {
                    alphas.List.Add(alpha.alpha);
                }
                return alphas;
            }
            set
            {
                var alphas = new List<GradientAlphaKey>();
                foreach (var alpha in value.List)
                {
                    // Assert type is float
                    if (!(alpha is float))
                    {
                        Debug.LogError("Invalid alpha type");
                        return;
                    }
                    alphas.Add(new GradientAlphaKey((float)alpha, (float)alpha));
                }
                Value.colorGradient.alphaKeys = alphas.ToArray();
            }
        }

        /// <summary>
        /// The width curve of the line renderer.
        /// </summary>
        [CLProperty(TypeArguments = new[] { "Vector2" })]
        public CustomLogicListBuiltin WidthCurve
        {
            get
            {
                CustomLogicListBuiltin curve = new CustomLogicListBuiltin();
                foreach (var point in Value.widthCurve.keys)
                {
                    curve.List.Add(new CustomLogicVector2Builtin(new Vector2(point.time, point.value)));
                }
                return curve;
            }
            set
            {
                var keys = new List<Keyframe>();
                foreach (var point in value.List)
                {
                    // Assert type is cl vector2
                    if (!(point is CustomLogicVector2Builtin))
                    {
                        Debug.LogError("Invalid point type");
                        return;
                    }
                    CustomLogicVector2Builtin clPoint = (CustomLogicVector2Builtin)point;
                    keys.Add(new Keyframe(clPoint.X, clPoint.Y));
                }
                Value.widthCurve.keys = keys.ToArray();
            }
        }

        /// <summary>
        /// The width multiplier of the line renderer.
        /// </summary>
        [CLProperty]
        public float WidthMultiplier
        {
            get => Value.widthMultiplier;
            set => Value.widthMultiplier = value;
        }

        /// <summary>
        /// The color gradient mode of the line renderer.
        /// </summary>
        [CLProperty]
        public string ColorGradientMode
        {
            get => Value.colorGradient.mode.ToString();
            set => Value.colorGradient.mode = (GradientMode)System.Enum.Parse(typeof(GradientMode), value);
        }

        /// <summary>
        /// Create a new LineRenderer.
        /// </summary>
        /// <returns>A new LineRenderer instance.</returns>
        [CLMethod, Obsolete("Create a new instance with LineRenderer() instead.")]
        public static CustomLogicLineRendererBuiltin CreateLineRenderer()
        {
            return new CustomLogicLineRendererBuiltin();
        }

        /// <summary>
        /// Get the position of a point in the line renderer.
        /// </summary>
        /// <param name="index">The index of the point (0 to PositionCount-1).</param>
        /// <returns>The position of the point.</returns>
        [CLMethod]
        public CustomLogicVector3Builtin GetPosition(int index)
        {
            return new CustomLogicVector3Builtin(Value.GetPosition(index));
        }

        /// <summary>
        /// Set the position of a point in the line renderer.
        /// </summary>
        /// <param name="index">The index of the point to set (0 to PositionCount-1).</param>
        /// <param name="position">The position to set.</param>
        [CLMethod]
        public void SetPosition(int index, CustomLogicVector3Builtin position)
        {
            Value.SetPosition(index, position.Value);
        }
    }
}
