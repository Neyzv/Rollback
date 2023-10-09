using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ExchangeObjectTransfertListToInvMessage : Message
	{
        public int[] ids;

        public const int Id = 6039;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeObjectTransfertListToInvMessage()
        {
        }
        public ExchangeObjectTransfertListToInvMessage(int[] ids)
        {
            this.ids = ids;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)ids.Length);
            foreach (var entry in ids)
            {
                 writer.WriteInt(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            ids = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 ids[i] = reader.ReadInt();
            }
		}
	}
}
