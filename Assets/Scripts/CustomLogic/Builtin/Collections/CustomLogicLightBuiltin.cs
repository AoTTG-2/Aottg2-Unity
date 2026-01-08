using Map;
using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Represents a Light component that can be directional, point, or spot light with configurable properties.
    /// </summary>
    [CLType(Name = "LightBuiltin", Static = true, Abstract = true)]
    partial class CustomLogicLightBuiltin : BuiltinComponentInstance
    {
        public Light Value;
        public CustomLogicMapObjectBuiltin OwnerMapObject;
        public GameObject Owner;

        /// <summary>
        /// Creates an empty light instance.
        /// </summary>
        [CLConstructor]
        public CustomLogicLightBuiltin() : base(null) { }

        public CustomLogicLightBuiltin(CustomLogicMapObjectBuiltin owner, LightType type) : base(GetOrAddComponent<Light>(owner.Value.GameObject))
        {
            OwnerMapObject = owner;
            Owner = owner.Value.GameObject;
            Value = (Light)Component;
            Value.type = type;
            Value.shadowBias = 0.2f;
            Value.intensity = 1f;
            Value.shadows = LightShadows.Soft;
            Value.shadowStrength = 0.8f;
            MapLoader.RegisterMapLight(Value, type == LightType.Directional);
        }

        /// <summary>
        /// LightType.Directional.
        /// </summary>
        [CLProperty]
        public static int LightTypeDirectional => (int)LightType.Directional;

        /// <summary>
        /// LightType.Point.
        /// </summary>
        [CLProperty]
        public static int LightTypePoint => (int)LightType.Point;

        /// <summary>
        /// LightType.Spot.
        /// </summary>
        [CLProperty]
        public static int LightTypeSpot => (int)LightType.Spot;

        /// <summary>
        /// LightShadows.None.
        /// </summary>
        [CLProperty]
        public static int ShadowTypeNone => (int)LightShadows.None;

        /// <summary>
        /// LightShadows.Hard.
        /// </summary>
        [CLProperty]
        public static int ShadowTypeHard => (int)LightShadows.Hard;

        /// <summary>
        /// LightShadows.Soft.
        /// </summary>
        [CLProperty]
        public static int ShadowTypeSoft => (int)LightShadows.Soft;

        /// <summary>
        /// The type of the light.
        /// </summary>
        [CLProperty]
        public int TypeOfLight
        {
            get => (int)Value.type;
            set => Value.type = (LightType)value;
        }

        /// <summary>
        /// The range of the light.
        /// </summary>
        [CLProperty]
        public float Range
        {
            get => Value.range;
            set => Value.range = value;
        }

        /// <summary>
        /// The spot angle of the light, works on spot lights only.
        /// </summary>
        [CLProperty]
        public float SpotAngle
        {
            get => Value.spotAngle;
            set => Value.spotAngle = value;
        }

        /// <summary>
        /// The color of the light.
        /// </summary>
        [CLProperty]
        public CustomLogicColorBuiltin Color
        {
            get => new CustomLogicColorBuiltin(Value.color);
            set => Value.color = value.Value.ToColor();
        }

        /// <summary>
        /// The intensity of the light.
        /// </summary>
        [CLProperty]
        public float Intensity
        {
            get => Value.intensity;
            set => Value.intensity = value;
        }

        /// <summary>
        /// The bounce intensity of the light.
        /// </summary>
        [CLProperty]
        public float BounceIntensity
        {
            get => Value.bounceIntensity;
            set => Value.bounceIntensity = value;
        }

        /// <summary>
        /// The shadow type of the light (Soft, None, Hard).
        /// </summary>
        [CLProperty]
        public int ShadowType
        {
            get => (int)Value.shadows;
            set => Value.shadows = (LightShadows)value;
        }

        /// <summary>
        /// The shadow strength of the light.
        /// </summary>
        [CLProperty]
        public float ShadowStrength
        {
            get => Value.shadowStrength;
            set => Value.shadowStrength = value;
        }

        private bool _weatherControlled = false;

        /// <summary>
        /// The light is controlled by the weather system.
        /// </summary>
        [CLProperty]
        public bool WeatherControlled
        {
            get => _weatherControlled;
            set
            {
                _weatherControlled = value;
                if (value && !MapLoader.Daylight.Contains(Value))
                {
                    MapLoader.Daylight.Add(Value);
                }
                else if (!value)
                {
                    MapLoader.Daylight.Remove(Value);
                }
            }
        }
    }
}
