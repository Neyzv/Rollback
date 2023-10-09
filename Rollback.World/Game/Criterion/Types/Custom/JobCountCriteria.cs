using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types.Custom
{
    [Identifier("PJC")]
    public sealed class JobCountCriteria : BaseCriteria
    {
        private byte? _jobCount;
        public byte JobCount =>
            _jobCount ??= Value.ChangeType<byte>();

        public JobCountCriteria(string identifier, Comparator comparator, string value, Operator op)
            : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            Compare(character.Jobs.Count, JobCount);
    }
}
