using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GuildUIOpenedMessage : Message
	{
        public sbyte type;

        public const int Id = 5561;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildUIOpenedMessage()
        {
        }
        public GuildUIOpenedMessage(sbyte type)
        {
            this.type = type;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(type);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
		}
	}
}
