using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record TaxCollectorInformationsInWaitForHelpState : TaxCollectorInformations
    {
        public ProtectedEntityWaitingForHelpInfo waitingForHelpInfo;
        public new const short Id = 166;
        public override short TypeId
        {
            get { return Id; }
        }
        public TaxCollectorInformationsInWaitForHelpState()
        {
        }
        public TaxCollectorInformationsInWaitForHelpState(int uniqueId, short firtNameId, short lastNameId, AdditionalTaxCollectorInformations additonalInformation, short worldX, short worldY, short subAreaId, sbyte state, EntityLook look, ProtectedEntityWaitingForHelpInfo waitingForHelpInfo) : base(uniqueId, firtNameId, lastNameId, additonalInformation, worldX, worldY, subAreaId, state, look)
        {
            this.waitingForHelpInfo = waitingForHelpInfo;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            waitingForHelpInfo.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            waitingForHelpInfo = new ProtectedEntityWaitingForHelpInfo();
            waitingForHelpInfo.Deserialize(reader);
        }
    }
}
