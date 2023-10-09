using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;


namespace Rollback.Protocol.Messages
{
	public record GameFightUpdateTeamMessage : Message
	{
        public short fightId;
        public FightTeamInformations team;

        public const int Id = 5572;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameFightUpdateTeamMessage()
        {
        }
        public GameFightUpdateTeamMessage(short fightId, FightTeamInformations team)
        {
            this.fightId = fightId;
            this.team = team;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(fightId);
            team.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            fightId = reader.ReadShort();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            team = new FightTeamInformations();
            team.Deserialize(reader);
		}
	}
}
