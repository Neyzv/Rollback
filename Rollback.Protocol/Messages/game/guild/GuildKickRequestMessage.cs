using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GuildKickRequestMessage : Message
	{
        public int kickedId;

        public const int Id = 5887;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildKickRequestMessage()
        {
        }
        public GuildKickRequestMessage(int kickedId)
        {
            this.kickedId = kickedId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(kickedId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            kickedId = reader.ReadInt();
            if (kickedId < 0)
                throw new Exception("Forbidden value on kickedId = " + kickedId + ", it doesn't respect the following condition : kickedId < 0");
		}
	}
}
