namespace Utility
{
    static class StringCompression
    {
        public static byte[] Compress(string str)
        {
            return DataCompressors.Brotli.CompressString(str);
        }

        public static string Decompress(byte[] data)
        {
            return DataCompressors.Brotli.DecompressString(data);
        }
    }
}