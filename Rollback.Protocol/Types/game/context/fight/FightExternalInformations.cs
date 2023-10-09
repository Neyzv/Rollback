using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record FightExternalInformations
    {
        public int fightId;
        public int fightStart;
        public bool fightSpectatorLocked;
        public FightTeamLightInformations[] fightTeams;
        public const short Id = 117;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public FightExternalInformations()
        {
        }
        public FightExternalInformations(int fightId, int fightStart, bool fightSpectatorLocked, FightTeamLightInformations[] fightTeams)
        {
            this.fightId = fightId;
            this.fightStart = fightStart;
            this.fightSpectatorLocked = fightSpectatorLocked;
            this.fightTeams = fightTeams;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteInt(fightStart);
            writer.WriteBoolean(fightSpectatorLocked);
            foreach (var entry in fightTeams)
            {
                entry.Serialize(writer);
            }
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            fightId = reader.ReadInt();
            fightStart = reader.ReadInt();
            if (fightStart < 0)
                throw new Exception("Forbidden value on fightStart = " + fightStart + ", it doesn't respect the following condition : fightStart < 0");
            fightSpectatorLocked = reader.ReadBoolean();
            fightTeams = new FightTeamLightInformations[2];
            for (int i = 0; i < 2; i++)
            {
                fightTeams[i] = new FightTeamLightInformations();
                fightTeams[i].Deserialize(reader);
            }
        }
    }
}
          