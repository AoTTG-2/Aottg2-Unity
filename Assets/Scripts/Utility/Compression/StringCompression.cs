using System;
using System.Text;

namespace Utility
{
    static class StringCompression
    {
        public static byte[] Compress(string text)
        {
            if (text == string.Empty)
                return new byte[0];
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return CLZF2.Compress(bytes);
        }

        public static string Decompress(byte[] data)
        {
            if (data.Length == 0)
                return string.Empty;
            return Encoding.UTF8.GetString(CLZF2.Decompress(data));
        }

        public static string CompressBase64(string text)
        {
            if (text == string.Empty)
                return string.Empty;
            return Convert.ToBase64String(Compress(text));
        }

        public static string DecompressBase64(string data)
        {
            if (data == string.Empty)
                return string.Empty;
            return Decompress(Convert.FromBase64String(data));
        }
    }
}
