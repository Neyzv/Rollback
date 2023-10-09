using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Version = Rollback.Protocol.Types.Version;

namespace Rollback.Protocol.Messages
{
    public record IdentificationMessageWithServerIdMessage : IdentificationMessage
    {
        public short serverId;

        public new const int Id = 6104;
        public override uint MessageId
        {
            get { return Id; }
        }
        public IdentificationMessageWithServerIdMessage()
        {
        }
        public IdentificationMessageWithServerIdMessage(Version version, string login, string password, bool autoconnect, short serverId) : base(version, login, password, autoconnect)
        {
            this.serverId = serverId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(serverId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            serverId = reader.ReadShort();
        }
    }
}
