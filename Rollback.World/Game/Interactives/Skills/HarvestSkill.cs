using Rollback.Common.DesignPattern.Threading.Schedul;
using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Interactives;
using Rollback.World.Game.Jobs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Interactives.Skills
{
    public sealed class HarvestSkill : Skill
    {
        public override int Duration =>
            JobManager.HarvestTime;

        public HarvestSkill(InteractiveObject interactive, InteractiveSkillRecord record)
            : base(interactive, record) { }

        private void Regrow()
        {
            _interactive.ChangeState(InteractiveState.Normal);
            _interactive.Refresh();
        }

        public override bool CanBeUsed(Character character) =>
            _interactive.State is InteractiveState.Normal && ParentJobId.HasValue && character.HaveJob(ParentJobId.Value);

        public override bool BeforeExecute(Character character)
        {
            var result = true;

            if (character.HaveJob(ParentJobId!.Value))
            {
                if (ParentJobId.Value > JobManager.BaseJobId && character.Jobs[ParentJobId.Value].Level < MinJobLevel)
                {
                    //Tu n\'as pas le niveau nécessaire.
                    character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 3);
                    result = false;
                }
            }
            else
            {
                //Tu n\'exerces pas le métier nécessaire.
                character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 2);
                result = false;
            }

            return result;
        }

        public override void Execute(Character character)
        {
            _interactive.ChangeState(InteractiveState.Activated);
            _interactive.Refresh();

            if (character.Inventory.IsFull)
                // Votre inventaire est plein. Votre récolte est perdue...
                character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 144);
            else if ((character.Jobs.TryGetValue(ParentJobId!.Value, out var job) || ParentJobId <= JobManager.BaseJobId) &&
                    JobManager.GetHarvestItemMinMax(job?.Level ?? default, _record.Template!) is { Item2: > 0 } countInfos)
            {
                character.Inventory.AddItem(_record.Template!.GatheredRessourceItem,
                    Random.Shared.Next(countInfos.Item1, countInfos.Item2));

                job?.ChangeExperience(JobManager.GetHarvestJobXp(MinJobLevel!.Value));
            }

            Scheduler.Instance.ExecuteDelayed(Regrow)
                .WithTime(TimeSpan.FromMilliseconds(Random.Shared.Next(InteractiveConfig.Instance.RegrowMinTime,
                    InteractiveConfig.Instance.RegrowMaxTime + 1)));
        }
    }
}
