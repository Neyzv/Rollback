using Rollback.Common.Network;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;
using Rollback.World.Handlers.Maps;
using Rollback.World.Network;

namespace Rollback.World.Extensions
{
    public static class MapObstacleExtensions
    {
        public static void Refresh(this IEnumerable<MapObstacle> obstacles, ClientCollection<WorldClient, Message> clients) =>
            clients.Send(MapHandler.SendMapObstacleUpdateMessage, new[] { obstacles.ToArray() });
    }
}
