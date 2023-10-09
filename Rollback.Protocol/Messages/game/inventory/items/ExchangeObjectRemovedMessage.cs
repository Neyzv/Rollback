using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeObjectRemovedMessage : ExchangeObjectMessage
	{
        public int objectUID;

        public new const int Id = 5517;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeObjectRemovedMessage()
        {
        }
        public ExchangeObjectRemovedMessage(bool remote, int objectUID) : base(remote)
        {
            this.objectUID = objectUID;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(objectUID);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            objectUID = reader.ReadInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
		}
	}
}
