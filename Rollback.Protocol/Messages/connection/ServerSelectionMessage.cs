using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record ServerSelectionMessage : Message
	{
        public short serverId;

        public const int Id = 40;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ServerSelectionMessage()
        {
        }
        public ServerSelectionMessage(short serverId)
        {
            this.serverId = serverId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(serverId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            serverId = reader.ReadShort();
		}
	}
}
