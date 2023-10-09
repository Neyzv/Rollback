using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion
{
    public sealed class CriterionExpression : Criteria
    {
        private readonly List<Criteria> _criterion;

        public CriterionExpression(List<Criteria> criterion, Operator op) : base(op) =>
            _criterion = criterion;

        public override bool Eval(Character character)
        {
            var res = default(bool?);
            var op = Operator.None;

            for (var i = 0; i < _criterion.Count && (!res.HasValue || (res == true ? op is Operator.And : op is Operator.Or)); i++)
            {
                res = _criterion[i].Eval(character);
                op = _criterion[i].Operator;
            }

            return !res.HasValue || res.Value;
        }
    }
}
