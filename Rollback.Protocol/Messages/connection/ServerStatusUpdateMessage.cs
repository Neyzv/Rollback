using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ServerStatusUpdateMessage : Message
	{
        public GameServerInformations server;

        public const int Id = 50;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ServerStatusUpdateMessage()
        {
        }
        public ServerStatusUpdateMessage(GameServerInformations server)
        {
            this.server = server;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            server.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            server = new GameServerInformations();
            server.Deserialize(reader);
		}
	}
}
