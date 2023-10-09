using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("PK")]
    public sealed class KamasCriteria : BaseCriteria
    {
        private int? _kamas;
        public int Kamas =>
            _kamas ??= Value.ChangeType<int>();

        public KamasCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            Compare(character.Kamas, Kamas);
    }
}
