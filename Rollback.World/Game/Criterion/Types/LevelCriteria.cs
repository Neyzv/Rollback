using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("PL")]
    public sealed class LevelCriteria : BaseCriteria
    {
        private byte? _level;
        public byte Level =>
            _level ??= Value.ChangeType<byte>();

        public LevelCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            Compare(character.Level, Level);
    }
}
