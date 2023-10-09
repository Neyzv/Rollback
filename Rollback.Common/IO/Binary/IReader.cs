namespace Rollback.Common.IO.Binary
{
    public interface IReader : IDisposable
    {
        long BytesAvailable { get; }

        long Position { get; }

        byte[] ReadBytes(int count);

        byte ReadByte();

        sbyte ReadSByte();

        bool ReadBoolean();

        short ReadShort();

        ushort ReadUShort();

        int ReadInt();

        uint ReadUInt();

        long ReadLong();

        ulong ReadULong();

        float ReadFloat();

        double ReadDouble();

        string ReadUTFBytes(int count);

        string ReadString();

        void Seek(int offset, SeekOrigin origin = SeekOrigin.Begin);
    }
}
