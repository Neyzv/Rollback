using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameActionFightDispellableEffectMessage : AbstractGameActionMessage
    {
        public AbstractFightDispellableEffect effect;

        public new const int Id = 6070;
        public override uint MessageId
        {
            get { return Id; }
        }
        public GameActionFightDispellableEffectMessage()
        {
        }
        public GameActionFightDispellableEffectMessage(short actionId, int sourceId, AbstractFightDispellableEffect effect) : base(actionId, sourceId)
        {
            this.effect = effect;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(effect.TypeId);
            effect.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            effect = (AbstractFightDispellableEffect)ProtocolManager.Instance.GetType(reader.ReadUShort());
            effect.Deserialize(reader);
        }
    }
}
