using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record CharacterItemSetInfosResponseMessage : Message
    {
        public ObjectEffect[] setEffects;

        public const int Id = 1338;
        public override uint MessageId
        {
            get { return Id; }
        }

        public CharacterItemSetInfosResponseMessage()
        {
        }
        public CharacterItemSetInfosResponseMessage(ObjectEffect[] setEffects)
        {
            this.setEffects = setEffects;
        }

        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)setEffects.Length);
            foreach (var effect in setEffects)
            {
                writer.WriteShort(effect.TypeId);
                effect.Serialize(writer);
            }
        }

        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            for (int i = 0; i < limit; i++)
            {
                setEffects[i] = (ObjectEffect)ProtocolManager.Instance.GetType(reader.ReadUShort());
                setEffects[i].Deserialize(reader);
            }
        }
    }
}
