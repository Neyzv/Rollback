using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameFightSpectateMessage : Message
    {
        public FightDispellableEffectExtendedInformations[] effects;

        public const int Id = 6069;
        public override uint MessageId
        {
            get { return Id; }
        }
        public GameFightSpectateMessage()
        {
        }
        public GameFightSpectateMessage(FightDispellableEffectExtendedInformations[] effects)
        {
            this.effects = effects;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)effects.Length);
            foreach (var entry in effects)
            {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            effects = new FightDispellableEffectExtendedInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                effects[i] = (FightDispellableEffectExtendedInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
                effects[i].Deserialize(reader);
            }
        }
    }
}
