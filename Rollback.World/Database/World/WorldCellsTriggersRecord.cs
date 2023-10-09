using Rollback.Common.ORM;
using Rollback.World.Game.Criterion;
using Rollback.World.Game.World.Maps.Triggers;

namespace Rollback.World.Database.World
{
    public static class WorldCellsTriggersRelator
    {
        public const string GetQueries = "SELECT * FROM world_cells_triggers";
    }

    [Table("world_cells_triggers")]
    public sealed record WorldCellsTriggersRecord
    {
        public WorldCellsTriggersRecord() =>
            (Action, _stringCriterion, ParametersCSV) = (string.Empty, string.Empty, string.Empty);

        public int MapId { get; set; }

        public short CellId { get; set; }

        public string Action { get; set; }

        public CellTriggerType Type { get; set; }

        private string _stringCriterion;
        public string StringCriterion
        {
            get => _stringCriterion;
            set
            {
                _stringCriterion = value;

                if (!string.IsNullOrEmpty(value))
                    Criterion = CriterionManager.Instance.Parse(value);
            }
        }

        [Ignore]
        public CriterionExpression? Criterion { get; private set; }

        public short Priority { get; set; }

        public string ParametersCSV { get; set; }
    }
}
