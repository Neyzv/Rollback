using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ContactLookErrorMessage : Message
	{
        public int requestId;

        public const int Id = 6045;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ContactLookErrorMessage()
        {
        }
        public ContactLookErrorMessage(int requestId)
        {
            this.requestId = requestId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(requestId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            requestId = reader.ReadInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
		}
	}
}
