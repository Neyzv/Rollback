using Rollback.Common.DesignPattern.Attributes;
using Rollback.Protocol.Enums;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Items;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests.Objectives.Events.Types
{
    [Identifier("DeleteItems")]
    public sealed class DeleteItemQuestObjectiveEvent : QuestObjectiveEvent
    {
        private Dictionary<short, int>? _itemInfos;
        public Dictionary<short, int> ItemsInfos =>
            _itemInfos ??= ItemManager.Instance.ParseItemsInfos(GetParameterValue<string>(0)!, out _);

        public DeleteItemQuestObjectiveEvent(Character owner, QuestObjective objective, QuestObjectiveEventRecord record) : base(owner, objective, record) { }

        public override void Trigger()
        {
            if (ItemsInfos is not null)
            {
                foreach (var (itemId, quantity) in ItemsInfos)
                {
                    var items = _owner.Inventory.GetItems(x => x.Id == itemId);

                    if (items.Length > 0)
                    {
                        var deletedItems = _owner.Inventory.DeleteItems(items, quantity is -1 ? items.Sum(x => x.Quantity) : quantity);

                        // Tu as perdu %1 \'$item%2\'.
                        _owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, deletedItems, itemId);
                    }
                }
            }
        }
    }
}
