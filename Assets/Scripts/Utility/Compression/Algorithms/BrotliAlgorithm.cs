using System;
using System.IO;
using System.IO.Compression;

namespace Utility.Algorithms
{
    /// <summary>
    /// A data compressor for the <c>brotli</c> compression algorithm, utilizing the <see cref="System.IO.Compression.BrotliStream"/> class
    /// </summary>
    internal class BrotliAlgorithm : CompressionAlgorithm
    {
        public override byte[] Compress(byte[] data, CompressionLevel level = CompressionLevel.Fastest)
        {
            try
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    using (Stream compressor = new BrotliStream(memory, level, true))
                    {
                        compressor.Write(data, 0, data.Length);
                    }

                    return memory.ToArray();
                }
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

        public override byte[] Decompress(byte[] data, int bufferSize = DefaultBufferSize, long maxSize = DefaultMaxSize)
        {
            try
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    using (Stream decompressor = new BrotliStream(new MemoryStream(data), CompressionMode.Decompress))
                    {
                        byte[] buffer = new byte[bufferSize];
                        long decompressed = 0;
                        int read;

                        while ((read = decompressor.Read(buffer, 0, bufferSize)) > 0)
                        {
                            decompressed += read;
                            if (decompressed > maxSize)
                            {
                                throw new IOException($"Decompressed size exceeded maximum allowed ({decompressed} > {maxSize})");
                            }

                            memory.Write(buffer, 0, read);
                        }
                    }

                    return memory.ToArray();
                }
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }
    }
}
