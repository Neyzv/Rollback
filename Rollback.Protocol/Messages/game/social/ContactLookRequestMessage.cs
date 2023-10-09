using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ContactLookRequestMessage : Message
	{
        public byte requestId;
        public sbyte contactType;

        public const int Id = 5932;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ContactLookRequestMessage()
        {
        }
        public ContactLookRequestMessage(byte requestId, sbyte contactType)
        {
            this.requestId = requestId;
            this.contactType = contactType;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteByte(requestId);
            writer.WriteSByte(contactType);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            requestId = reader.ReadByte();
            if (requestId < 0 || requestId > 255)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0 || requestId > 255");
            contactType = reader.ReadSByte();
            if (contactType < 0)
                throw new Exception("Forbidden value on contactType = " + contactType + ", it doesn't respect the following condition : contactType < 0");
		}
	}
}
