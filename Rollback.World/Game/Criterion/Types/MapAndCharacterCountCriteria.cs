using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("MK")]
    public sealed class MapAndCharacterCountCriteria : BaseCriteria
    {
        public int MapId { get; }

        public int CharacterCount { get; }

        public MapAndCharacterCountCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op)
        {
            var valueSplitted = value.Split(',');

            MapId = valueSplitted[0].ChangeType<int>();
            CharacterCount = valueSplitted[1].ChangeType<int>();
        }

        public override bool Eval(Character character) =>
            Compare(character.MapInstance.Map.Record.Id, MapId) && Compare(character.MapInstance.GetActors<Character>().Length, CharacterCount);
    }
}
