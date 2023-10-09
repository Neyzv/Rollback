using Rollback.Common.ORM;
using Rollback.World.Game.Items;

namespace Rollback.World.Database.Accounts.Bank
{
    public static class AccountBankItemRelator
    {
        public const string GetItemsByAccountId = "SELECT * FROM accounts_bank_items WHERE OwnerId = {0}";
    }

    [Table("accounts_bank_items")]
    public sealed record AccountBankItemRecord : ItemBaseRecord
    {
        public AccountBankItemRecord() : base() { }
    }
}
