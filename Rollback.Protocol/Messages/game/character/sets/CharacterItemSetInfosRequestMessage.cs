using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record CharacterItemSetInfosRequestMessage : Message
    {
        public short setId;

        public const int Id = 1337;
        public override uint MessageId
        {
            get { return Id; }
        }

        public CharacterItemSetInfosRequestMessage()
        {
        }
        public CharacterItemSetInfosRequestMessage(short setId)
        {
            this.setId = setId;
        }

        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(setId);
        }

        public override void Deserialize(BigEndianReader reader)
        {
            setId = reader.ReadShort();
        }
    }
}
