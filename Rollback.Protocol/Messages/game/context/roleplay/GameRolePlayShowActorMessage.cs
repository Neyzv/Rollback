using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameRolePlayShowActorMessage : Message
    {
        public GameRolePlayActorInformations informations;

        public const int Id = 5632;
        public override uint MessageId
        {
            get { return Id; }
        }
        public GameRolePlayShowActorMessage()
        {
        }
        public GameRolePlayShowActorMessage(GameRolePlayActorInformations informations)
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
            informations = (GameRolePlayActorInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
            informations.Deserialize(reader);
        }
    }
}
