using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.Items;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("BuyItems")]
    public sealed class BuyItemsReply : NpcReply
    {
        private Dictionary<short, int>? _itemsInfos;
        public Dictionary<short, int> ItemsInfos
        {
            get
            {
                var price = -1;

                _itemsInfos ??= ItemManager.Instance.ParseItemsInfos(GetParameterValue<string>(0)!, out price);

                if (price > 0)
                    TotalPrice = price;

                return _itemsInfos;
            }
        }


        public int TotalPrice { get; private set; }

        public BuyItemsReply(NpcReplyRecord record) : base(record) { }

        public override bool Execute(Npc npc, Character character)
        {
            if (ItemsInfos.Count is not 0)
            {
                if (character.Kamas >= Math.Abs(TotalPrice))
                {
                    character.ChangeKamas(TotalPrice);

                    foreach (var itemInfo in ItemsInfos)
                        character.Inventory.AddItem(itemInfo.Key, itemInfo.Value);
                }
                else
                    //Vous n\'avez pas assez de kamas pour effectuer cette action.
                    character.SendInformationMessage(Protocol.Enums.TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 82);
            }
            else
                character.ReplyError("Can not buy an empty list of items...");

            return true;
        }
    }
}
