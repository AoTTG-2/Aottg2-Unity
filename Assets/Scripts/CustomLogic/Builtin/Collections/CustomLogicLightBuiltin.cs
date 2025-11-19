using Map;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "LightBuiltin", Static = true, Abstract = true, Description = "")]
    partial class CustomLogicLightBuiltin : BuiltinComponentInstance
    {
        public Light Value;
        public CustomLogicMapObjectBuiltin OwnerMapObject;
        public GameObject Owner;

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
            Value.shadowBias = 0.2f;
            MapLoader.RegisterMapLight(Value, type == LightType.Directional);
        }

        [CLProperty(Static = true, Description = "LightType.Directional")]
        public static int LightTypeDirectional => (int)LightType.Directional;

        [CLProperty(Static = true, Description = "LightType.Point")]
        public static int LightTypePoint => (int)LightType.Point;

        [CLProperty(Static = true, Description = "LightType.Point")]
        public static int LightTypeSpot => (int)LightType.Spot;

        [CLProperty(Static = true, Description = "LightShadows.None")]
        public static int ShadowTypeNone => (int)LightShadows.None;

        [CLProperty(Static = true, Description = "LightShadows.Hard")]
        public static int ShadowTypeHard => (int)LightShadows.Hard;

        [CLProperty(Static = true, Description = "LightShadows.Soft")]
        public static int ShadowTypeSoft => (int)LightShadows.Soft;

        [CLProperty(Description = "The type of the light.")]
        public int TypeOfLight
        {
            get => (int)Value.type;
            set => Value.type = (LightType)value;
        }

        [CLProperty(Description = "The range of the light.")]
        public float Range
        {
            get => Value.range;
            set => Value.range = value;
        }

        [CLProperty(Description = "The spot angle of the light, works on spot lights only.")]
        public float SpotAngle
        {
            get => Value.spotAngle;
            set => Value.spotAngle = value;
        }

        [CLProperty(Description = "The color of the light.")]
        public CustomLogicColorBuiltin Color
        {
            get => new CustomLogicColorBuiltin(Value.color);
            set => Value.color = value.Value.ToColor();
        }

        [CLProperty(Description = "The intensity of the light.")]
        public float Intensity
        {
            get => Value.intensity;
            set => Value.intensity = value;
        }

        // Bounce Intensity
        [CLProperty(Description = "The bounce intensity of the light.")]
        public float BounceIntensity
        {
            get => Value.bounceIntensity;
            set => Value.bounceIntensity = value;
        }

        [CLProperty(Description = "The shadow type of the light (Soft, None, Hard).")]
        public int ShadowType
        {
            get => (int)Value.shadows;
            set => Value.shadows = (LightShadows)value;
        }

        [CLProperty(Description = "The shadow strength of the light.")]
        public float ShadowStrength
        {
            get => Value.shadowStrength;
            set => Value.shadowStrength = value;
        }

        // Weather controlled prop
        private bool _weatherControlled = false;
        [CLProperty(Description = "The light is controlled by the weather system.")]
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
