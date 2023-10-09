using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Items.Types;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests.Objectives
{
    [Identifier(QuestObjectiveType.UseItem)]
    public sealed class UseItemQuestObjective : QuestObjective
    {
        private short? _itemId;
        public short ItemId =>
            _itemId ??= GetParameterValue<short>(0);

        public UseItemQuestObjective(Quest quest, QuestObjectiveRecord record, Character owner, int progression) : base(quest, record, owner, progression) { }

        private void OnItemUsed(PlayerItem item)
        {
            if (item.Id == ItemId)
                Complete();
        }

        protected override void EnableObjective() =>
            _owner.Inventory.ItemUsed += OnItemUsed;

        protected override void DisableObjective() =>
            _owner.Inventory.ItemUsed -= OnItemUsed;
    }
}
