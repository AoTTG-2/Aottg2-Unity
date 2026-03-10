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
            return CLZF2.Compress(data);
        }

        public override byte[] Decompress(byte[] data, int bufferSize = DefaultBufferSize, long maxSize = DefaultMaxSize)
        {
            return CLZF2.Decompress(data);
        }
    }
}
