using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ChannelEnablingChangeMessage : Message
	{
        public sbyte channel;
        public bool enable;

        public const int Id = 891;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChannelEnablingChangeMessage()
        {
        }
        public ChannelEnablingChangeMessage(sbyte channel, bool enable)
        {
            this.channel = channel;
            this.enable = enable;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(channel);
            writer.WriteBoolean(enable);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            channel = reader.ReadSByte();
            if (channel < 0)
                throw new Exception("Forbidden value on channel = " + channel + ", it doesn't respect the following condition : channel < 0");
            enable = reader.ReadBoolean();
		}
	}
}
