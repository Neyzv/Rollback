using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Interactions;
using Rollback.World.Game.Interactions.Dialogs.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests.Objectives
{
    [Identifier(QuestObjectiveType.NpcTalkBack)]
    public sealed class TalkBackQuestObjective : QuestObjective
    {
        private short? _npcId;
        public short NpcId =>
            _npcId ??= GetParameterValue<short>(0);

        public TalkBackQuestObjective(Quest quest, QuestObjectiveRecord record, Character owner, int progression) : base(quest, record, owner, progression) { }

        private void OnLeaveInteraction(Character character, IInteraction interaction)
        {
            if (interaction is NpcDialog dialog && dialog.Dialoger.Record.Id == NpcId)
            {
                var canComplete = false;
                var objectives = _quest.GetObjectives();

                for (var i = 0; i < objectives.Length && !canComplete; i++)
                    if (objectives[i] == this)
                    {
                        canComplete = true;
                        break;
                    }
                    else if (objectives[i].IsInProgress)
                        break;

                if (canComplete)
                    Complete();
            }
        }

        protected override void EnableObjective() =>
            _owner.LeaveInteraction += OnLeaveInteraction;

        protected override void DisableObjective() =>
            _owner.LeaveInteraction -= OnLeaveInteraction;
    }
}
