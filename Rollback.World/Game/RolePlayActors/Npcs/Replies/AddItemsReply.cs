using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.Items;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("AddItems")]
    public sealed class AddItemsReply : NpcReply
    {
        private Dictionary<short, int>? _itemsInfos;
        public Dictionary<short, int> ItemsInfos =>
            _itemsInfos ??= ItemManager.Instance.ParseItemsInfos(GetParameterValue<string>(0)!, out _);

        public AddItemsReply(NpcReplyRecord record)
            : base(record) { }

        public override bool Execute(Npc npc, Character character)
        {
            foreach (var (itemId, quantity) in ItemsInfos)
                character.Inventory.AddItem(itemId, quantity);

            return true;
        }
    }
}
