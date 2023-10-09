using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Version = Rollback.Protocol.Types.Version;

namespace Rollback.Protocol.Messages
{
    public record IdentificationFailedForBadVersionMessage : IdentificationFailedMessage
    {
        public Version requiredVersion;

        public new const int Id = 21;
        public override uint MessageId
        {
            get { return Id; }
        }
        public IdentificationFailedForBadVersionMessage()
        {
        }
        public IdentificationFailedForBadVersionMessage(sbyte reason, Version requiredVersion) : base(reason)
        {
            this.requiredVersion = requiredVersion;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            requiredVersion.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            requiredVersion = new Version();
            requiredVersion.Deserialize(reader);
        }
    }
}
