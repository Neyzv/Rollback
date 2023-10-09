using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("PS")]
    public sealed class SexCriteria : BaseCriteria
    {
        private byte? _sex;
        public byte Sex =>
            _sex ??= Value.ChangeType<byte>();

        public SexCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            Compare(character.Sex is true ? 1 : 0, Sex);
    }
}
