using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GoldAddedMessage : Message
	{
        public GoldItem gold;

        public const int Id = 6030;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GoldAddedMessage()
        {
        }
        public GoldAddedMessage(GoldItem gold)
        {
            this.gold = gold;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            gold.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            gold = new GoldItem();
            gold.Deserialize(reader);
		}
	}
}
