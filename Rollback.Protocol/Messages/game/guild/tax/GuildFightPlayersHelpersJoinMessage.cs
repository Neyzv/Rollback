using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;


namespace Rollback.Protocol.Messages
{
	public record GuildFightPlayersHelpersJoinMessage : Message
	{
        public double fightId;
        public CharacterMinimalPlusLookInformations playerInfo;

        public const int Id = 5720;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildFightPlayersHelpersJoinMessage()
        {
        }
        public GuildFightPlayersHelpersJoinMessage(double fightId, CharacterMinimalPlusLookInformations playerInfo)
        {
            this.fightId = fightId;
            this.playerInfo = playerInfo;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteDouble(fightId);
            playerInfo.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            fightId = reader.ReadDouble();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            playerInfo = new CharacterMinimalPlusLookInformations();
            playerInfo.Deserialize(reader);
		}
	}
}
