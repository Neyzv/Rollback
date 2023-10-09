using Rollback.Common.IO.Binary;

namespace Rollback.Common.Network.Protocol
{
    public abstract record Message : IMessage
    {
        public const byte headerSize = 2;
        public const byte bitShift = 3;

        public abstract uint MessageId { get; }

        public abstract void Serialize(BigEndianWriter writer);

        public abstract void Deserialize(BigEndianReader reader);

        public void Pack(BigEndianWriter writer)
        {
            Serialize(writer);

            byte[] datas = writer.Buffer;
            writer.ClearBuffer();

            var typeLen = ComputeTypeLen((uint)datas.Length);
            writer.WriteShort(ComputeStaticHeader(MessageId, typeLen));

            switch (typeLen)
            {
                case 1:
                    writer.WriteByte((byte)datas.Length);
                    break;
                case 2:
                    writer.WriteShort((short)datas.Length);
                    break;
                case 3:
                    writer.WriteByte((byte)(datas.Length >> sizeof(decimal) & byte.MaxValue));
                    writer.WriteShort((short)(datas.Length & ushort.MaxValue));
                    break;
                default:
                    return;
            }

            writer.WriteBytes(datas);
        }

        public bool UnPack(BigEndianReader reader, byte typeLen)
        {
            int bodyBytesCount = typeLen;

            var j = 0;
            for (int i = bodyBytesCount - 1; i >= 0; i--, j++)
                bodyBytesCount |= reader.ReadByte() << (i * 8);

            if (bodyBytesCount != 0 && bodyBytesCount - j > reader.BytesAvailable)
                return false;

            Deserialize(reader);

            return true;
        }

        private static byte ComputeTypeLen(uint len) => len switch
        {
            > ushort.MaxValue => 3,
            > byte.MaxValue => 2,
            > 0 => 1,
            _ => 0,
        };

        private static short ComputeStaticHeader(uint msgId, uint typeLen) =>
            (short)(msgId << headerSize | typeLen);
    }
}
