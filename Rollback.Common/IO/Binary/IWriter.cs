namespace Rollback.Common.IO.Binary
{
    public interface IWriter : IDisposable
    {
        byte[] Buffer { get; }

        void ClearBuffer();

        void WriteBytes(byte[] bytes);

        void WriteByte(byte b);

        void WriteSByte(sbyte sb);

        void WriteBoolean(bool b);

        void WriteShort(short s);

        void WriteUShort(ushort us);

        void WriteInt(int i);

        void WriteUInt(uint ui);

        void WriteLong(long l);

        void WriteULong(ulong ul);

        void WriteFloat(float f);

        void WriteDouble(double d);

        void WriteUTFBytes(string s);

        void WriteString(string s);
    }
}
