using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record AbstractFightTeamInformations
    {
        public sbyte teamId;
        public int leaderId;
        public sbyte teamSide;
        public const short Id = 116;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public AbstractFightTeamInformations()
        {
        }
        public AbstractFightTeamInformations(sbyte teamId, int leaderId, sbyte teamSide)
        {
            this.teamId = teamId;
            this.leaderId = leaderId;
            this.teamSide = teamSide;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(teamId);
            writer.WriteInt(leaderId);
            writer.WriteSByte(teamSide);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            teamId = reader.ReadSByte();
            if (teamId < 0)
                throw new Exception("Forbidden value on teamId = " + teamId + ", it doesn't respect the following condition : teamId < 0");
            leaderId = reader.ReadInt();
            teamSide = reader.ReadSByte();
        }
    }
}

