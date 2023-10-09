using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record IgnoredAddedMessage : Message
    {
        public IgnoredInformations ignoreAdded;

        public const int Id = 5678;
        public override uint MessageId
        {
            get { return Id; }
        }
        public IgnoredAddedMessage()
        {
        }
        public IgnoredAddedMessage(IgnoredInformations ignoreAdded)
        {
            this.ignoreAdded = ignoreAdded;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(ignoreAdded.TypeId);
            ignoreAdded.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            ignoreAdded = (IgnoredInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
            ignoreAdded.Deserialize(reader);
        }
    }
}
