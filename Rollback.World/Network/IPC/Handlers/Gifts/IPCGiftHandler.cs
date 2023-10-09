using Rollback.Common.Network.IPC.Messages.Gifts;

namespace Rollback.World.Network.IPC.Handlers.Gifts
{
    public static class IPCGiftHandler
    {
        public static void SendGiftReceivedMessage(int giftId) =>
            IPCService.Instance.Send(new GiftReceivedMessage(giftId));
    }
}
