using Utility.Algorithms;

namespace Utility
{
    static class DataCompressors
    {
        /// <summary>
        /// A singleton instance for using the <c>Brotli</c> (de-)compression algorithm
        /// </summary>
        public static readonly BrotliAlgorithm Brotli = new BrotliAlgorithm();

        /// <summary>
        /// A singleton instance for using the <c>GZip</c> (de-)compression algorithm
        /// </summary>
        public static readonly GZipAlgorithm GZip = new GZipAlgorithm();

        /// <summary>
        /// A singleton instance for using the <c>ZLib</c> (de-)compression algorithm
        /// </summary>
        public static readonly DeflateAlgorithm ZLib = new DeflateAlgorithm();

        /// <summary>
        /// A singleton instance for using the <c>LZF</c> (de-)compression algorithm
        /// </summary>
        public static readonly LZFAlgorithm LZF = new LZFAlgorithm();
    }
}
