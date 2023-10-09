using Rollback.Common.DesignPattern.Attributes;
using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Interactions;
using Rollback.World.Game.Interactions.Dialogs.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests.Objectives
{
    [Identifier(QuestObjectiveType.GiveItems)]
    public sealed class GiveItemsQuestObjective : QuestObjective
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

        public GiveItemsQuestObjective(Quest quest, QuestObjectiveRecord record, Character owner, int progression) : base(quest, record, owner, progression) { }

        private void OnInteracted(IInteraction interaction)
        {
            if (interaction is NpcDialog dialog && dialog.Dialoger.Record.Id == NpcId)
            {
                var items = _owner.Inventory.GetItems(x => x.Id == ItemId);
                if (items.Length > 0 && items.Sum(x => x.Quantity) >= Quantity)
                {
                    var deletedItems = _owner.Inventory.DeleteItems(items, Quantity);

                    // Tu as perdu %1 \'$item%2\'.
                    _owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, deletedItems, ItemId);

                    Complete();
                }
            }
        }

        protected override void EnableObjective() =>
            _owner.Interact += OnInteracted;

        protected override void DisableObjective() =>
            _owner.Interact -= OnInteracted;
    }
}
