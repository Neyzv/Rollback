using Rollback.Common.ORM;
using Rollback.World.CustomEnums;

namespace Rollback.World.Database.Quests
{
    public static class QuestObjectiveRelator
    {
        public const string GetQuestObjectiveById = "SELECT * FROM quests_objectives WHERE Id = {0}";
    }

    [Table("quests_objectives")]
    public sealed record QuestObjectiveRecord
    {
        public QuestObjectiveRecord() =>
            (ParametersCSV) = (string.Empty);

        [Key]
        public short Id { get; set; }

        public QuestObjectiveType Type { get; set; }

        public string ParametersCSV { get; set; }

        public sbyte? X { get; set; }

        public sbyte? Y { get; set; }
    }
}
