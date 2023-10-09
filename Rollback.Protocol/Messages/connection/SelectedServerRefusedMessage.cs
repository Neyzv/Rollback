using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record SelectedServerRefusedMessage : Message
	{
        public short serverId;
        public sbyte error;
        public sbyte serverStatus;

        public const int Id = 41;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public SelectedServerRefusedMessage()
        {
        }
        public SelectedServerRefusedMessage(short serverId, sbyte error, sbyte serverStatus)
        {
            this.serverId = serverId;
            this.error = error;
            this.serverStatus = serverStatus;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(serverId);
            writer.WriteSByte(error);
            writer.WriteSByte(serverStatus);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            serverId = reader.ReadShort();
            error = reader.ReadSByte();
            if (error < 0)
                throw new Exception("Forbidden value on error = " + error + ", it doesn't respect the following condition : error < 0");
            serverStatus = reader.ReadSByte();
            if (serverStatus < 0)
                throw new Exception("Forbidden value on serverStatus = " + serverStatus + ", it doesn't respect the following condition : serverStatus < 0");
		}
	}
}
