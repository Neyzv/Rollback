using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("PG")]
    public class BreedCriteria : BaseCriteria
    {
        private byte? _breedId;
        public byte BreedId =>
            _breedId ??= Value.ChangeType<byte>();

        public BreedCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            Compare((byte)character.Breed, BreedId);
    }
}
