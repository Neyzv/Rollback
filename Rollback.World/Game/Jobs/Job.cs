using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Database.Jobs;
using Rollback.World.Game.Experiences;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Jobs;

namespace Rollback.World.Game.Jobs
{
    public sealed class Job
    {
        private readonly Character _owner;
        private readonly CharacterJobRecord _record;
        private readonly JobTemplateRecord _template;

        public JobIds Id =>
            _template.Id;

        public long Experience
        {
            get => _record.Experience;
            private set => _record.Experience = value;
        }

        public sbyte Level { get; private set; }

        public long UpperExperienceLevelFloor { get; private set; }

        public long LowerExperienceLevelFloor { get; private set; }

        public JobDescription JobDescription =>
            new((sbyte)Id, _template.Skills.Where(x => x.MinJobLevel <= Level).Select(x => x.GetSkillActionDescription(this)).ToArray());

        public JobExperience JobExperience =>
            new((sbyte)Id, Level, Experience, LowerExperienceLevelFloor, UpperExperienceLevelFloor);

        public JobCrafterDirectorySettings JobCrafterDirectorySettings =>
            new((sbyte)Id, 2, 1); // TO DO

        public Job(Character owner, CharacterJobRecord record)
        {
            _owner = owner;
            _record = record;

            var template = JobManager.Instance.GetRecordById(record.JobId);
            if (template is null)
            {
                Logger.Instance.LogWarn($"Force disconnection of client: Can not find job {record.JobId} for character {owner.Id}...");
                owner.Client.Dispose();
            }
            _template = template!;

            RefreshLevelValues();
        }

        private void RefreshLevelValues()
        {
            Level = ExperienceManager.Instance.GetJobLevel(Experience);
            UpperExperienceLevelFloor = ExperienceManager.Instance.GetJobUpperExperienceLevelFloor(Experience);
            LowerExperienceLevelFloor = ExperienceManager.Instance.GetJobLowerExperienceLevelFloor(Experience);
        }

        private void AdjustLevel()
        {
            if (Level < ExperienceManager.Instance.MaxJobLevel)
            {
                var lastLevel = Level;

                RefreshLevelValues();

                var levelDifference = (short)(Level - lastLevel);

                if (levelDifference is not 0)
                {
                    _owner.Stats[Stat.Weight].Base += (short)(levelDifference * JobManager.PodsPerJobLevel);
                    _owner.RefreshStats();

                    JobHandler.SendObjectJobAddedMessage(_owner.Client, this);
                    JobHandler.SendJobLevelUpMessage(_owner.Client, this);
                }
            }
        }

        public void ChangeExperience(long experience)
        {
            Experience += experience;

            if (Experience < 0)
                Experience = 0;

            if (Experience < LowerExperienceLevelFloor || (UpperExperienceLevelFloor > 0 && Experience >= UpperExperienceLevelFloor))
                AdjustLevel();

            JobHandler.SendJobExperienceUpdateMessage(_owner.Client, this);
        }

        public void Save() =>
            DatabaseAccessor.Instance.InsertOrUpdate(_record);
    }
}
