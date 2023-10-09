using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangePlayerMultiCraftRequestMessage : ExchangeRequestMessage
	{
        public int target;
        public int skillId;

        public new const int Id = 5784;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangePlayerMultiCraftRequestMessage()
        {
        }
        public ExchangePlayerMultiCraftRequestMessage(sbyte exchangeType, int target, int skillId) : base(exchangeType)
        {
            this.target = target;
            this.skillId = skillId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(target);
            writer.WriteInt(skillId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            target = reader.ReadInt();
            if (target < 0)
                throw new Exception("Forbidden value on target = " + target + ", it doesn't respect the following condition : target < 0");
            skillId = reader.ReadInt();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
		}
	}
}
