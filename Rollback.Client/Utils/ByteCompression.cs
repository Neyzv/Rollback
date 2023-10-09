using System.IO.Compression;

namespace Rollback.Client.Utils
{
    internal static class ByteCompression
    {
        internal static MemoryStream Decompress(MemoryStream input)
        {
            var outputStream = new MemoryStream();

            using (var inputStream = new ZLibStream(input, CompressionMode.Decompress))
                inputStream.CopyTo(outputStream);

            outputStream.Position = 0;

            return outputStream;
        }

        internal static byte[] Compress(byte[] content)
        {
            var outputStream = new MemoryStream();

            using (var outZStream = new ZLibStream(outputStream, CompressionLevel.SmallestSize))
                outZStream.Write(content);

            return outputStream.ToArray();
        }
    }
}
