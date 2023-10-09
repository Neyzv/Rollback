using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record FightResultMutantListEntry : FightResultFighterListEntry
    {
        public ushort level;

        public new const short Id = 216;
        public override short TypeId
        {
            get { return Id; }
        }
        public FightResultMutantListEntry()
        {
        }
        public FightResultMutantListEntry(short outcome, Types.FightLoot rewards, int id, bool alive, ushort level) : base(outcome, rewards, id, alive)
        {
            this.level = level;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort(level);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            level = reader.ReadUShort();
            if (level < 0 || level > 255)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0 || level > 255");
        }
    }
}
