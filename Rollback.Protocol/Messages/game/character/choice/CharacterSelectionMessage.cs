using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record CharacterSelectionMessage : Message
	{
        public int id;

        public const int Id = 152;
        public override uint MessageId
        {
        	get { return 152; }
        }
        public CharacterSelectionMessage()
        {
        }
        public CharacterSelectionMessage(int id)
        {
            this.id = id;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(id);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadInt();
            if (id < 1 || id > 2147483647)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 1 || id > 2147483647");
		}
	}
}
