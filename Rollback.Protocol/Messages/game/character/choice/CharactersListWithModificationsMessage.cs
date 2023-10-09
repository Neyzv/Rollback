using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record CharactersListWithModificationsMessage : CharactersListMessage
    {
        public CharacterToRecolorInformation[] charactersToRecolor;
        public int[] charactersToRename;

        public new const int Id = 6120;
        public override uint MessageId
        {
            get { return Id; }
        }
        public CharactersListWithModificationsMessage()
        {
        }
        public CharactersListWithModificationsMessage(bool hasStartupActions, bool tutorielAvailable, CharacterBaseInformations[] characters, CharacterToRecolorInformation[] charactersToRecolor, int[] charactersToRename) : base(hasStartupActions, tutorielAvailable, characters)
        {
            this.charactersToRecolor = charactersToRecolor;
            this.charactersToRename = charactersToRename;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)charactersToRecolor.Length);
            foreach (var entry in charactersToRecolor)
            {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)charactersToRename.Length);
            foreach (var entry in charactersToRename)
            {
                writer.WriteInt(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            charactersToRecolor = new CharacterToRecolorInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                charactersToRecolor[i] = (CharacterToRecolorInformation)ProtocolManager.Instance.GetType(reader.ReadUShort());
                charactersToRecolor[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            charactersToRename = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                charactersToRename[i] = reader.ReadInt();
            }
        }
    }
}
