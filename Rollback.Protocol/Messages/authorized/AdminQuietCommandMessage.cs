using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record AdminQuietCommandMessage : AdminCommandMessage
    {
        public new const int Id = 5662;

        public override uint MessageId
        {
            get { return Id; }
        }
        public AdminQuietCommandMessage()
        {
        }
        public AdminQuietCommandMessage(string content) : base(content)
        {
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
