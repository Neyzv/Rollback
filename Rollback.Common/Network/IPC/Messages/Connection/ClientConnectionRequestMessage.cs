using Rollback.Common.IO.Binary;

namespace Rollback.Common.Network.IPC.Messages.Connection
{
    public sealed record ClientConnectionRequestMessage : IPCMessage
    {
        public const int Id = 3;
        public override uint MessageId =>
            Id;

        public string? Ticket { get; set; }

        public ClientConnectionRequestMessage() { }

        public ClientConnectionRequestMessage(string ticket) =>
            Ticket = ticket;

        protected override void InternalSerialize(BigEndianWriter writer) =>
            writer.WriteString(Ticket!);

        protected override void InternalDeserialize(BigEndianReader reader) =>
            Ticket = reader.ReadString();
    }
}
