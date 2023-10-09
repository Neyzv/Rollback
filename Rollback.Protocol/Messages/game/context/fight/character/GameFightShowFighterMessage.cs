using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameFightShowFighterMessage : Message
    {
        public GameFightFighterInformations informations;

        public const int Id = 5864;
        public override uint MessageId
        {
            get { return Id; }
        }
        public GameFightShowFighterMessage()
        {
        }
        public GameFightShowFighterMessage(GameFightFighterInformations informations)
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
            informations = (GameFightFighterInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
            informations.Deserialize(reader);
        }
    }
}
