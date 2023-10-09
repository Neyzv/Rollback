using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record TaxCollectorMovementAddMessage : Message
    {
        public TaxCollectorInformations informations;

        public const int Id = 5917;
        public override uint MessageId
        {
            get { return Id; }
        }
        public TaxCollectorMovementAddMessage()
        {
        }
        public TaxCollectorMovementAddMessage(TaxCollectorInformations informations)
        {
            this.informations = informations;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(informations.TypeId);
            informations.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            informations = (TaxCollectorInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
            informations.Deserialize(reader);
        }
    }
}
