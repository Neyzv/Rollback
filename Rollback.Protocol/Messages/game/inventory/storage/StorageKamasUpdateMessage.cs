using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record StorageKamasUpdateMessage : Message
	{
        public int kamasTotal;

        public const int Id = 5645;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public StorageKamasUpdateMessage()
        {
        }
        public StorageKamasUpdateMessage(int kamasTotal)
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
