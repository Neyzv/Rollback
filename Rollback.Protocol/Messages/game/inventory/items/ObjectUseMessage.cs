using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ObjectUseMessage : Message
	{
        public int objectUID;

        public const int Id = 3019;
        public override uint MessageId
        {
        	get { return 3019; }
        }
        public ObjectUseMessage()
        {
        }
        public ObjectUseMessage(int objectUID)
        {
            this.objectUID = objectUID;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(objectUID);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            objectUID = reader.ReadInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
		}
	}
}
