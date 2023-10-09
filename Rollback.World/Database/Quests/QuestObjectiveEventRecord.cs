using Rollback.Common.ORM;
using Rollback.World.Game.Criterion;
using Rollback.World.Game.Quests.Objectives.Events;

namespace Rollback.World.Database.Quests
{
    public static class QuestObjectiveEventRelator
    {
        public const string GetObjectiveEvent = "SELECT * FROM quests_objectives_events";
    }

    [Table("quests_objectives_events")]
    public sealed record QuestObjectiveEventRecord
    {
        public QuestObjectiveEventRecord()
        {
            Action = string.Empty;
            Parameters = string.Empty;
        }

        [Key]
        public int Id { get; set; }

        public short QuestObjectiveId { get; set; }

        public QuestObjectiveEventTriggerType TriggerType { get; set; }

        public string Action { get; set; }

        public string Parameters { get; set; }

        private string _stringCriterion = string.Empty;
        public string StringCriterion
        {
            get => _stringCriterion;
            set
            {
                _stringCriterion = value;

                if (!string.IsNullOrWhiteSpace(value))
                    Criterion = CriterionManager.Instance.Parse(value);
            }
        }

        [Ignore]
        public CriterionExpression? Criterion { get; private set; }
    }
}
