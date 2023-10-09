using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record OnConnectionEventMessage : Message
	{
        public sbyte eventType;

        public const int Id = 5726;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public OnConnectionEventMessage()
        {
        }
        public OnConnectionEventMessage(sbyte eventType)
        {
            this.eventType = eventType;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(eventType);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            eventType = reader.ReadSByte();
            if (eventType < 0)
                throw new Exception("Forbidden value on eventType = " + eventType + ", it doesn't respect the following condition : eventType < 0");
		}
	}
}
