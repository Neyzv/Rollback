using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("Qo")]
    public sealed class QuestObjectiveCriteria : BaseCriteria
    {
        private short? _questObjectiveId;
        public short QuestObjectiveId =>
            _questObjectiveId ??= Value.ChangeType<short>();

        public QuestObjectiveCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
             character.GetQuest(x => !x.IsFinished && x.GetObjective(y => y.Id == QuestObjectiveId && (Comparator is Comparator.Equal ? !y.IsInProgress : y.IsInProgress)) is not null) is not null;
    }
}
