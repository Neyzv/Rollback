
using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record FightResultExperienceData : FightResultAdditionalData
    {
        public bool showExperience;
        public bool showExperienceLevelFloor;
        public bool showExperienceNextLevelFloor;
        public bool showExperienceFightDelta;
        public bool showExperienceForGuild;
        public bool showExperienceForMount;
        public double experience;
        public double experienceLevelFloor;
        public double experienceNextLevelFloor;
        public int experienceFightDelta;
        public int experienceForGuild;
        public int experienceForMount;
        public new const short Id = 192;
        public override short TypeId
        {
            get { return Id; }
        }
        public FightResultExperienceData()
        {
        }
        public FightResultExperienceData(bool showExperience, bool showExperienceLevelFloor, bool showExperienceNextLevelFloor, bool showExperienceFightDelta, bool showExperienceForGuild, bool showExperienceForMount, double experience, double experienceLevelFloor, double experienceNextLevelFloor, int experienceFightDelta, int experienceForGuild, int experienceForMount)
        {
            this.showExperience = showExperience;
            this.showExperienceLevelFloor = showExperienceLevelFloor;
            this.showExperienceNextLevelFloor = showExperienceNextLevelFloor;
            this.showExperienceFightDelta = showExperienceFightDelta;
            this.showExperienceForGuild = showExperienceForGuild;
            this.showExperienceForMount = showExperienceForMount;
            this.experience = experience;
            this.experienceLevelFloor = experienceLevelFloor;
            this.experienceNextLevelFloor = experienceNextLevelFloor;
            this.experienceFightDelta = experienceFightDelta;
            this.experienceForGuild = experienceForGuild;
            this.experienceForMount = experienceForMount;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            byte flag1 = 0;
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 0, showExperience);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 1, showExperienceLevelFloor);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 2, showExperienceNextLevelFloor);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 3, showExperienceFightDelta);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 4, showExperienceForGuild);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 5, showExperienceForMount);
            writer.WriteByte(flag1);
            writer.WriteDouble(experience);
            writer.WriteDouble(experienceLevelFloor);
            writer.WriteDouble(experienceNextLevelFloor);
            writer.WriteInt(experienceFightDelta);
            writer.WriteInt(experienceForGuild);
            writer.WriteInt(experienceForMount);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            byte flag1 = reader.ReadByte();
            showExperience = BigEndianBooleanByteWrapper.GetFlag(flag1, 0);
            showExperienceLevelFloor = BigEndianBooleanByteWrapper.GetFlag(flag1, 1);
            showExperienceNextLevelFloor = BigEndianBooleanByteWrapper.GetFlag(flag1, 2);
            showExperienceFightDelta = BigEndianBooleanByteWrapper.GetFlag(flag1, 3);
            showExperienceForGuild = BigEndianBooleanByteWrapper.GetFlag(flag1, 4);
            showExperienceForMount = BigEndianBooleanByteWrapper.GetFlag(flag1, 5);
            experience = reader.ReadDouble();
            if (experience < 0)
                throw new Exception("Forbidden value on experience = " + experience + ", it doesn't respect the following condition : experience < 0");
            experienceLevelFloor = reader.ReadDouble();
            if (experienceLevelFloor < 0)
                throw new Exception("Forbidden value on experienceLevelFloor = " + experienceLevelFloor + ", it doesn't respect the following condition : experienceLevelFloor < 0");
            experienceNextLevelFloor = reader.ReadDouble();
            if (experienceNextLevelFloor < 0)
                throw new Exception("Forbidden value on experienceNextLevelFloor = " + experienceNextLevelFloor + ", it doesn't respect the following condition : experienceNextLevelFloor < 0");
            experienceFightDelta = reader.ReadInt();
            experienceForGuild = reader.ReadInt();
            if (experienceForGuild < 0)
                throw new Exception("Forbidden value on experienceForGuild = " + experienceForGuild + ", it doesn't respect the following condition : experienceForGuild < 0");
            experienceForMount = reader.ReadInt();
            if (experienceForMount < 0)
                throw new Exception("Forbidden value on experienceForMount = " + experienceForMount + ", it doesn't respect the following condition : experienceForMount < 0");
        }
    }
}