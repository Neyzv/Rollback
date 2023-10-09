using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record KamasUpdateMessage : Message
	{
        public int kamasTotal;

        public const int Id = 5537;
        public override uint MessageId
        {
        	get { return 5537; }
        }
        public KamasUpdateMessage()
        {
        }
        public KamasUpdateMessage(int kamasTotal)
        {
            this.kamasTotal = kamasTotal;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(kamasTotal);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            kamasTotal = reader.ReadInt();
		}
	}
}
