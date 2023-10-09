using Rollback.Common.IO.Binary;

namespace Rollback.Common.Network.IPC.Messages.Characters
{
    public sealed record CharacterDeletedMessage : IPCMessage
    {
        public const int Id = 7;
        public override uint MessageId =>
            Id;

        public int CharacterId { get; set; }

        public CharacterDeletedMessage() { }

        public CharacterDeletedMessage(int characterId) =>
            CharacterId = characterId;

        protected override void InternalSerialize(BigEndianWriter writer) =>
            writer.WriteInt(CharacterId);

        protected override void InternalDeserialize(BigEndianReader reader) =>
            CharacterId = reader.ReadInt();
    }
}
