using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record EmoteAddMessage : Message
	{
        public sbyte emoteId;

        public const int Id = 5644;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public EmoteAddMessage()
        {
        }
        public EmoteAddMessage(sbyte emoteId)
        {
            this.emoteId = emoteId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(emoteId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            emoteId = reader.ReadSByte();
            if (emoteId < 0)
                throw new Exception("Forbidden value on emoteId = " + emoteId + ", it doesn't respect the following condition : emoteId < 0");
		}
	}
}
