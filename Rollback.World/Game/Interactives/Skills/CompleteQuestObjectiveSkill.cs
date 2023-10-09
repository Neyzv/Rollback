using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Interactives;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Interactives.Skills
{
    [Identifier(185)]
    public sealed class CompleteQuestObjectiveSkill : Skill
    {
        private short? _questId;
        public short QuestId =>
            _questId ??= GetParameterValue<short>(0);

        private short? _questObjectiveId;
        public short QuestObjectiveId =>
            _questObjectiveId ??= GetParameterValue<short>(1);

        public CompleteQuestObjectiveSkill(InteractiveObject interactive, InteractiveSkillRecord record)
            : base(interactive, record) { }

        public override void Execute(Character character) =>
            character.CompleteQuestObjective(QuestId, QuestObjectiveId);
    }
}
