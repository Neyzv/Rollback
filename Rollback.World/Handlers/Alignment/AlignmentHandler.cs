using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;
using Rollback.World.Network;

namespace Rollback.World.Handlers.Alignment
{
    public static class AlignmentHandler
    {
        public static void SendAlignmentRankUpdateMessage(WorldClient client) =>
            client.Send(new AlignmentRankUpdateMessage(client.Account!.Character!.AlignmentSide switch
            {
                AlignmentSideEnum.ALIGNMENT_ANGEL => 1,
                AlignmentSideEnum.ALIGNMENT_EVIL => 18,
                _ => 0
            }, false));

        public static void SendAlignmentSubAreasListMessage(WorldClient client) =>
            client.Send(new AlignmentSubAreasListMessage(Array.Empty<short>(), Array.Empty<short>())); //TO DO
    }
}
