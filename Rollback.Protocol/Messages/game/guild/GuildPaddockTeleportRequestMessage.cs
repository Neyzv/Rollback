using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GuildPaddockTeleportRequestMessage : Message
	{
        public int paddockId;

        public const int Id = 5957;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildPaddockTeleportRequestMessage()
        {
        }
        public GuildPaddockTeleportRequestMessage(int paddockId)
        {
            this.paddockId = paddockId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(paddockId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            paddockId = reader.ReadInt();
            if (paddockId < 0)
                throw new Exception("Forbidden value on paddockId = " + paddockId + ", it doesn't respect the following condition : paddockId < 0");
		}
	}
}
