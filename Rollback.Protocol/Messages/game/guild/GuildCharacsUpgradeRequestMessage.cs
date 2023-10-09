using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GuildCharacsUpgradeRequestMessage : Message
	{
        public sbyte charaTypeTarget;

        public const int Id = 5706;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildCharacsUpgradeRequestMessage()
        {
        }
        public GuildCharacsUpgradeRequestMessage(sbyte charaTypeTarget)
        {
            this.charaTypeTarget = charaTypeTarget;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(charaTypeTarget);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            charaTypeTarget = reader.ReadSByte();
            if (charaTypeTarget < 0)
                throw new Exception("Forbidden value on charaTypeTarget = " + charaTypeTarget + ", it doesn't respect the following condition : charaTypeTarget < 0");
		}
	}
}
