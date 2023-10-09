using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Quests;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests.Objectives.Events.Types
{
    [Identifier("AddExperience")]
    public sealed class AddExperienceQuestObjectiveEvent : QuestObjectiveEvent
    {
        public AddExperienceQuestObjectiveEvent(Character owner, QuestObjective objective, QuestObjectiveEventRecord record)
            : base(owner, objective, record) { }

        public override void Trigger() =>
            _owner.ChangeExperience(GetParameterValue<long>(0));
    }
}
