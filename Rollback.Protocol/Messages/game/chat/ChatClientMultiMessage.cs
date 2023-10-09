using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ChatClientMultiMessage : ChatAbstractClientMessage
	{
        public sbyte channel;

        public new const int Id = 861;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChatClientMultiMessage()
        {
        }
        public ChatClientMultiMessage(string content, sbyte channel) : base(content)
        {
            this.channel = channel;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(channel);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            channel = reader.ReadSByte();
            if (channel < 0)
                throw new Exception("Forbidden value on channel = " + channel + ", it doesn't respect the following condition : channel < 0");
		}
	}
}
