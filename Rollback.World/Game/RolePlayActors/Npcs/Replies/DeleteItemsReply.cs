using Rollback.Common.DesignPattern.Attributes;
using Rollback.Protocol.Enums;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.Items;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("DeleteItems")]
    public sealed class DeleteItemsReply : NpcReply
    {
        private Dictionary<short, int>? _itemsInfos;
        public Dictionary<short, int> ItemsInfos =>
            _itemsInfos ??= ItemManager.Instance.ParseItemsInfos(GetParameterValue<string>(0)!, out _);

        public DeleteItemsReply(NpcReplyRecord record) : base(record) { }

        public override bool Execute(Npc npc, Character character)
        {
            if (ItemsInfos.Count is not 0)
            {
                foreach (var (itemId, quantity) in ItemsInfos)
                {
                    var deletedItems = character.Inventory.DeleteItems(character.Inventory.GetItems(x => x.Id == itemId), quantity);

                    // Tu as perdu %1 \'$item%2\'.
                    character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, deletedItems, itemId);
                }
            }

            return true;
        }
    }
}
