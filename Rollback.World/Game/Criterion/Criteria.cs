using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion
{
    public abstract class Criteria
    {
        public Operator Operator { get; }

        public Criteria(Operator op) =>
            Operator = op;

        public abstract bool Eval(Character character);
    }
}
