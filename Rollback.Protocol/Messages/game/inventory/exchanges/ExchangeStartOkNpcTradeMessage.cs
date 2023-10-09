using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeStartOkNpcTradeMessage : Message
	{
        public int npcId;

        public const int Id = 5785;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeStartOkNpcTradeMessage()
        {
        }
        public ExchangeStartOkNpcTradeMessage(int npcId)
        {
            this.npcId = npcId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(npcId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            npcId = reader.ReadInt();
		}
	}
}
