using UnityEngine;

namespace Utility
{
    public class Color255
    {
        public int R;
        public int G;
        public int B;
        public int A;

        public Color255(int r, int g, int b, int a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color255()
        {
            R = G = B = A = 255;
        }

        public Color255(Color color)
        {
            R = (int)(color.r * 255);
            G = (int)(color.g * 255);
            B = (int)(color.b * 255);
            A = (int)(color.a * 255);
        }

        public Color ToColor()
        {
            return new Color(R / 255f, G / 255f, B / 255f, A / 255f);
        }

        public static Color255 Lerp(Color255 from, Color255 to, float t)
        {
            int r = (int)Mathf.Lerp(from.R, to.R, t);
            int g = (int)Mathf.Lerp(from.G, to.G, t);
            int b = (int)Mathf.Lerp(from.B, to.B, t);
            int a = (int)Mathf.Lerp(from.A, to.A, t);
            return new Color255(r, g, b, a);
        }

        public static Color255 Gradient(GradientColorKey[] colorKeys, GradientAlphaKey[] alphakeys, GradientMode mode, float t)
        {
            var gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphakeys);
            return new Color255(gradient.Evaluate(t));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Color255))
                return false;
            var other = (Color255)obj;
            return R == other.R && G == other.G && B == other.B && A == other.A;
        }

        public override int GetHashCode()
        {
            return (R, G, B, A).GetHashCode();
        }
    }
}
