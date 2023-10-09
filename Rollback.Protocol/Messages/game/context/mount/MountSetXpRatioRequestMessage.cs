using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record MountSetXpRatioRequestMessage : Message
	{
        public sbyte xpRatio;

        public const int Id = 5989;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MountSetXpRatioRequestMessage()
        {
        }
        public MountSetXpRatioRequestMessage(sbyte xpRatio)
        {
            this.xpRatio = xpRatio;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(xpRatio);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            xpRatio = reader.ReadSByte();
            if (xpRatio < 0)
                throw new Exception("Forbidden value on xpRatio = " + xpRatio + ", it doesn't respect the following condition : xpRatio < 0");
		}
	}
}
