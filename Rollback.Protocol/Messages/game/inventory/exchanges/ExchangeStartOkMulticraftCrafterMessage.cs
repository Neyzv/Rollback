using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeStartOkMulticraftCrafterMessage : Message
	{
        public sbyte maxCase;
        public int skillId;

        public const int Id = 5818;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeStartOkMulticraftCrafterMessage()
        {
        }
        public ExchangeStartOkMulticraftCrafterMessage(sbyte maxCase, int skillId)
        {
            this.maxCase = maxCase;
            this.skillId = skillId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(maxCase);
            writer.WriteInt(skillId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            maxCase = reader.ReadSByte();
            if (maxCase < 0)
                throw new Exception("Forbidden value on maxCase = " + maxCase + ", it doesn't respect the following condition : maxCase < 0");
            skillId = reader.ReadInt();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
		}
	}
}
