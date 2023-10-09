using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeStartOkMountMessage : ExchangeStartOkMountWithOutPaddockMessage
	{
        public MountClientData[] paddockedMountsDescription;

        public new const int Id = 5979;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeStartOkMountMessage()
        {
        }
        public ExchangeStartOkMountMessage(MountClientData[] stabledMountsDescription, MountClientData[] paddockedMountsDescription) : base(stabledMountsDescription)
        {
            this.paddockedMountsDescription = paddockedMountsDescription;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)paddockedMountsDescription.Length);
            foreach (var entry in paddockedMountsDescription)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            paddockedMountsDescription = new MountClientData[limit];
            for (int i = 0; i < limit; i++)
            {
                 paddockedMountsDescription[i] = new MountClientData();
                 paddockedMountsDescription[i].Deserialize(reader);
            }
		}
	}
}
