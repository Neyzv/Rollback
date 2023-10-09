using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ServersListMessage : Message
	{
        public GameServerInformations[] servers;

        public const int Id = 30;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ServersListMessage()
        {
        }
        public ServersListMessage(GameServerInformations[] servers)
        {
            this.servers = servers;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)servers.Length);
            foreach (var entry in servers)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            servers = new GameServerInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 servers[i] = new GameServerInformations();
                 servers[i].Deserialize(reader);
            }
		}
	}
}
