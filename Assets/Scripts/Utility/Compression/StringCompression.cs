using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Utility
{
    static class StringCompression
    {
        // Lengths in order of Bytes * Kilobytes * Megabytes [ * etc... ], produces final length in unit of Bytes.
        private const int BufferLength = 1024 * 4; // 4KB
        private const long MaxContentLength = 1024 * 1024 * 8; // 8MB

        public static byte[] Compress(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Array.Empty<byte>();
            }

            BrotliStream brotli = null;

            try
            {
                MemoryStream stream = new MemoryStream(); // TRACKING: Disposed with "brotli" automatically.
                brotli = new BrotliStream(stream, CompressionLevel.Fastest);

                byte[] content = Encoding.UTF8.GetBytes(text);
                brotli.Write(content, 0, content.Length);
                brotli.Flush();

                return stream.ToArray();
            }
            catch // On failure, return empty array.
            {
                return Array.Empty<byte>();
            }
            finally
            {
                brotli?.Dispose();
            }
        }

        public static string Decompress(byte[] content)
        {
            if (content == null || content.Length == 0)
            {
                return string.Empty;
            }

            BrotliStream brotli = null;
            MemoryStream output = null;

            try
            {
                MemoryStream stream = new MemoryStream(content); // TRACKING: Disposed with "brotli" automatically.
                brotli = new BrotliStream(stream, CompressionMode.Decompress);
                output = new MemoryStream();

                byte[] buffer = new byte[BufferLength];
                long contentLength = 0;
                int readLength;

                while ((readLength = brotli.Read(buffer, 0, BufferLength)) > 0)
                {
                    contentLength += readLength;
                    if (contentLength > MaxContentLength) // Uh oh! This content is too large for us to be handling.
                    {
                        return string.Empty;
                    }

                    output.Write(buffer, 0, readLength);
                }

                return Encoding.UTF8.GetString(output.ToArray());
            }
            catch // On failure, return empty string.
            {
                return string.Empty;
            }
            finally
            {
                // "stream" gets disposed automatically when "brotli" is disposed.
                brotli?.Dispose();
                output?.Dispose();
            }
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