using Rollback.Common.IO.Binary;

namespace Rollback.Common.Network.IPC.Messages.Connection
{
    public sealed record ClientConnectionRefusedMessage : IPCMessage
    {
        public const int Id = 5;
        public override uint MessageId =>
            Id;

        public ClientConnectionRefusedMessage() { }

        protected override void InternalSerialize(BigEndianWriter writer) { }

        protected override void InternalDeserialize(BigEndianReader reader) { }
    }
}
