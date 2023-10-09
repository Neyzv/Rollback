using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types.Custom
{
    [Identifier("Mp")]
    public sealed class MapIdCriteria : BaseCriteria
    {
        private int? _mapId;
        public int MapId =>
            _mapId ??= Value.ChangeType<int>();

        public MapIdCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            Comparator is Comparator.Inequal ? character.MapInstance.Map.Record.Id != MapId : character.MapInstance.Map.Record.Id == MapId;
    }
}
