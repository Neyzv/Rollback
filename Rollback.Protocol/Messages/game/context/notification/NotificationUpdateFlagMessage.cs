using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record NotificationUpdateFlagMessage : Message
	{
        public short index;

        public const int Id = 6090;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public NotificationUpdateFlagMessage()
        {
        }
        public NotificationUpdateFlagMessage(short index)
        {
            this.index = index;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(index);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            index = reader.ReadShort();
            if (index < 0)
                throw new Exception("Forbidden value on index = " + index + ", it doesn't respect the following condition : index < 0");
		}
	}
}
