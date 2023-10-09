using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record BasicPingMessage : Message
    {
        public bool quiet;

        public const int Id = 182;
        public override uint MessageId
        {
            get { return Id; }
        }
        public BasicPingMessage()
        {
        }
        public BasicPingMessage(bool quiet)
        {
            this.quiet = quiet;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(quiet);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            quiet = reader.ReadBoolean();
        }
    }
}
