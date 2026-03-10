using System;
using System.IO.Compression;
using System.Text;

namespace Utility.Algorithms
{
    /// <summary>
    /// An abstract class for implementing data compression algorithms
    /// </summary>
    abstract class CompressionAlgorithm
    {
        /// <summary>
        /// The default buffer size used during decompression, in scale of bytes.
        /// This value is currently set to 4KB
        /// </summary>
        public const int DefaultBufferSize = 1024 * 4; // Remember to update documentation above if you change this value!
        /// <summary>
        /// The default maximum allowed length of bytes to be processed during decompression, in scale of bytes.
        /// This value is currently set to 10MB
        /// </summary>
        public const int DefaultMaxSize = 1024 * 1024 * 10; // Remember to update documentation above if you change this value!

        /// <summary>
        /// Returns a new byte array constructed by compressing <paramref name="data"/>
        /// </summary>
        /// <param name="data">input byte array</param>
        /// <param name="level">(optional) level of compression to use</param>
        /// <returns>the compressed form of <paramref name="data"/></returns>
        public abstract byte[] Compress(byte[] data, CompressionLevel level = CompressionLevel.Fastest);

        /// <summary>
        /// Returns the byte array constructed by decompressing <paramref name="data"/>
        /// </summary>
        /// <param name="data">input byte array</param>
        /// <param name="bufferSize">amount of bytes to read per read</param>
        /// <param name="maxSize">max amount of bytes to be processed</param>
        /// <returns>the original byte array from <paramref name="data"/></returns>
        public abstract byte[] Decompress(byte[] data, int bufferSize = DefaultBufferSize, long maxSize = DefaultMaxSize);

        /// <summary>
        /// After converting <paramref name="str"/> to an array of UTF-8 encoded bytes, this method returns the value of <see cref="Compress(byte[], CompressionLevel)"/>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="level"></param>
        /// <returns>the compressed form of <paramref name="str"/></returns>
        public byte[] CompressString(string str, CompressionLevel level = CompressionLevel.Fastest)
        {
            if (string.IsNullOrEmpty(str))
            {
                return Array.Empty<byte>();
            }

            return Compress(Encoding.UTF8.GetBytes(str), level);
        }

        /// <summary>
        /// Returns a UTF-8 encoded string constructed after calling <see cref="Decompress(byte[], int, long)"/>
        /// </summary>
        /// <param name="data">input byte array</param>
        /// <param name="bufferSize">amount of bytes to read per read</param>
        /// <param name="maxSize">max amount of bytes to be processed</param>
        /// <returns>the original UTF-8 string from <paramref name="data"/></returns>
        public string DecompressString(byte[] data, int bufferSize = DefaultBufferSize, long maxSize = DefaultMaxSize)
        {
            if (data == null || data.Length == 0)
            {
                return string.Empty;
            }

            return Encoding.UTF8.GetString(Decompress(data, bufferSize, maxSize));
        }
    }
}
