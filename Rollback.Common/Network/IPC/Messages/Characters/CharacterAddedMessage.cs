using Rollback.Common.IO.Binary;

namespace Rollback.Common.Network.IPC.Messages.Characters
{
    public sealed record CharacterAddedMessage : IPCMessage
    {
        public const int Id = 6;
        public override uint MessageId =>
            Id;

        public int AccountId { get; set; }

        public int CharacterId { get; set; }

        public CharacterAddedMessage() { }

        public CharacterAddedMessage(int accountId, int characterId)
        {
            AccountId = accountId;
            CharacterId = characterId;
        }

        protected override void InternalSerialize(BigEndianWriter writer)
        {
            writer.WriteInt(AccountId);
            writer.WriteInt(CharacterId);
        }

        protected override void InternalDeserialize(BigEndianReader reader)
        {
            AccountId = reader.ReadInt();
            CharacterId = reader.ReadInt();
        }
    }
}
