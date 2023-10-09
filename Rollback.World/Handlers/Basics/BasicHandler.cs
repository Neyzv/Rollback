using Rollback.Common.Extensions;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Basics
{
    public static class BasicHandler
    {
        [WorldHandler(BasicWhoIsRequestMessage.Id)]
        public static void HandleBasicWhoIsRequestMessage(WorldClient client, BasicWhoIsRequestMessage message)
        {
            var target = WorldServer.Instance.GetClient(x => x.Account?.Character?.Name == message.search)?.Account!.Character;
            if (target is not null)
                SendBasicWhoIsMessage(client, target);
            else
                SendBasicWhoIsNoMatchMessage(client, message.search);
        }

        [WorldHandler(BasicPingMessage.Id)]
        public static void HandleBasicPingMessage(WorldClient client, BasicPingMessage message) =>
            SendBasicPongMessage(client, message.quiet);

        public static void SendBasicTimeMessage(WorldClient client) =>
            client.Send(new BasicTimeMessage(DateTime.Now.GetUnixTimeStamp(), 3600));

        public static void SendBasicNoOperationMessage(WorldClient client) =>
            client.Send(new BasicNoOperationMessage());

        public static void SendTextInformationMessage(WorldClient client, TextInformationTypeEnum msgType, short msgId, params string[] parameters) =>
            client.Send(new TextInformationMessage((sbyte)msgType, msgId, parameters));

        public static void SendDebugHighlightCellsMessage(WorldClient client, int color, short[] cells) =>
            client.Send(new DebugHighlightCellsMessage(color, cells));

        public static void SendDebugClearHighlightCellsMessage(WorldClient client) =>
            client.Send(new DebugClearHighlightCellsMessage());

        public static void SendBasicWhoIsNoMatchMessage(WorldClient client, string name) =>
            client.Send(new BasicWhoIsNoMatchMessage(name));

        public static void SendBasicWhoIsMessage(WorldClient client, Character character) =>
            client.Send(new BasicWhoIsMessage(character.Id == client.Account!.Character!.Id, 0, character.Client.Account!.Nickname, character.Name, character.MapInstance?.Map.SubArea?.Area.Id is null ? (short)-1 : character.MapInstance.Map.SubArea.Area.Id));

        public static void SendBasicPongMessage(WorldClient client, bool quiet) =>
            client.Send(new BasicPongMessage(quiet));
    }
}
