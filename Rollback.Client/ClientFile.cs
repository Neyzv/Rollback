using Rollback.Client.Dlm;
using Rollback.Client.Ele;
using Rollback.Client.Utils;
using Rollback.Common.IO.Binary;

namespace Rollback.Client
{
    public static class ClientFile
    {
        private const sbyte dlmHeader = 77;
        private const sbyte eleHeader = 69;

        private static BigEndianReader CreateReader(string filePath, sbyte fileConst)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            var ms = new MemoryStream(File.ReadAllBytes(filePath));
            var reader = new BigEndianReader(ms);

            if (reader.ReadSByte() != fileConst)
            {
                reader.Seek(0);

                try
                {
                    reader = new(ByteCompression.Decompress(ms));
                }
                catch
                {
                    throw new Exception("Wrong header and can not be uncompressed...");
                }

                if (reader.ReadSByte() != fileConst)
                    throw new Exception("Wrong header file...");
            }

            return reader;
        }

        public static Map ReadDlm(string filePath)
        {
            using var reader = CreateReader(filePath, dlmHeader);

            return Map.FromRaw(reader);
        }

        public static void WriteDlm(Map map, string filePath)
        {
            using var writer = new BigEndianWriter();
            writer.WriteSByte(dlmHeader);
            map.Serialize(writer);

            File.WriteAllBytes(filePath, ByteCompression.Compress(writer.Buffer));
        }

        public static Elements ReadEle(string filePath)
        {
            using var reader = CreateReader(filePath, eleHeader);

            return Elements.FromRaw(reader);
        }

        public static void WriteEle(Elements ele, string filePath)
        {
            using var writer = new BigEndianWriter();
            writer.WriteSByte(eleHeader);
            ele.Serialize(writer);

            File.WriteAllBytes(filePath, ByteCompression.Compress(writer.Buffer));
        }
    }
}
