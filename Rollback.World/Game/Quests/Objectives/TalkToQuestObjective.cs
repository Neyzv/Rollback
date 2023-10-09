using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Interactions;
using Rollback.World.Game.Interactions.Dialogs.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests.Objectives
{
    [Identifier(QuestObjectiveType.TalkToNpc)]
    public sealed class TalkToQuestObjective : QuestObjective
    {
        private short? _npcId;
        public short NpcId =>
            _npcId ??= GetParameterValue<short>(0);

        public TalkToQuestObjective(Quest quest, QuestObjectiveRecord record, Character owner, int progression) : base(quest, record, owner, progression) { }

        private void OnInteracted(IInteraction interaction)
        {
            if (interaction is NpcDialog dialog && dialog.Dialoger.Record.Id == NpcId)
                Complete();
        }

        protected override void EnableObjective() =>
            _owner.Interact += OnInteracted;

        protected override void DisableObjective() =>
            _owner.Interact -= OnInteracted;
    }
}
