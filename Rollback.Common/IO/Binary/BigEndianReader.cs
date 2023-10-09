using System.Buffers.Binary;
using System.Text;

namespace Rollback.Common.IO.Binary
{
    public class BigEndianReader : IReader
    {
        private readonly MemoryStream _stream;

        public BigEndianReader(byte[] buffer) =>
            _stream = new MemoryStream(buffer);

        public BigEndianReader(MemoryStream stream) =>
            _stream = stream;

        public long BytesAvailable =>
            _stream.Length - _stream.Position;

        public long Position =>
            _stream.Position;

        /// <summary>
        /// Read bytes with out taking care of the endianness
        /// </summary>
        public byte[] ReadBytes(int count)
        {
            var buffer = new byte[count];
            _stream.Read(buffer, 0, count);

            return buffer;
        }

        public byte ReadByte() =>
            (byte)_stream.ReadByte();

        public sbyte ReadSByte() =>
            (sbyte)_stream.ReadByte();

        public bool ReadBoolean() =>
            _stream.ReadByte() is 1;

        public short ReadShort()
        {
            var buffer = new byte[sizeof(short)];
            _stream.Read(buffer);

            return BinaryPrimitives.ReadInt16BigEndian(buffer);
        }

        public ushort ReadUShort()
        {
            var buffer = new byte[sizeof(ushort)];
            _stream.Read(buffer);

            return BinaryPrimitives.ReadUInt16BigEndian(buffer);
        }

        public int ReadInt()
        {
            var buffer = new byte[sizeof(int)];
            _stream.Read(buffer);

            return BinaryPrimitives.ReadInt32BigEndian(buffer);
        }

        public uint ReadUInt()
        {
            var buffer = new byte[sizeof(uint)];
            _stream.Read(buffer);

            return BinaryPrimitives.ReadUInt32BigEndian(buffer);
        }

        public long ReadLong()
        {
            var buffer = new byte[sizeof(long)];
            _stream.Read(buffer);

            return BinaryPrimitives.ReadInt64BigEndian(buffer);
        }

        public ulong ReadULong()
        {
            var buffer = new byte[sizeof(ulong)];
            _stream.Read(buffer);

            return BinaryPrimitives.ReadUInt64BigEndian(buffer);
        }

        public float ReadFloat()
        {
            var buffer = new byte[sizeof(float)];
            _stream.Read(buffer);

            return BinaryPrimitives.ReadSingleBigEndian(buffer);
        }

        public double ReadDouble()
        {
            var buffer = new byte[sizeof(double)];
            _stream.Read(buffer);

            return BinaryPrimitives.ReadDoubleBigEndian(buffer);
        }

        public string ReadUTFBytes(int count)
        {
            var buffer = new byte[count];
            _stream.Read(buffer);

            return Encoding.UTF8.GetString(buffer);
        }

        public string ReadString() =>
            ReadUTFBytes(ReadUShort());

        public void Seek(int offset, SeekOrigin origin = SeekOrigin.Begin) =>
            _stream.Seek(offset, origin);

        public void Dispose()
        {
            _stream.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
