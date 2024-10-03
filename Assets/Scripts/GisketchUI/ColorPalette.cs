using UnityEngine;

namespace GisketchUI
{
    public static class ColorPalette
    {
        public static readonly Color Blue = new Color(0.1254902f, 0.3960784f, 0.627451f);    // #2065a0
        public static readonly Color Orange = new Color(0.7294118f, 0.4f, 0.1215686f);       // #ba661f
        public static readonly Color Green = new Color(0.1607843f, 0.5333334f, 0.5411765f);  // #29888a
        public static readonly Color Red = new Color(0.5058824f, 0.2392157f, 0.3215686f);    // #813d52
        public static readonly Color Purple = new Color(0.3803922f, 0.2980392f, 0.5647059f); // #614c90

        public static readonly Color RedLight = new Color(0.8431373f, 0.2784314f, 0.3686275f); // #d7475e

        public static readonly Color Primary = Red;
        public static readonly Color Secondary = Purple;
        public static readonly Color Tertiary = Blue;

        public static readonly Color PrimaryLight = RedLight;

        // Neutrals
        public static readonly Color White = Color.white;
        public static readonly Color Black = new Color(0.0588235f, 0.0588235f, 0.0588235f);  // #0F0F0F
    }
}