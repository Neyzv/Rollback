using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeMountStableBornAddMessage : ExchangeMountStableAddMessage
	{
        public new const int Id = 5966;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeMountStableBornAddMessage()
        {
        }
        public ExchangeMountStableBornAddMessage(MountClientData mountDescription) : base(mountDescription)
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
