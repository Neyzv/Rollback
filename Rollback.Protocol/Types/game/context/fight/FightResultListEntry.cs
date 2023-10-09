
using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record FightResultListEntry
    {
        public short outcome;
        public FightLoot rewards;
        public const short Id = 16;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public FightResultListEntry()
        {
        }
        public FightResultListEntry(short outcome, FightLoot rewards)
        {
            this.outcome = outcome;
            this.rewards = rewards;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(outcome);
            rewards.Serialize(writer);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            outcome = reader.ReadShort();
            if (outcome < 0)
                throw new Exception("Forbidden value on outcome = " + outcome + ", it doesn't respect the following condition : outcome < 0");
            rewards = new Types.FightLoot();
            rewards.Deserialize(reader);
        }
    }
}
