using ApplicationManagers;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    [CLType(Name = "LineRenderer", Static = true)]
    partial class CustomLogicLineRendererBuiltin : BuiltinClassInstance
    {
        public LineRenderer Value = null;

        [CLConstructor]
        public CustomLogicLineRendererBuiltin(object[] parameterValues)
        {
            Value = new GameObject().AddComponent<LineRenderer>();
            Value.material = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Map, "Materials/TransparentMaterial", true);
            Value.material.color = Color.black;
            Value.startWidth = 1f;
            Value.endWidth = 1f;
            Value.positionCount = 0;
            Value.enabled = false;
        }

        public CustomLogicLineRendererBuiltin(LineRenderer value)
        {
            Value = value;
        }

        [CLProperty(description: "The width of the line at the start")]
        public float StartWidth
        {
            get => Value.startWidth;
            set => Value.startWidth = value;
        }

        [CLProperty(description: "The width of the line at the end")]
        public float EndWidth
        {
            get => Value.endWidth;
            set => Value.endWidth = value;
        }

        [CLProperty(description: "The color of the line")]
        public CustomLogicColorBuiltin LineColor
        {
            get => new CustomLogicColorBuiltin(new Color255(Value.material.color));
            set => Value.material.color = value.Value.ToColor();
        }

        [CLProperty(description: "The number of points in the line")]
        public int PositionCount
        {
            get => Value.positionCount;
            set => Value.positionCount = value;
        }

        [CLProperty(description: "Is the line renderer enabled")]
        public bool Enabled
        {
            get => Value.enabled;
            set => Value.enabled = value;
        }

        // Loop
        [CLProperty(description: "Is the line renderer a loop")]
        public bool Loop
        {
            get => Value.loop;
            set => Value.loop = value;
        }

        // Corner Vertices
        [CLProperty(description: "The number of corner vertices")]
        public int NumCornerVertices
        {
            get => Value.numCornerVertices;
            set => Value.numCornerVertices = value;
        }

        // End Cap Vertices
        [CLProperty(description: "The number of end cap vertices")]
        public int NumCapVertices
        {
            get => Value.numCapVertices;
            set => Value.numCapVertices = value;
        }

        // Alignment
        [CLProperty(description: "The alignment of the line renderer")]
        public string Alignment
        {
            get => Value.alignment.ToString();
            set => Value.alignment = (LineAlignment)System.Enum.Parse(typeof(LineAlignment), value);
        }

        // Texture Mode
        [CLProperty(description: "The texture mode of the line renderer")]
        public string TextureMode
        {
            get => Value.textureMode.ToString();
            set => Value.textureMode = (LineTextureMode)System.Enum.Parse(typeof(LineTextureMode), value);
        }

        // World Space
        [CLProperty(description: "Is the line renderer in world space")]
        public bool UseWorldSpace
        {
            get => Value.useWorldSpace;
            set => Value.useWorldSpace = value;
        }

        // Cast Shadows
        [CLProperty(description: "Does the line renderer cast shadows")]
        public string ShadowCastingMode
        {
            get => Value.shadowCastingMode.ToString();
            set => Value.shadowCastingMode = (UnityEngine.Rendering.ShadowCastingMode)System.Enum.Parse(typeof(UnityEngine.Rendering.ShadowCastingMode), value);
        }

        // Receive Shadows
        [CLProperty(description: "Does the line renderer receive shadows")]
        public bool ReceiveShadows
        {
            get => Value.receiveShadows;
            set => Value.receiveShadows = value;
        }

        // Gradient
        [CLProperty(description: "The gradient of the line renderer")]
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

        // Alpha Gradient
        [CLProperty(description: "The alpha gradient of the line renderer")]
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

        // Width Curve use a list of vector2's to create then animation curve
        [CLProperty(description: "The width curve of the line renderer")]
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

        // Multiplier
        [CLProperty(description: "The width multiplier of the line renderer")]
        public float WidthMultiplier
        {
            get => Value.widthMultiplier;
            set => Value.widthMultiplier = value;
        }

        // Color Gradient Mode
        [CLProperty(description: "The color gradient mode of the line renderer")]
        public string ColorGradientMode
        {
            get => Value.colorGradient.mode.ToString();
            set => Value.colorGradient.mode = (GradientMode)System.Enum.Parse(typeof(GradientMode), value);
        }

        [CLMethod(description: "Create a new LineRenderer"), Obsolete("Create a new instance with LineRenderer() instead.")]
        public static CustomLogicLineRendererBuiltin CreateLineRenderer()
        {
            GameObject obj = new GameObject();
            var renderer = obj.AddComponent<LineRenderer>();
            renderer.material = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Map, "Materials/TransparentMaterial", true);
            renderer.material.color = Color.black;
            renderer.startWidth = 1f;
            renderer.endWidth = 1f;
            renderer.positionCount = 0;
            renderer.enabled = false;
            return new CustomLogicLineRendererBuiltin(renderer);
        }

        [CLMethod(description: "Get the position of a point in the line renderer")]
        public CustomLogicVector3Builtin GetPosition(int index)
        {
            return new CustomLogicVector3Builtin(Value.GetPosition(index));
        }

        [CLMethod(description: "Set the position of a point in the line renderer")]
        public void SetPosition(int index, CustomLogicVector3Builtin position)
        {
            Value.SetPosition(index, position.Value);
        }
    }
}
