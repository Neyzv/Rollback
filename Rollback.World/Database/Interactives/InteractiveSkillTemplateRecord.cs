using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Jobs;

namespace Rollback.World.Database.Interactives
{
    public static class InteractiveSkillTemplateRelator
    {
        public const string GetInteractiveSkillTemplates = "SELECT * FROM interactives_skills_templates";
    }

    [Table("interactives_skills_templates")]
    public sealed record InteractiveSkillTemplateRecord
    {
        [Key]
        public short Id { get; set; }

        public JobIds ParentJobId { get; set; }

        public bool IsForgemagus { get; set; }

        public bool IsRepair { get; set; }

        public ItemType ModifiableItemType { get; set; }

        public short GatheredRessourceItem { get; set; }

        private string _craftableItemIdsCSV = string.Empty;
        public string CraftableItemIdsCSV
        {
            get => _craftableItemIdsCSV;
            set
            {
                _craftableItemIdsCSV = value;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    foreach (var itemId in value.Split(','))
                        if (int.TryParse(itemId, out var id))
                            if (!CraftableItemIds.Contains(id))
                                CraftableItemIds.Add(id);
                            else
                                Logger.Instance.LogError(msg: $"Duplicate id {id} for {nameof(CraftableItemIds)} of {nameof(InteractiveSkillTemplateRecord)} {Id}...");
                        else
                            Logger.Instance.LogError(msg: $"Can not parse id {id} for {nameof(CraftableItemIds)} of {nameof(InteractiveSkillTemplateRecord)} {Id}...");
                }
            }
        }

        [Ignore]
        public HashSet<int> CraftableItemIds { get; private set; } = new();

        public int InteractiveTypeId { get; set; }

        public sbyte MinJobLevel { get; set; }

        public SkillActionDescription GetSkillActionDescription(Job job)
        {
            SkillActionDescription res;

            if (GatheredRessourceItem > 0)
            {
                var minMax = JobManager.GetHarvestItemMinMax(job.Level, this);
                res = new SkillActionDescriptionCollect(Id, (byte)Math.Ceiling(JobManager.HarvestTime / 100d),
                    minMax.Item1, minMax.Item2);
            }
            else if (CraftableItemIds.Count is not 0)
                res = new SkillActionDescriptionCraft(Id, JobManager.GetCraftMaxSlotsCount(job.Level),
                    JobManager.GetCraftSuccessPercentage(job.Level));
            else
                res = new SkillActionDescription(Id);

            return res;
        }
    }
}
