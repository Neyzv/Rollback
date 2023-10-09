using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Interactions;
using Rollback.World.Game.Interactions.Dialogs.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests.Objectives
{
    [Identifier(QuestObjectiveType.BringSouls)]
    public sealed class BringSoulsQuestObjective : QuestObjective
    {
        private short? _npcId;
        public short NpcId =>
            _npcId ??= GetParameterValue<short>(0);

        private short? _monsterId;
        public short MonsterId =>
            _monsterId ??= GetParameterValue<short>(1);

        private int? _quantity;
        public int Quantity =>
            _quantity ??= GetParameterValue<int>(2);

        protected override int ProgressionReference =>
            Quantity;

        public BringSoulsQuestObjective(Quest quest, QuestObjectiveRecord record, Character owner, int progression) : base(quest, record, owner, progression) { }

        private void OnInteracted(IInteraction interaction)
        {
            if (interaction is NpcDialog dialog && dialog.Dialoger.Record.Id == NpcId)
            {
                foreach (var item in _owner.Inventory.GetItems(x => x.TypeId is ItemType.PierreDAmePleine))
                {
                    // TO DO
                }
            }
        }

        protected override void EnableObjective() =>
            _owner.Interact += OnInteracted;

        protected override void DisableObjective() =>
            _owner.Interact -= OnInteracted;
    }
}
