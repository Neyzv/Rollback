using Rollback.Auth.Database;
using Rollback.Common.Network.IPC.Handler;
using Rollback.Common.Network.IPC.Messages.Gifts;
using Rollback.Common.ORM;

namespace Rollback.Auth.Network.IPC.Handlers.Gifts
{
    public static class IPCGiftHandler
    {
        [IPCHandler(GiftReceivedMessage.Id)]
        public static void HandleGiftReceivedMessage(IPCReceiver receiver, GiftReceivedMessage message) =>
            DatabaseAccessor.Instance.ExecuteNonQuery(string.Format(AccountGiftRelator.DeleteAccountGiftById, message.GiftId));
    }
}
