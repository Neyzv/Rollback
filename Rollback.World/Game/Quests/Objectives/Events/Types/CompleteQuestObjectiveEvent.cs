using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Quests;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests.Objectives.Events.Types
{
    [Identifier("CompleteQuestObjective")]
    public sealed class CompleteQuestObjectiveEvent : QuestObjectiveEvent
    {
        public CompleteQuestObjectiveEvent(Character owner, QuestObjective objective, QuestObjectiveEventRecord record)
            : base(owner, objective, record) { }

        public override void Trigger() =>
            _owner.CompleteQuestObjective(GetParameterValue<short>(0), GetParameterValue<short>(1));
    }
}
