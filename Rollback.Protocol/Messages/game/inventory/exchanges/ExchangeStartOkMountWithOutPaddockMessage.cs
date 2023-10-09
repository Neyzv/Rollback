using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeStartOkMountWithOutPaddockMessage : Message
	{
        public MountClientData[] stabledMountsDescription;

        public const int Id = 5991;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeStartOkMountWithOutPaddockMessage()
        {
        }
        public ExchangeStartOkMountWithOutPaddockMessage(MountClientData[] stabledMountsDescription)
        {
            this.stabledMountsDescription = stabledMountsDescription;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)stabledMountsDescription.Length);
            foreach (var entry in stabledMountsDescription)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            stabledMountsDescription = new MountClientData[limit];
            for (int i = 0; i < limit; i++)
            {
                 stabledMountsDescription[i] = new MountClientData();
                 stabledMountsDescription[i].Deserialize(reader);
            }
		}
	}
}
