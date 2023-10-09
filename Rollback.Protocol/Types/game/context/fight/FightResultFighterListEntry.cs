using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record FightResultFighterListEntry : FightResultListEntry
    {
        public int id;
        public bool alive;
        public new const short Id = 189;
        public override short TypeId
        {
            get { return Id; }
        }
        public FightResultFighterListEntry()
        {
        }
        public FightResultFighterListEntry(short outcome, Types.FightLoot rewards, int id, bool alive) : base(outcome, rewards)
        {
            this.id = id;
            this.alive = alive;
        }

        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(id);
            writer.WriteBoolean(alive);

        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            id = reader.ReadInt();
            alive = reader.ReadBoolean();
        }
    }
}