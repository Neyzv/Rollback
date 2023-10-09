using Rollback.Common.IO.Binary;

namespace Rollback.Common.Network.IPC.Messages.Accounts
{
    public sealed record AccountBannedMessage : IPCMessage
    {
        public const int Id = 10;
        public override uint MessageId =>
            Id;

        public int AccountId { get; set; }

        public int BanEndTime { get; set; }

        public AccountBannedMessage() { }

        public AccountBannedMessage(int accountId, int banEndTime)
        {
            AccountId = accountId;
            BanEndTime = banEndTime;
        }

        protected override void InternalSerialize(BigEndianWriter writer)
        {
            writer.WriteInt(AccountId);
            writer.WriteInt(BanEndTime);
        }

        protected override void InternalDeserialize(BigEndianReader reader)
        {
            AccountId = reader.ReadInt();
            BanEndTime = reader.ReadInt();
        }
    }
}
