
using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record FightDispellableEffectExtendedInformations
    {
        public short actionId;
        public int sourceId;
        public AbstractFightDispellableEffect effect;
        public const short Id = 208;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public FightDispellableEffectExtendedInformations()
        {
        }
        public FightDispellableEffectExtendedInformations(short actionId, int sourceId, AbstractFightDispellableEffect effect)
        {
            this.actionId = actionId;
            this.sourceId = sourceId;
            this.effect = effect;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(actionId);
            writer.WriteInt(sourceId);
            writer.WriteShort(effect.TypeId);
            effect.Serialize(writer);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            actionId = reader.ReadShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            sourceId = reader.ReadInt();
            effect = (AbstractFightDispellableEffect)ProtocolManager.Instance.GetType(reader.ReadUShort());
            effect.Deserialize(reader);
        }
    }
}