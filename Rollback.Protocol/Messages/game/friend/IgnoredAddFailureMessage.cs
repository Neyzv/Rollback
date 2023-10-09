using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record IgnoredAddFailureMessage : Message
	{
        public sbyte reason;

        public const int Id = 5679;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public IgnoredAddFailureMessage()
        {
        }
        public IgnoredAddFailureMessage(sbyte reason)
        {
            this.reason = reason;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(reason);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            reason = reader.ReadSByte();
            if (reason < 0)
                throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
		}
	}
}
