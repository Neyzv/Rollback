#nullable disable

using Rollback.Common.IO.Binary;
using Rollback.Common.Network.IPC.Types.Gifts;

namespace Rollback.Common.Network.IPC.Messages.Connection
{
    public sealed record ClientConnectionAcceptedMessage : IPCMessage
    {
        public const int Id = 4;
        public override uint MessageId =>
            Id;

        public int AccountId { get; set; }

        public string Nickname { get; set; }

        public sbyte Role { get; set; }

        public int[] CharactersIds { get; set; }

        public string SecretAnswer { get; set; }

        public sbyte FreeCharacterSlots { get; set; }

        public int LastConnection { get; set; }

        public string LastIP { get; set; }

        public GiftInformations[] Gifts { get; set; }

        public ClientConnectionAcceptedMessage() { }

        public ClientConnectionAcceptedMessage(int accountId, string nickname, sbyte role, int[] charactersIds, string secretAnswer,
            sbyte freeCharacterSlots, int lastConnection, string lastIP, GiftInformations[] gifts)
        {
            AccountId = accountId;
            Nickname = nickname;
            Role = role;
            CharactersIds = charactersIds;
            SecretAnswer = secretAnswer;
            FreeCharacterSlots = freeCharacterSlots;
            LastConnection = lastConnection;
            LastIP = lastIP;
            Gifts = gifts;
        }

        protected override void InternalSerialize(BigEndianWriter writer)
        {
            writer.WriteInt(AccountId);
            writer.WriteString(Nickname);
            writer.WriteSByte(Role);

            writer.WriteByte((byte)CharactersIds.Length);
            foreach (var id in CharactersIds)
                writer.WriteInt(id);

            writer.WriteString(SecretAnswer);
            writer.WriteSByte(FreeCharacterSlots);
            writer.WriteInt(LastConnection);
            writer.WriteString(LastIP);

            writer.WriteUShort((ushort)Gifts.Length);
            for (int i = 0; i < Gifts.Length; i++)
                Gifts[i].Serialize(writer);
        }

        protected override void InternalDeserialize(BigEndianReader reader)
        {
            AccountId = reader.ReadInt();
            Nickname = reader.ReadString();
            Role = reader.ReadSByte();

            var length = reader.ReadByte();
            CharactersIds = new int[length];
            for (var i = 0; i < length; i++)
                CharactersIds[i] = reader.ReadInt();

            SecretAnswer = reader.ReadString();
            FreeCharacterSlots = reader.ReadSByte();
            LastConnection = reader.ReadInt();
            LastIP = reader.ReadString();

            var giftsCount = reader.ReadUShort();
            Gifts = new GiftInformations[giftsCount];
            for (var i = 0; i < giftsCount; i++)
            {
                Gifts[i] = new GiftInformations();
                Gifts[i].Deserialize(reader);
            }
        }
    }
}
