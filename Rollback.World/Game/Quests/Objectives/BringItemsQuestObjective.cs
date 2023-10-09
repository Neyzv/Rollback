using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Interactions;
using Rollback.World.Game.Interactions.Dialogs.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests.Objectives
{
    [Identifier(QuestObjectiveType.BringItems)]
    public sealed class BringItemsQuestObjective : QuestObjective
    {
        private int? _npcId;
        public int NpcId =>
            _npcId ??= GetParameterValue<int>(0);

        private short? _itemId;
        public short ItemId =>
            _itemId ??= GetParameterValue<short>(1);

        private int? _quantity;
        public int Quantity =>
            _quantity ??= GetParameterValue<int>(2);

        public BringItemsQuestObjective(Quest quest, QuestObjectiveRecord record, Character owner, int progression)
            : base(quest, record, owner, progression) { }

        private void OnInteracted(IInteraction interaction)
        {
            if (interaction is NpcDialog dialog && dialog.Dialoger.Record.Id == NpcId &&
                _owner.Inventory.GetItems(x => x.Id == ItemId).Sum(x => x.Quantity) >= Quantity)
                Complete();
        }

        protected override void EnableObjective() =>
            _owner.Interact += OnInteracted;

        protected override void DisableObjective() =>
            _owner.Interact -= OnInteracted;
    }
}
