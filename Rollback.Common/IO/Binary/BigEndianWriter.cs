using System.Buffers.Binary;
using System.Text;

namespace Rollback.Common.IO.Binary
{
    public class BigEndianWriter : IWriter
    {
        private MemoryStream _stream;

        public BigEndianWriter() =>
            _stream = new MemoryStream();

        public byte[] Buffer =>
            _stream.ToArray();

        public void ClearBuffer() =>
            _stream = new();

        /// <summary>
        /// It doesn't take care of the endianess
        /// </summary>
        public void WriteBytes(byte[] bytes) =>
            _stream.Write(bytes);

        public void WriteByte(byte b) =>
            _stream.WriteByte(b);

        public void WriteSByte(sbyte sb) =>
            _stream.WriteByte((byte)sb);

        public void WriteBoolean(bool b) =>
            _stream.WriteByte(Convert.ToByte(b));

        public void WriteShort(short s)
        {
            var buffer = new byte[sizeof(short)];
            BinaryPrimitives.WriteInt16BigEndian(buffer, s);

            _stream.Write(buffer);
        }

        public void WriteUShort(ushort us)
        {
            var buffer = new byte[sizeof(ushort)];
            BinaryPrimitives.WriteUInt16BigEndian(buffer, us);

            _stream.Write(buffer);
        }

        public void WriteInt(int i)
        {
            var buffer = new byte[sizeof(int)];
            BinaryPrimitives.WriteInt32BigEndian(buffer, i);

            _stream.Write(buffer);
        }

        public void WriteUInt(uint ui)
        {
            var buffer = new byte[sizeof(uint)];
            BinaryPrimitives.WriteUInt32BigEndian(buffer, ui);

            _stream.Write(buffer);
        }

        public void WriteLong(long l)
        {
            var buffer = new byte[sizeof(long)];
            BinaryPrimitives.WriteInt64BigEndian(buffer, l);

            _stream.Write(buffer);
        }

        public void WriteULong(ulong ul)
        {
            var buffer = new byte[sizeof(ulong)];
            BinaryPrimitives.WriteUInt64BigEndian(buffer, ul);

            _stream.Write(buffer);
        }

        public void WriteFloat(float f)
        {
            var buffer = new byte[sizeof(float)];
            BinaryPrimitives.WriteSingleBigEndian(buffer, f);

            _stream.Write(buffer);
        }

        public void WriteDouble(double d)
        {
            var buffer = new byte[sizeof(double)];
            BinaryPrimitives.WriteDoubleBigEndian(buffer, d);

            _stream.Write(buffer);
        }

        public void WriteUTFBytes(string s)
        {
            var buffer = Encoding.UTF8.GetBytes(s);
            _stream.Write(buffer);
        }

        public void WriteString(string s)
        {
            var buffer = Encoding.UTF8.GetBytes(s);

            WriteUShort((ushort)buffer.Length);
            _stream.Write(buffer);
        }

        public void Dispose()
        {
            _stream.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
