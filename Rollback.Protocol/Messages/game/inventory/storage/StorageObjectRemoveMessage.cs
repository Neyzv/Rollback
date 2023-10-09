using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record StorageObjectRemoveMessage : Message
	{
        public int objectUID;

        public const int Id = 5648;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public StorageObjectRemoveMessage()
        {
        }
        public StorageObjectRemoveMessage(int objectUID)
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
