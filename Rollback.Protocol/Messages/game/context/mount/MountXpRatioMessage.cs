using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record MountXpRatioMessage : Message
	{
        public sbyte ratio;

        public const int Id = 5970;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MountXpRatioMessage()
        {
        }
        public MountXpRatioMessage(sbyte ratio)
        {
            this.ratio = ratio;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(ratio);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            ratio = reader.ReadSByte();
            if (ratio < 0)
                throw new Exception("Forbidden value on ratio = " + ratio + ", it doesn't respect the following condition : ratio < 0");
		}
	}
}
