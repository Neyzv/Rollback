using Rollback.Common.IO.Binary;

namespace Rollback.Common.Network.IPC.Messages.Connection
{
    public sealed record AccountConnectedMessage : IPCMessage
    {
        public const int Id = 9;
        public override uint MessageId =>
            Id;

        public int AccountId { get; set; }

        public AccountConnectedMessage() { }

        public AccountConnectedMessage(int accountId) =>
            AccountId = accountId;

        protected override void InternalSerialize(BigEndianWriter writer) =>
            writer.WriteInt(AccountId);

        protected override void InternalDeserialize(BigEndianReader reader) =>
            AccountId = reader.ReadInt();
    }
}
