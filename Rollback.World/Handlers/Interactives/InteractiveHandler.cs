using Rollback.Protocol.Messages;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Interactions.Dialogs.Interactives;
using Rollback.World.Game.Interactives;
using Rollback.World.Game.World.Maps;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Interactives
{
    public static class InteractiveHandler
    {
        [WorldHandler(InteractiveUseRequestMessage.Id)]
        public static void HandleInteractiveUseRequestMessage(WorldClient client, InteractiveUseRequestMessage message)
        {
            if (client.Account!.Character!.Fighter is null)
                client.Account.Character.MapInstance?.GetInteractiveById(message.elemId)?.Use(client.Account.Character, message.skillId);
        }

        [WorldHandler(TeleportRequestMessage.Id)]
        public static void HandleTeleportRequestMessage(WorldClient client, TeleportRequestMessage message)
        {
            if (client.Account!.Character!.Interaction is ZaapDialog dialog)
                dialog.Teleport(message.mapId);
        }

        public static void SendInteractiveMapUpdateMessage(WorldClient client, MapInstance map) =>
            client.Send(new InteractiveMapUpdateMessage(map.GetInteractives().Select(x => x.GetInteractiveElement(client.Account!.Character!)).ToArray()));

        public static void SendInteractiveUsedMessage(WorldClient client, int entityId, InteractiveObject interactive, Skill skill) =>
            client.Send(new InteractiveUsedMessage(entityId, interactive.Id, skill.TemplateId!.Value, (short)(skill.Duration / 100)));

        public static void SendInteractiveUseEndedMessage(WorldClient client, InteractiveObject interactive, Skill skill) =>
            client.Send(new InteractiveUseEndedMessage(interactive.Id, skill.TemplateId!.Value));

        private static (int[], short[], short[]) AssignTeleportDestinationsInformations(Dictionary<int, Map> maps, Func<Map, short> costFunc)
        {
            var mapIds = new int[maps.Count];
            var subAreaIds = new short[maps.Count];
            var costs = new short[maps.Count];

            var i = 0;
            foreach (var map in maps.Values)
            {
                mapIds[i] = map.Record.Id;
                subAreaIds[i] = map.Record.SubAreaId;
                costs[i] = costFunc(map);

                i++;
            }

            return (mapIds, subAreaIds, costs);
        }

        public static void SendTeleportDestinationsListMessage(WorldClient client, TeleporterType type, Dictionary<int, Map> maps, Func<Map, short> costFunc)
        {
            var (mapIds, subAreaIds, costs) = AssignTeleportDestinationsInformations(maps, costFunc);

            client.Send(new TeleportDestinationsListMessage((sbyte)type, mapIds, subAreaIds, costs));
        }

        public static void SendZaapListMessage(WorldClient client, Dictionary<int, Map> maps, Func<Map, short> costFunc)
        {
            var (mapIds, subAreaIds, costs) = AssignTeleportDestinationsInformations(maps, costFunc);

            client.Send(new ZaapListMessage((sbyte)TeleporterType.Zaap, mapIds, subAreaIds, costs, client.Account!.Character!.SaveMapId ?? -1));
        }

        public static void SendInteractiveElementUpdatedMessage(WorldClient client, InteractiveObject interactive) =>
            client.Send(new InteractiveElementUpdatedMessage(interactive.GetInteractiveElement(client.Account!.Character!)));

        public static void SendStatedElementUpdatedMessage(WorldClient client, InteractiveObject interactive) =>
            client.Send(new StatedElementUpdatedMessage(interactive.StatedElement));
    }
}
