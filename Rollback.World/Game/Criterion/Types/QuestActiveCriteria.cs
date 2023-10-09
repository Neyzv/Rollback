using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("Qa")]
    public sealed class QuestActiveCriteria : BaseCriteria
    {
        private short? _questId;
        public short QuestId =>
            _questId ??= Value.ChangeType<short>();

        public QuestActiveCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            character.GetQuest(QuestId)?.IsFinished == false;
    }
}
