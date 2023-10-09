using Rollback.Common.IO.Binary;

namespace Rollback.Common.Network.IPC.Messages.Gifts
{
    public sealed record GiftReceivedMessage : IPCMessage
    {
        public const int Id = 8;
        public override uint MessageId =>
            Id;

        public int GiftId { get; set; }

        public GiftReceivedMessage() { }

        public GiftReceivedMessage(int giftId) =>
            GiftId = giftId;

        protected override void InternalSerialize(BigEndianWriter writer) =>
            writer.WriteInt(GiftId);

        protected override void InternalDeserialize(BigEndianReader reader) =>
            GiftId = reader.ReadInt();
    }
}
