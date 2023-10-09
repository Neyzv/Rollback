using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Interactives;
using Rollback.World.Game.Interactives;

namespace Rollback.World.Database.Jobs
{
    public static class JobTemplateRelator
    {
        public const string QueryAll = "SELECT * FROM jobs_templates";
    }

    [Table("jobs_templates")]
    public sealed record JobTemplateRecord
    {
        [Key]
        public JobIds Id { get; set; }

        public sbyte SpecializationOfId { get; set; }

        private string _toolIdsCSV = string.Empty;
        public string ToolIdsCSV
        {
            get => _toolIdsCSV;
            set
            {
                _toolIdsCSV = value;

                if (!string.IsNullOrWhiteSpace(_toolIdsCSV))
                {
                    ToolIds.Clear();

                    foreach (var toolId in value.Split(';'))
                    {
                        if (short.TryParse(toolId, out var id))
                            if (!ToolIds.Contains(id))
                                ToolIds.Add(id);
                            else
                                Logger.Instance.LogError(msg: $"Error while trying to add tool {id} for {nameof(JobTemplateRecord)} {Id}, tool id already in...");
                        else
                            Logger.Instance.LogError(msg: $"Error while parsing {nameof(ToolIdsCSV)} of {nameof(JobTemplateRecord)} {Id}...");
                    }
                }
            }
        }

        [Ignore]
        public HashSet<short> ToolIds { get; private set; } = new();

        private InteractiveSkillTemplateRecord[]? _skills;
        [Ignore]
        public InteractiveSkillTemplateRecord[] Skills =>
            _skills ??= InteractiveManager.Instance.GetInteractiveSkillTemplates(x => x.ParentJobId == Id);
    }
}
