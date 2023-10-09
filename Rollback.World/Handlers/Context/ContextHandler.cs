using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Context
{
    public static class ContextHandler
    {
        [WorldHandler(GameContextCreateRequestMessage.Id)]
        public static void HandleGameContextCreateRequestMessage(WorldClient client, GameContextCreateRequestMessage message)
        {
            if (client.Account!.Character!.Fighter is null)
            {
                SendGameContextDestroyMessage(client);
                SendGameContextCreateMessage(client, GameContextEnum.ROLE_PLAY);

                client.Account.Character.RefreshStats();
                client.Account.Character.Teleport(client.Account.Character.MapInstance, client.Account.Character.Cell.Id,
                    client.Account.Character.Direction, true);
            }
        }

        [WorldHandler(GameContextReadyMessage.Id)]
        public static void HandleGameContextReadyMessage(WorldClient client, GameContextReadyMessage message) { }

        public static void SendGameContextDestroyMessage(WorldClient client) =>
            client.Send(new GameContextDestroyMessage());

        public static void SendGameContextCreateMessage(WorldClient client, GameContextEnum context) =>
            client.Send(new GameContextCreateMessage((sbyte)context));

        public static void SendCurrentMapMessage(WorldClient client) =>
            client.Send(new CurrentMapMessage(client.Account!.Character!.MapInstance.Map.Record.Id));

        public static void SendTeleportOnSameMapMessage(WorldClient client, Character character) =>
            client.Send(new TeleportOnSameMapMessage(character.Id, character.Cell.Id));
    }
}
