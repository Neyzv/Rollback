using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Items;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests.Objectives.Events.Types
{
    [Identifier("AddItems")]
    public sealed class AddItemQuestObjectiveEvent : QuestObjectiveEvent
    {
        private Dictionary<short, int>? _itemsInfos;
        public Dictionary<short, int> ItemsInfos =>
            _itemsInfos ??= ItemManager.Instance.ParseItemsInfos(GetParameterValue<string>(0)!, out _);

        public AddItemQuestObjectiveEvent(Character owner, QuestObjective objective, QuestObjectiveEventRecord record) : base(owner, objective, record) { }

        public override void Trigger()
        {
            foreach (var (itemId, quantity) in ItemsInfos)
                _owner.Inventory.AddItem(itemId, quantity);
        }
    }
}
