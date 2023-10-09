using Rollback.Common.ORM;
using Rollback.World.Game.Criterion;

namespace Rollback.World.Database.Npcs
{
    public static class NpcActionRelator
    {
        public const string GetNpcActions = "SELECT * FROM npcs_actions";
    }

    [Table("npcs_actions")]
    public sealed record NpcActionRecord
    {
        public NpcActionRecord()
        {
            Action = string.Empty;
            _stringCriterion = string.Empty;
            ParametersCSV = string.Empty;
            Items = new();
        }

        [Key]
        public int Id { get; set; }

        public short NpcId { get; set; }

        public string Action { get; set; }

        private string _stringCriterion;
        public string StringCriterion
        {
            get => _stringCriterion;
            set
            {
                _stringCriterion = value;

                if (!string.IsNullOrEmpty(_stringCriterion))
                    Criterion = CriterionManager.Instance.Parse(value);
            }
        }

        [Ignore]
        public CriterionExpression? Criterion { get; private set; }

        public short Priority { get; set; }

        public string ParametersCSV { get; set; }

        [Ignore]
        public Dictionary<short, NpcItemRecord> Items { get; set; }
    }
}
