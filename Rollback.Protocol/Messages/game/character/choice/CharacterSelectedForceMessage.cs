using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record CharacterSelectedForceMessage : Message
	{
        public int id;

        public const int Id = 6068;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterSelectedForceMessage()
        {
        }
        public CharacterSelectedForceMessage(int id)
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
