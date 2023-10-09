using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record FightTeamMemberInformations
    {
        public int id;
        public const short Id = 44;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public FightTeamMemberInformations()
        {
        }
        public FightTeamMemberInformations(int id)
        {
            this.id = id;
        }

        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(id);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadInt();
        }
    }
}

