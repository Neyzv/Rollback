using Rollback.Protocol.Messages;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Items;
using Rollback.World.Database.World;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Mounts;
using Rollback.World.Game.Mounts;
using Rollback.World.Game.World.Maps;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Mounts
{
    public static class MountHandler
    {
        [WorldHandler(MountInformationRequestMessage.Id)]
        public static void HandleMountInformationRequestMessage(WorldClient client, MountInformationRequestMessage message)
        {
            if (MountManager.Instance.GetMountById((int)message.id) is { } mount)
                SendMountDataMessage(client, mount);
        }

        [WorldHandler(ExchangeHandleMountStableMessage.Id)]
        public static void HandleExchangeHandleMountStableMessage(WorldClient client, ExchangeHandleMountStableMessage message)
        {
            if (client.Account!.Character!.Interaction is PaddockExchange exchange)
            {
                switch ((StableExchangeAction)message.actionType)
                {
                    case StableExchangeAction.InventoryToEquip:
                        exchange.InventoryToEquip(message.rideId);
                        break;
                    
                    case StableExchangeAction.EquipToInventory:
                        exchange.EquipToInventory(message.rideId);
                        break;
                    
                    case StableExchangeAction.EquipToStable:
                        exchange.EquipToStable(message.rideId);
                        break;
                    
                    case StableExchangeAction.StableToEquip:
                        exchange.StableToEquip(message.rideId);
                        break;
                    
                    case StableExchangeAction.StableToInventory:
                        exchange.StableToInventory(message.rideId);
                        break;
                    
                    case StableExchangeAction.InventoryToStable:
                        exchange.InventoryToStable(message.rideId);
                        break;
                    
                    case StableExchangeAction.StableToPaddock:
                        break;
                    
                    case StableExchangeAction.PaddockToStable:
                        break;
                    
                    case StableExchangeAction.EquipToPaddock:
                        break;
                    
                    case StableExchangeAction.PaddockToEquip:
                        break;
                    
                    case StableExchangeAction.PaddockToInventory:
                        break;
                    
                    case StableExchangeAction.InventoryToPaddock:
                        break;
                }
            }
        }

        [WorldHandler(MountToggleRidingRequestMessage.Id)]
        public static void HandleMountToggleRidingRequestMessage(WorldClient client, MountToggleRidingRequestMessage message)
        {
            if (!client.Account!.Character!.IsBusy ||
                client.Account.Character.Fighter?.Team?.Fight.State is FightState.Placement)
            {
                if(client.Account!.Character!.IsRiding)
                    client.Account.Character.Dismount();
                else
                    client.Account.Character.RideMount();
            }
        }

        [WorldHandler(MountRenameRequestMessage.Id)]
        public static void HandleMountRenameRequestMessage(WorldClient client, MountRenameRequestMessage message)
        {
            if (MountManager.Instance.GetMountById((int)message.mountId) is { } mount)
            {
                if (mount.AccountId == client.Account!.Id)
                {
                    if(mount.Id == client.Account.Character!.EquipedMount?.Id ||
                       client.Account.Character.Interaction is PaddockExchange)
                        mount.RenameMount(message.name);
                }
            }
        }

        [WorldHandler(MountSetXpRatioRequestMessage.Id)]
        public static void HandleMountSetXpRatioRequestMessage(WorldClient client,
            MountSetXpRatioRequestMessage message) =>
            client.Account!.Character!.ChangeMountXpPercent(message.xpRatio);

        public static void SendMountDataMessage(WorldClient client, Mount mount) =>
            client.Send(new MountDataMessage(mount.MountClientData));

        public static void SendPaddockPropertiesMessage(WorldClient client, Paddock paddock) =>
            client.Send(new PaddockPropertiesMessage(paddock.GetPaddockInformations(client.Account!.Character!)));

        public static void SendExchangeStartOkMountMessage(WorldClient client, IEnumerable<Mount> stabledMounts,
            IEnumerable<Mount> paddockedMounts) =>
            client.Send(new ExchangeStartOkMountMessage(stabledMounts.Select(x => x.MountClientData).ToArray(),
                paddockedMounts.Select(x => x.MountClientData).ToArray()));

        public static void SendMountSetMessage(WorldClient client) =>
            client.Send(new MountSetMessage(client.Account!.Character!.EquipedMount!.MountClientData));

        public static void SendMountXpRatioMessage(WorldClient client) =>
            client.Send(new MountXpRatioMessage(client.Account!.Character!.MountXpPercent));

        public static void SendMountUnSetMessage(WorldClient client) =>
            client.Send(new MountUnSetMessage());

        public static void SendMountRidingMessage(WorldClient client) =>
            client.Send(new MountRidingMessage(client.Account!.Character!.IsRiding));

        public static void SendMountRenamedMessage(WorldClient client, Mount mount) =>
            client.Send(new MountRenamedMessage(mount.Id, mount.Name));

        public static void SendExchangeMountStableAddMessage(WorldClient client, Mount mount) =>
            client.Send(new ExchangeMountStableAddMessage(mount.MountClientData));

        public static void SendExchangeMountStableRemoveMessage(WorldClient client, Mount mount) =>
            client.Send(new ExchangeMountStableRemoveMessage(mount.Id));

        public static void SendGameDataPaddockObjectAddMessage(WorldClient client, PaddockItemRecord item) =>
            client.Send(new GameDataPaddockObjectAddMessage(item.PaddockItem));

        public static void SendGameDataPaddockObjectListAddMessage(WorldClient client, PaddockItem[] items) =>
            client.Send(new GameDataPaddockObjectListAddMessage(items));
    }
}
