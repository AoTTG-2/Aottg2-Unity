// http://wiki.unity3d.com/index.php/TextureScale

using UnityEngine;
using System.Collections;

// Only works on ARGB32, RGB24 and Alpha8 textures that are marked readable

using System.Threading;

public class TextureScaler
{
    public class ThreadData
    {
        public Color[] TexColors;
        public Color[] NewColors;
        public int TexWidth;
        public int TexHeight;
        public int NewWidth;
        public int NewHeight;

        public ThreadData(Color[] texColors, Color[] newColors, int texWidth, int texHeight, int newWidth, int newHeight)
        {
            TexColors = texColors;
            NewColors = newColors;
            TexWidth = texWidth;
            TexHeight = texHeight;
            NewWidth = newWidth;
            NewHeight = newHeight;
        }
    }

    public static IEnumerator Scale(Texture2D tex, int newWidth, int newHeight)
    {
        Color[] texColors = tex.GetPixels();
        Color[] newColors = new Color[newWidth * newHeight];
        ThreadData threadData = new ThreadData(texColors, newColors, tex.width, tex.height, newWidth, newHeight);
        ParameterizedThreadStart ts = new ParameterizedThreadStart(BilinearScale);
        Thread thread = new Thread(ts);
        thread.Start(threadData);
        while (thread.IsAlive)
            yield return new WaitForEndOfFrame();
        tex.Reinitialize(newWidth, newHeight);
        tex.SetPixels(newColors);
        yield return new WaitForEndOfFrame();
        tex.Apply();
    }

    public static void ScaleBlocking(Texture2D tex, int newWidth, int newHeight)
    {
        Color[] texColors = tex.GetPixels();
        Color[] newColors = new Color[newWidth * newHeight];
        ThreadData threadData = new ThreadData(texColors, newColors, tex.width, tex.height, newWidth, newHeight);
        ParameterizedThreadStart ts = new ParameterizedThreadStart(BilinearScale);
        Thread thread = new Thread(ts);
        thread.Start(threadData);
        while (thread.IsAlive)
            Thread.Sleep(1);
        tex.Reinitialize(newWidth, newHeight);
        tex.SetPixels(newColors);
        tex.Apply();
    }

    public static void BilinearScale(System.Object obj)
    {
        ThreadData threadData = (ThreadData)obj;
        float ratioX = 1.0f / ((float)threadData.NewWidth / (threadData.TexWidth - 1));
        float ratioY = 1.0f / ((float)threadData.NewHeight / (threadData.TexHeight - 1));
        int w = threadData.TexWidth;
        int w2 = threadData.NewWidth;
        for (var y = 0; y < threadData.NewHeight; y++)
        {
            int yFloor = (int)Mathf.Floor(y * ratioY);
            var y1 = yFloor * w;
            var y2 = (yFloor + 1) * w;
            var yw = y * w2;

            for (var x = 0; x < w2; x++)
            {
                int xFloor = (int)Mathf.Floor(x * ratioX);
                var xLerp = x * ratioX - xFloor;
                var texColors = threadData.TexColors;
                threadData.NewColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor + 1], xLerp),
                                                       ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor + 1], xLerp),
                                                       y * ratioY - yFloor);
            }
        }
    }

    private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
    {
        return new Color(c1.r + (c2.r - c1.r) * value,
                          c1.g + (c2.g - c1.g) * value,
                          c1.b + (c2.b - c1.b) * value,
                          c1.a + (c2.a - c1.a) * value);
    }
}