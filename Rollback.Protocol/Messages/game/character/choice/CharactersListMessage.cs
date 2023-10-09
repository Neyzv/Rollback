using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record CharactersListMessage : Message
    {
        public bool hasStartupActions;
        public bool tutorielAvailable;
        public CharacterBaseInformations[] characters;

        public const int Id = 151;
        public override uint MessageId
        {
            get { return Id; }
        }
        public CharactersListMessage()
        {
        }
        public CharactersListMessage(bool hasStartupActions, bool tutorielAvailable, CharacterBaseInformations[] characters)
        {
            this.hasStartupActions = hasStartupActions;
            this.tutorielAvailable = tutorielAvailable;
            this.characters = characters;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            byte flag1 = 0;
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 0, hasStartupActions);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 1, tutorielAvailable);
            writer.WriteByte(flag1);
            writer.WriteUShort((ushort)characters.Length);
            foreach (var entry in characters)
            {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            byte flag1 = reader.ReadByte();
            hasStartupActions = BigEndianBooleanByteWrapper.GetFlag(flag1, 0);
            tutorielAvailable = BigEndianBooleanByteWrapper.GetFlag(flag1, 1);
            var limit = reader.ReadUShort();
            characters = new CharacterBaseInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                characters[i] = (CharacterBaseInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
                characters[i].Deserialize(reader);
            }
        }
    }
}
