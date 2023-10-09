using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record CharacterSelectionWithRenameMessage : CharacterSelectionMessage
	{
        public string name;

        public new const int Id = 6121;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterSelectionWithRenameMessage()
        {
        }
        public CharacterSelectionWithRenameMessage(int id, string name) : base(id)
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
