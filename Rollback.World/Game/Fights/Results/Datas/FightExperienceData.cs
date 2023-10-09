using Rollback.Protocol.Types;
using Rollback.World.Game.Experiences;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Fights.Results.Datas
{
    public sealed class FightExperienceData : FightAdditionalDatas
    {
        public bool ShowExperience { get; set; }

        public bool ShowExperienceLevelFloor =>
            Character.Level < ExperienceManager.Instance.MaxCharacterLevel;

        public bool ShowExperienceNextLevelFloor =>
            Character.Level < ExperienceManager.Instance.MaxCharacterLevel;

        public bool ShowExperienceFightDelta { get; set; }

        public bool ShowExperienceForGuild { get; set; }

        public bool ShowExperienceForMount { get; set; }

        public bool IsIncarnationExperience { get; set; }

        public int ExperienceFightDelta { get; set; }

        public int ExperienceForGuild { get; set; }

        public int ExperienceForMount { get; set; }

        public override FightResultAdditionalData FightResultAdditionalData =>
            new FightResultExperienceData(ShowExperience, ShowExperienceLevelFloor, ShowExperienceNextLevelFloor,
                ShowExperienceFightDelta, ShowExperienceForGuild, ShowExperienceForMount, Character.Experience,
                Character.LowerExperienceLevelFloor, Character.UpperExperienceLevelFloor, ExperienceFightDelta,
                ExperienceForGuild, ExperienceForMount);

        public FightExperienceData(Character character) : base(character) { }

        public override void Apply()
        {
            if (ExperienceFightDelta > 0)
                Character.ChangeExperience(ExperienceFightDelta, false);

            if (ExperienceForGuild > 0)
                Character.GuildMember?.Guild.ChangeExperience(ExperienceForGuild);
            
            if(ExperienceForMount > 0)
                Character.EquipedMount?.ChangeExperience(ExperienceForMount);
        }
    }
}
