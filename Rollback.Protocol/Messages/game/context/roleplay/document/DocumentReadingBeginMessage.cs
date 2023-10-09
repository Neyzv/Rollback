using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record DocumentReadingBeginMessage : Message
	{
        public short documentId;

        public const int Id = 5675;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public DocumentReadingBeginMessage()
        {
        }
        public DocumentReadingBeginMessage(short documentId)
        {
            this.documentId = documentId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(documentId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            documentId = reader.ReadShort();
            if (documentId < 0)
                throw new Exception("Forbidden value on documentId = " + documentId + ", it doesn't respect the following condition : documentId < 0");
		}
	}
}
