using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Items;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Items;
using Rollback.World.Game.Items.Types;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Inventory
{
    public static class InventoryHandler
    {
        [WorldHandler(ObjectDeleteMessage.Id)]
        public static void HandleObjectDeleteMessage(WorldClient client, ObjectDeleteMessage message)
        {
            if ((client.Account!.Character!.Fighter is null || !client.Account.Character.Fighter.Team!.Fight.Started))
                if (client.Account.Character.Inventory.GetItemByUID(message.objectUID) is { } item)
                {
                    if (item.TypeId is not ItemType.ObjetDeQuete && item.TypeId is not ItemType.Dofus &&
                        !item.IsLinked())
                        client.Account!.Character!.Inventory.RemoveItem(item, message.quantity);
                }
                else
                    //Tu ne possèdes pas l\'objet nécessaire.
                    client.Account.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 4);
        }

        [WorldHandler(ObjectSetPositionMessage.Id)]
        public static void HandleObjectSetPositionMessage(WorldClient client, ObjectSetPositionMessage message)
        {
            if ((client.Account!.Character!.Fighter is null || !client.Account.Character.Fighter.Team!.Fight.Started) &&
                Enum.IsDefined(typeof(CharacterInventoryPositionEnum), (int)message.position))
                client.Account.Character.Inventory.MoveItem(message.objectUID, (CharacterInventoryPositionEnum)message.position);
        }

        [WorldHandler(CharacterItemSetInfosRequestMessage.Id)]
        public static void HandleCharacterItemSetInfosRequestMessage(WorldClient client, CharacterItemSetInfosRequestMessage message)
        {
            var set = ItemManager.Instance.GetSetRecordById(message.setId);
            if (set is not null)
                SendCharacterItemSetInfosResponseMessage(client, set);
        }

        [WorldHandler(ObjectUseMessage.Id)]
        public static void HandleObjectUseMessage(WorldClient client, ObjectUseMessage message)
        {
            if (client.Account!.Character!.Fighter is null || !client.Account.Character.Fighter.Team!.Fight.Started)
                client.Account!.Character!.Inventory.UseItem(message.objectUID);
        }

        [WorldHandler(ObjectUseOnCellMessage.Id)]
        public static void HandleObjectUseOnCellMessage(WorldClient client, ObjectUseOnCellMessage message)
        {
            if ((client.Account!.Character!.Fighter is null || !client.Account.Character.Fighter.Team!.Fight.Started) &&
                Cell.CellIdValid(message.cells))
                client.Account!.Character!.Inventory.UseItem(message.objectUID, targetedCell: client.Account.Character.MapInstance.Map.Record.Cells[message.cells]);
        }

        [WorldHandler(ObjectUseOnCharacterMessage.Id)]
        public static void HandleObjectUseOnCharacterMessage(WorldClient client, ObjectUseOnCharacterMessage message)
        {
            if ((client.Account!.Character!.Fighter is null || !client.Account.Character.Fighter.Team!.Fight.Started))
            {
                var target = client.Account.Character.MapInstance.GetActor<Character>(x => x.Id == message.characterId);
                if (target is not null)
                    client.Account.Character.Inventory.UseItem(message.objectUID, targetedCell: target.Cell);
            }
        }

        public static void SendKamasUpdateMessage(WorldClient client) =>
            client.Send(new KamasUpdateMessage(client.Account!.Character!.Kamas));

        public static void SendInventoryContentMessage(WorldClient client) =>
            client.Send(new InventoryContentMessage(client.Account!.Character!.Inventory.InventoryContent, client.Account!.Character!.Kamas));

        public static void SendInventoryWeightMessage(WorldClient client) =>
            client.Send(new InventoryWeightMessage(client.Account!.Character!.Inventory.Pods, client.Account.Character.Inventory.MaxPods));

        public static void SendSpellListMessage(WorldClient client) =>
            client.Send(new SpellListMessage(true, client.Account!.Character!.GetSpells().Select(x => x.SpellItem).ToArray()));

        public static void SendObjectAddedMessage(WorldClient client, PlayerItem item) =>
            client.Send(new ObjectAddedMessage(item.ObjectItem));

        public static void SendObjectDeletedMessage(WorldClient client, PlayerItem item) =>
            client.Send(new ObjectDeletedMessage(item.UID));

        public static void SendObjectModifiedMessage(WorldClient client, PlayerItem item) =>
            client.Send(new ObjectModifiedMessage(item.ObjectItem));

        public static void SendObjectQuantityMessage(WorldClient client, PlayerItem item) =>
            client.Send(new ObjectQuantityMessage(item.UID, item.Quantity));

        public static void SendObjectErrorMessage(WorldClient client, ObjectErrorEnum reason) =>
            client.Send(new ObjectErrorMessage((sbyte)reason));

        public static void SendObjectMovementMessage(WorldClient client, PlayerItem item) =>
            client.Send(new ObjectMovementMessage(item.UID, (byte)item.Position));

        public static void SendCharacterItemSetInfosResponseMessage(WorldClient client, ItemSetRecord set)
        {
            var nbrOfItems = client.Account!.Character!.Inventory.GetAmountOfEquipedItemsForSet(set.Id);
            client.Send(new CharacterItemSetInfosResponseMessage(nbrOfItems > 1 ? EffectManager.GetObjectEffects(set.Effects[nbrOfItems - 2]) : Array.Empty<ObjectEffect>()));
        }

        public static void SendStorageInventoryContentMessage(WorldClient client, ObjectItem[] items, int kamas) =>
            client.Send(new StorageInventoryContentMessage(items, kamas));
    }
}
