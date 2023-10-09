using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record CharacterReplayWithRenameRequestMessage : CharacterReplayRequestMessage
	{
        public string name;

        public new const int Id = 6122;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterReplayWithRenameRequestMessage()
        {
        }
        public CharacterReplayWithRenameRequestMessage(int characterId, string name) : base(characterId)
        {
            this.name = name;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteString(name);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadString();
		}
	}
}
