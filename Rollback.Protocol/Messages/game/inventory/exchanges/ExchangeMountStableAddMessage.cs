using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeMountStableAddMessage : Message
	{
        public MountClientData mountDescription;

        public const int Id = 5971;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeMountStableAddMessage()
        {
        }
        public ExchangeMountStableAddMessage(MountClientData mountDescription)
        {
            this.mountDescription = mountDescription;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            mountDescription.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            mountDescription = new MountClientData();
            mountDescription.Deserialize(reader);
		}
	}
}
