using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;


namespace Rollback.Protocol.Messages
{
	public record GuildFightPlayersEnemiesListMessage : Message
	{
        public double fightId;
        public CharacterMinimalPlusLookInformations[] playerInfo;

        public const int Id = 5928;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildFightPlayersEnemiesListMessage()
        {
        }
        public GuildFightPlayersEnemiesListMessage(double fightId, CharacterMinimalPlusLookInformations[] playerInfo)
        {
            this.fightId = fightId;
            this.playerInfo = playerInfo;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteDouble(fightId);
            writer.WriteUShort((ushort)playerInfo.Length);
            foreach (var entry in playerInfo)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            fightId = reader.ReadDouble();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            var limit = reader.ReadUShort();
            playerInfo = new CharacterMinimalPlusLookInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 playerInfo[i] = new CharacterMinimalPlusLookInformations();
                 playerInfo[i].Deserialize(reader);
            }
		}
	}
}
