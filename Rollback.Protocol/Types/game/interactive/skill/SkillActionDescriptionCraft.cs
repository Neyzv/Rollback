using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record SkillActionDescriptionCraft : SkillActionDescription
    {
        public sbyte maxSlots;
        public sbyte probability;
        public new const short Id = 100;
        public override short TypeId
        {
            get { return Id; }
        }
        public SkillActionDescriptionCraft()
        {
        }
        public SkillActionDescriptionCraft(short skillId, sbyte maxSlots, sbyte probability) : base(skillId)
        {
            this.maxSlots = maxSlots;
            this.probability = probability;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(maxSlots);
            writer.WriteSByte(probability);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            maxSlots = reader.ReadSByte();
            if (maxSlots < 0)
                throw new Exception("Forbidden value on maxSlots = " + maxSlots + ", it doesn't respect the following condition : maxSlots < 0");
            probability = reader.ReadSByte();
            if (probability < 0)
                throw new Exception("Forbidden value on probability = " + probability + ", it doesn't respect the following condition : probability < 0");
        }
    }
}

