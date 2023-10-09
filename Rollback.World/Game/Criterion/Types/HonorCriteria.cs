using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("CH")]
    public sealed class HonorCriteria : BaseCriteria
    {
        private ushort? _honor;
        public ushort Honor =>
            _honor ??= Value.ChangeType<ushort>();

        public HonorCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            Compare(character.Honor, Honor);
    }
}
