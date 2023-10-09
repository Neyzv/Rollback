using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record MountDataMessage : Message
	{
        public MountClientData mountData;

        public const int Id = 5973;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MountDataMessage()
        {
        }
        public MountDataMessage(MountClientData mountData)
        {
            this.mountData = mountData;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            mountData.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            mountData = new MountClientData();
            mountData.Deserialize(reader);
		}
	}
}
