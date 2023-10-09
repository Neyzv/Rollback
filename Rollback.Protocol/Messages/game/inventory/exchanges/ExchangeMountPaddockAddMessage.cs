using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeMountPaddockAddMessage : Message
	{
        public MountClientData mountDescription;

        public const int Id = 6049;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeMountPaddockAddMessage()
        {
        }
        public ExchangeMountPaddockAddMessage(MountClientData mountDescription)
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
