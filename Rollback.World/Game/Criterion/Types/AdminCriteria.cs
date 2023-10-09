using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("PX")]
    public sealed class AdminCriteria : BaseCriteria
    {
        public AdminCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            character.Client.Account!.Role > Protocol.Enums.GameHierarchyEnum.PLAYER;
    }
}
