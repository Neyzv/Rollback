using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("Pj"), Identifier("PJ")]
    public sealed class JobCriteria : BaseCriteria
    {
        public JobCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character)
        {
            var splittedValue = Value.Split(','); // TODO Rework et aussi celui haveManyitems

            return splittedValue.Length > 0 && sbyte.TryParse(splittedValue[0], out var jobId) &&
                (Comparator is Comparator.Equal ? character.Jobs.ContainsKey((JobIds)jobId) && (splittedValue.Length < 2 ||
                (byte.TryParse(splittedValue[1], out var jobLevel) && character.Jobs[(JobIds)jobId].Level >= jobLevel))
                : !character.HaveJob((JobIds)jobId));
        }
    }
}
