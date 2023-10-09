using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record FightEntityDispositionInformations : EntityDispositionInformations
    {
        public int carryingCharacterId;
        public new const short Id = 217;
        public override short TypeId
        {
            get { return Id; }
        }
        public FightEntityDispositionInformations()
        {
        }
        public FightEntityDispositionInformations(short cellId, sbyte direction, int carryingCharacterId) : base(cellId, direction)
        {
            this.carryingCharacterId = carryingCharacterId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(carryingCharacterId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            carryingCharacterId = reader.ReadInt();
        }
    }
}
