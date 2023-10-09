using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Network;

namespace Rollback.World.Handlers.Compass
{
    public static class CompassHandler
    {
        public static void SendCompassUpdatePartyMemberMessage(WorldClient client, Character target) =>
            client.Send(new CompassUpdatePartyMemberMessage((sbyte)CompassTypeEnum.COMPASS_TYPE_PARTY,
                target.MapInstance.Map.Record.X,
                target.MapInstance.Map.Record.Y,
                target.Id));

        public static void SendCompassUpdateSpouseMessage(WorldClient client) =>
            client.Send(new CompassUpdatePartyMemberMessage((sbyte)CompassTypeEnum.COMPASS_TYPE_SPOUSE,
                client.Account!.Character!.Spouse!.Character!.MapInstance.Map.Record.X,
                client.Account!.Character!.Spouse!.Character!.MapInstance.Map.Record.Y,
                client.Account!.Character!.Spouse!.Character!.Id));

        public static void SendCompassUpdateMessage(WorldClient client, sbyte x, sbyte y) =>
            client.Send(new CompassUpdateMessage((sbyte)CompassTypeEnum.COMPASS_TYPE_SIMPLE, x, y));

        public static void SendCompassResetMessage(WorldClient client) =>
            client.Send(new CompassResetMessage());
    }
}
