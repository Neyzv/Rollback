using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record MapFightCountMessage : Message
	{
        public short fightCount;

        public const int Id = 210;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MapFightCountMessage()
        {
        }
        public MapFightCountMessage(short fightCount)
        {
            this.fightCount = fightCount;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(fightCount);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            fightCount = reader.ReadShort();
            if (fightCount < 0)
                throw new Exception("Forbidden value on fightCount = " + fightCount + ", it doesn't respect the following condition : fightCount < 0");
		}
	}
}
