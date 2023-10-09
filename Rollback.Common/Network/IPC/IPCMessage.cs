using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Common.Network.IPC
{
    public abstract record IPCMessage : Message
    {
        public int RequestId { get; set; } = -1;

        protected abstract void InternalSerialize(BigEndianWriter writer);

        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(RequestId);
            InternalSerialize(writer);
        }

        protected abstract void InternalDeserialize(BigEndianReader reader);

        public override void Deserialize(BigEndianReader reader)
        {
            RequestId = reader.ReadInt();
            InternalDeserialize(reader);
        }
    }
}
