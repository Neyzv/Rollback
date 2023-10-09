using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("CD")]
    public sealed class DishonorCriteria : BaseCriteria
    {
        private ushort? _dishonor;
        public ushort Dishonor =>
            _dishonor ??= Value.ChangeType<ushort>();

        public DishonorCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            Compare(character.Dishonor, Dishonor);
    }
}
