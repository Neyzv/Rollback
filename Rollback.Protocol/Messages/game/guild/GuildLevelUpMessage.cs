using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GuildLevelUpMessage : Message
	{
        public byte newLevel;

        public const int Id = 6062;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildLevelUpMessage()
        {
        }
        public GuildLevelUpMessage(byte newLevel)
        {
            this.newLevel = newLevel;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteByte(newLevel);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            newLevel = reader.ReadByte();
            if (newLevel < 2 || newLevel > 200)
                throw new Exception("Forbidden value on newLevel = " + newLevel + ", it doesn't respect the following condition : newLevel < 2 || newLevel > 200");
		}
	}
}
