using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record SelectedServerDataMessage : Message
	{
        public short serverId;
        public string address;
        public ushort port;
        public bool canCreateNewCharacter;
        public string ticket;

        public const int Id = 42;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public SelectedServerDataMessage()
        {
        }
        public SelectedServerDataMessage(short serverId, string address, ushort port, bool canCreateNewCharacter, string ticket)
        {
            this.serverId = serverId;
            this.address = address;
            this.port = port;
            this.canCreateNewCharacter = canCreateNewCharacter;
            this.ticket = ticket;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(serverId);
            writer.WriteString(address);
            writer.WriteUShort(port);
            writer.WriteBoolean(canCreateNewCharacter);
            writer.WriteString(ticket);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            serverId = reader.ReadShort();
            address = reader.ReadString();
            port = reader.ReadUShort();
            if (port < 0 || port > 65535)
                throw new Exception("Forbidden value on port = " + port + ", it doesn't respect the following condition : port < 0 || port > 65535");
            canCreateNewCharacter = reader.ReadBoolean();
            ticket = reader.ReadString();
		}
	}
}
