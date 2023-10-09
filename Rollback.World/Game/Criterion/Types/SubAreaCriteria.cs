using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("PB")]
    public sealed class SubAreaCriteria : BaseCriteria
    {
        private short? _subAreaId;
        public short SubAreaId =>
            _subAreaId ??= Value.ChangeType<short>();

        public SubAreaCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            Compare(character.MapInstance.Map.Record.SubAreaId, SubAreaId);
    }
}
