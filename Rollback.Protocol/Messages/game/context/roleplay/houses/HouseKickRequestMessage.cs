using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record HouseKickRequestMessage : Message
	{
        public int id;

        public const int Id = 5698;
        public override uint MessageId
        {
        	get { return 5698; }
        }
        public HouseKickRequestMessage()
        {
        }
        public HouseKickRequestMessage(int id)
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
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
		}
	}
}
