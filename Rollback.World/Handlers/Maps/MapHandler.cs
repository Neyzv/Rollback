using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;
using Rollback.Protocol.Types;
using Rollback.World.Game.Items.Types;
using Rollback.World.Game.Mounts;
using Rollback.World.Game.RolePlayActors;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Mounts;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Maps
{
    public static class MapHandler
    {
        [WorldHandler(MapInformationsRequestMessage.Id)]
        public static void HandleMapInformationsRequestMessage(WorldClient client, MapInformationsRequestMessage message)
        {
            SendMapComplementaryInformationsDataMessage(client);
            SendObjectGroundListAddedMessage(client);
            
            if (MountManager.Instance.GetPaddock(client.Account!.Character!.MapInstance.Map.Record.Id) is { } paddock)
            {
                MountHandler.SendPaddockPropertiesMessage(client, paddock);
                    
                if(paddock.GetPaddockItems(client.Account.Character) is {Length: > 0} items)
                    MountHandler.SendGameDataPaddockObjectListAddMessage(client, items);
            }
        }

        [WorldHandler(GameMapMovementRequestMessage.Id)]
        public static void HandleGameMapMovementRequestMessage(WorldClient client, GameMapMovementRequestMessage message)
        {
            if (message.keyMovements.Length > 1)
            {
                var cellId = Cell.KeyMovementToCellId(message.keyMovements[^1]);
                if (Cell.CellIdValid(cellId))
                {
                    if (client.Account!.Character!.Fighter is not null && client.Account.Character.Fighter.Team?.Fight?.Started == true)
                        client.Account.Character.Fighter.Move(message.keyMovements);
                    else if (client.Account.Character.Fighter is null && !client.Account.Character.IsBusy && client.Account.Character.MapInstance.Map.Record.Cells[cellId]?.Walkable == true)
                        client.Account.Character.StartMove(message.keyMovements);
                    else
                        SendGameMapNoMovementMessage(client);
                }
                else
                    SendGameMapNoMovementMessage(client);
            }
            else
                SendGameMapNoMovementMessage(client);
        }


        [WorldHandler(GameMapMovementConfirmMessage.Id)]
        public static void HandleGameMapMovementConfirmMessage(WorldClient client, GameMapMovementConfirmMessage message) =>
            client.Account!.Character!.StopMove();

        [WorldHandler(GameMapMovementCancelMessage.Id)]
        public static void HandleGameMapMovementCancelMessage(WorldClient client, GameMapMovementCancelMessage message) =>
            client.Account!.Character!.StopMove(message.cellId);


        [WorldHandler(ChangeMapMessage.Id)]
        public static void HandleChangeMapMessage(WorldClient client, ChangeMapMessage message)
        {
            if (!client.Account!.Character!.IsBusy)
            {
                if (client.Account.Character.Cell.Point.GetAdjacentCells(diagonal: true).Count() is not Cell.AdjacentCellsCount) // TO DO check si sur la bonne bordure
                {
                    var cellId = client.Account!.Character!.Cell.Id;

                    if (client.Account!.Character!.MapInstance.Map.Record.TopNeighbourId == message.mapId)
                        cellId = (short)(client.Account.Character.MapInstance.Map.Record.TopNeighbourCellId.HasValue ? client.Account.Character.MapInstance.Map.Record.TopNeighbourCellId!.Value : cellId + 532);
                    else if (client.Account.Character.MapInstance.Map.Record.BottomNeighbourId == message.mapId)
                        cellId = (short)(client.Account.Character.MapInstance.Map.Record.BottomNeighbourCellId.HasValue ? client.Account.Character.MapInstance.Map.Record.BottomNeighbourCellId!.Value : cellId - 532);
                    else if (client.Account.Character.MapInstance.Map.Record.LeftNeighbourId == message.mapId)
                        cellId = (short)(client.Account.Character.MapInstance.Map.Record.LeftNeighbourCellId.HasValue ? client.Account.Character.MapInstance.Map.Record.LeftNeighbourCellId!.Value : cellId + 13);
                    else if (client.Account.Character.MapInstance.Map.Record.RightNeighbourId == message.mapId)
                        cellId = (short)(client.Account.Character.MapInstance.Map.Record.RightNeighbourCellId.HasValue ? client.Account.Character.MapInstance.Map.Record.RightNeighbourCellId!.Value : cellId - 13);
                    else
                        return;

                    client.Account!.Character!.Teleport(message.mapId, cellId);
                }
                else
                    client.SafeBotBan();
            }
        }

        [WorldHandler(GameMapChangeOrientationRequestMessage.Id)]
        public static void HandleGameMapChangeOrientationRequestMessage(WorldClient client, GameMapChangeOrientationRequestMessage message)
        {
            if (client.Account!.Character!.Fighter is null && Enum.IsDefined(typeof(DirectionsEnum), (int)message.direction))
                client.Account.Character.ChangeDirection((DirectionsEnum)message.direction);
        }

        [WorldHandler(ObjectDropMessage.Id)]
        public static void HandleObjectDropMessage(WorldClient client, ObjectDropMessage message)
        {
            if (!client.Account!.Character!.IsBusy)
                client.Account.Character.Inventory.DropItem(message.objectUID, message.quantity);
        }

        public static void SendGameRolePlayShowActorMessage(WorldClient client, RolePlayActor actor) =>
            client.Send(new GameRolePlayShowActorMessage(actor.GameRolePlayActorInformations(client.Account!.Character!)));

        public static void SendGameContextRemoveElementMessage(WorldClient client, int actorId) =>
            client.Send(new GameContextRemoveElementMessage(actorId));

        public static void SendMapComplementaryInformationsDataMessage(WorldClient client) => // TO DO
            client.Send(new MapComplementaryInformationsDataMessage(client.Account!.Character!.Id, client.Account.Character.MapInstance.Map.Record.SubAreaId,
                Array.Empty<HouseInformations>(), client.Account.Character.MapInstance.GetActors<RolePlayActor>(x => x.CanBeSee(client.Account.Character)).Select(x => x.GameRolePlayActorInformations(client.Account.Character)).ToArray(),
                client.Account.Character.MapInstance.GetInteractives().Select(x => x.GetInteractiveElement(client.Account.Character)).Where(x => x.enabledSkillIds.Length is not 0 || x.disabledSkillIds.Any()).ToArray(),
                client.Account.Character.MapInstance.GetInteractives(x => x.Animated).Select(x => x.StatedElement).ToArray(),
                client.Account.Character.MapInstance.GetObstacles(), client.Account.Character.MapInstance.GetFights(x => !x.Started).Select(x => x.FightCommonInformations).ToArray()));

        public static void SendGameMapNoMovementMessage(WorldClient client) =>
            client.Send(new GameMapNoMovementMessage());

        public static void SendGameMapMovementMessage(WorldClient client, int entityId, short[] keyMovements) =>
            client.Send(new GameMapMovementMessage(entityId, keyMovements));

        public static void SendGameMapChangeOrientationMessage(WorldClient client, RolePlayActor actor) =>
            client.Send(new GameMapChangeOrientationMessage(actor.Id, (sbyte)actor.Direction));

        public static void SendGameContextRefreshEntityLookMessage(WorldClient client, RolePlayActor actor) =>
            client.Send(new GameContextRefreshEntityLookMessage(actor.Id, actor.Look.GetEntityLook()));

        public static void SendObjectGroundAddedMessage(WorldClient client, MapItem mapItem) =>
            client.Send(new ObjectGroundAddedMessage(mapItem.Cell.Id, mapItem.Item.Id));

        public static void SendObjectGroundListAddedMessage(WorldClient client)
        {
            var items = client.Account!.Character!.MapInstance?.GetItems();

            if (items is not null && items.Length is not 0)
            {
                var cells = new short[items.Length];
                var itemTempIds = new int[items.Length];

                for (var i = 0; i < items.Length; i++)
                {
                    cells[i] = items[i].Cell.Id;
                    itemTempIds[i] = items[i].Item.Id;
                }

                client.Send(new ObjectGroundListAddedMessage(cells, itemTempIds));
            }
        }

        public static void SendObjectGroundRemovedMessage(WorldClient client, short cellId) =>
            client.Send(new ObjectGroundRemovedMessage(cellId));

        public static void SendMapFightCountMessage(WorldClient client, short fightCount) =>
            client.Send(new MapFightCountMessage(fightCount));

        public static void SendMapObstacleUpdateMessage(WorldClient client, MapObstacle[] obstacles) =>
            client.Send(new MapObstacleUpdateMessage(obstacles));
    }
}

