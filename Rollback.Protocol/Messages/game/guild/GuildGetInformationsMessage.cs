using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GuildGetInformationsMessage : Message
	{
        public sbyte infoType;

        public const int Id = 5550;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildGetInformationsMessage()
        {
        }
        public GuildGetInformationsMessage(sbyte infoType)
        {
            this.infoType = infoType;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(infoType);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            infoType = reader.ReadSByte();
            if (infoType < 0)
                throw new Exception("Forbidden value on infoType = " + infoType + ", it doesn't respect the following condition : infoType < 0");
		}
	}
}
