using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record FightTeamMemberCharacterInformations : FightTeamMemberInformations
    {
        public string name;
        public short level;
        public new const short Id = 13;
        public override short TypeId
        {
            get { return Id; }
        }
        public FightTeamMemberCharacterInformations()
        {
        }
        public FightTeamMemberCharacterInformations(int id, string name, short level) : base(id)
        {
            this.name = name;
            this.level = level;
        }

        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteString(name);
            writer.WriteShort(level);

        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadString();
            level = reader.ReadShort();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
        }
    }
}
