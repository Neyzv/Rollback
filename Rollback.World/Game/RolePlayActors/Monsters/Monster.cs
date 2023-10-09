using Rollback.Common.Logging;
using Rollback.Protocol.Types;
using Rollback.World.Database.Monsters;
using Rollback.World.Game.Looks;
using Rollback.World.Game.Spells;
using Rollback.World.Game.Stats;

namespace Rollback.World.Game.RolePlayActors.Monsters
{
    public sealed class Monster
    {
        private readonly Dictionary<short, Spell> _spells;

        private readonly MonsterRecord _record;
        private MonsterGradeRecord GradeRecord =>
            _record.Grades[GradeId - 1];

        public short Id =>
            _record.Id;

        public sbyte GradeId { get; private set; }

        public ActorLook Look { get; }

        public int Health { get; set; }

        public int MaxHealth =>
            Health;

        public short Level { get; private set; }

        public StatsData Stats { get; }

        public long XP { get; set; }

        public int MinKamas { get; set; }

        public int MaxKamas { get; set; }

        public int Race =>
            _record.Race;

        public IReadOnlyDictionary<short, Spell> Spells =>
            _spells;

        private List<MonsterDropRecord>? _drops;
        public List<MonsterDropRecord> Drops =>
            _drops ??= _record.Drops.Select(x => x with { }).ToList();

        public MonsterInGroupInformations MonsterInGroupInformations =>
            new(Id, Level, Look.GetEntityLook());

        public Monster(MonsterRecord record, sbyte gradeId)
        {
            _record = record;

            if (_record.Grades.Count < gradeId)
                Logger.Instance.LogError(default, $"Incorrect grade {gradeId} for monster {record.Id}...");

            GradeId = gradeId;

            _spells = new(GradeRecord.Spells);

            Look = ActorLook.Parse(_record.EntityLookString);
            Health = GradeRecord.Health;
            Level = GradeRecord.Level;
            Stats = StatsData.CreateStats(GradeRecord);
            XP = GradeRecord.XP;
            MinKamas = GradeRecord.MinKamas;
            MaxKamas = GradeRecord.MaxKamas;
        }
    }
}
