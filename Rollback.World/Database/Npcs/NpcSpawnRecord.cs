using Rollback.Common.ORM;
using Rollback.Protocol.Enums;
using Rollback.World.Game.Criterion;
using Rollback.World.Game.RolePlayActors.Npcs;

namespace Rollback.World.Database.Npcs
{
    public static class NpcSpawnRelator
    {
        public const string GetSpawns = "SELECT * FROM npcs_spawns";
    }

    [Table("npcs_spawns")]
    public sealed record NpcSpawnRecord
    {
        public NpcSpawnRecord() =>
            _stringCriterion = string.Empty;

        [Key]
        public int Id { get; set; }

        private short _npcId;
        public short NpcId
        {
            get => _npcId;
            set
            {
                _npcId = value;
                Npc = NpcManager.Instance.GetNpcRecordById(value);
            }
        }

        [Ignore]
        public NpcRecord? Npc { get; private set; }

        public int MapId { get; set; }

        public short CellId { get; set; }

        public DirectionsEnum Direction { get; set; }

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
    }
}
