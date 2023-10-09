using Rollback.Common.ORM;

namespace Rollback.World.Database.Quests
{
    public static class QuestRelator
    {
        public const string GetQuests = "SELECT * FROM quests";
    }

    [Table("quests")]
    public sealed record QuestRecord
    {
        public QuestRecord() =>
            (StepsCSV, Steps) = (string.Empty, Array.Empty<QuestStepRecord>());

        [Key]
        public short Id { get; set; }

        public string StepsCSV { get; set; }

        [Ignore]
        public QuestStepRecord[] Steps { get; set; }

        public bool IsRepeatable { get; set; }
    }
}
