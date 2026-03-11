using System;
using System.IO.Compression;

namespace Utility.Algorithms
{
    /// <summary>
    /// A data compressor for the <c>lzf</c> compression algorithm, utilizing our <see cref="CLZF2"/> class
    /// </summary>
    internal class LZFAlgorithm : CompressionAlgorithm
    {
        public override byte[] Compress(byte[] data, CompressionLevel level = CompressionLevel.Fastest)
        {
            // Starting guess, increase it later if needed
            int sizeGuess = data.Length * 2;
            byte[] buffer = new byte[sizeGuess];
            int size = CLZF2.lzf_compress(data, ref buffer);

            // If byteCount is 0, then increase buffer and try again
            while (size == 0)
            {
                sizeGuess *= 2;
                buffer = new byte[sizeGuess];
                size = CLZF2.lzf_compress(data, ref buffer);
            }

            byte[] output = new byte[size];
            Buffer.BlockCopy(buffer, 0, output, 0, size);
            return output;
        }

        public override byte[] Decompress(byte[] data, int bufferSize = DefaultBufferSize, long maxSize = DefaultMaxSize)
        {
            // Starting guess, increase it later if needed
            int sizeGuess = data.Length * 2;
            byte[] buffer = new byte[sizeGuess];
            int size = CLZF2.lzf_decompress(data, ref buffer);

            // If byteCount is 0, then increase buffer and try again
            while (size == 0)
            {
                sizeGuess += bufferSize;
                if (sizeGuess > maxSize)
                {
                    return Array.Empty<byte>();
                }

                buffer = new byte[sizeGuess];
                size = CLZF2.lzf_decompress(data, ref buffer);
                if (size < -100) // return code < -100 = error
                {
                    return Array.Empty<byte>();
                }
            }

            byte[] output = new byte[size];
            Buffer.BlockCopy(buffer, 0, output, 0, size);
            return output;
        }
    }
}
