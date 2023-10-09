using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record EmotePlayAbstractMessage : Message
	{
        public sbyte emoteId;
        public byte duration;

        public const int Id = 5690;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public EmotePlayAbstractMessage()
        {
        }
        public EmotePlayAbstractMessage(sbyte emoteId, byte duration)
        {
            this.emoteId = emoteId;
            this.duration = duration;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(emoteId);
            writer.WriteByte(duration);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            emoteId = reader.ReadSByte();
            if (emoteId < 0)
                throw new Exception("Forbidden value on emoteId = " + emoteId + ", it doesn't respect the following condition : emoteId < 0");
            duration = reader.ReadByte();
            if (duration < 0 || duration > 255)
                throw new Exception("Forbidden value on duration = " + duration + ", it doesn't respect the following condition : duration < 0 || duration > 255");
		}
	}
}
