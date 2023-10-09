using Rollback.Auth.Managers;
using Rollback.Common.Extensions;
using Rollback.Common.Logging;
using Rollback.Common.Network.IPC.Handler;
using Rollback.Common.Network.IPC.Messages.Accounts;
using Rollback.Common.ORM;

namespace Rollback.Auth.Network.IPC.Handlers.Accounts
{
    public static class IPCAccountHandler
    {
        [IPCHandler(AccountBannedMessage.Id)]
        public static void HandleAccountBannedMessage(IPCReceiver client, AccountBannedMessage message)
        {
            var account = AccountManager.GetAccountById(message.AccountId);
            if (account is not null)
            {
                account.BannedDate = message.BanEndTime.GetDateTimeFromUnixTimeStamp();
                DatabaseAccessor.Instance.Update(account);

                Logger.Instance.LogInfo($"Account {message.AccountId} banned successfully.");
            }
            else
                Logger.Instance.LogWarn($"Can not ban account {message.AccountId}, account not found.");
        }
    }
}
