using Rollback.Common.ORM;

namespace Rollback.World.Database.Accounts
{
    public static class AccountInformationsRelator
    {
        public const string GetAccountInformationsById = "SELECT * FROM accounts_informations WHERE AccountId = {0}";
    }

    [Table("accounts_informations")]
    public sealed record AccountInformationsRecord
    {
        public AccountInformationsRecord() =>
            Nickname = string.Empty;

        [Key]
        public int AccountId { get; set; }

        public string Nickname { get; set; }

        public int BankKamas { get; set; }

        public DateTime LastServerConnection { get; set; }

        public bool WarnOnFriendsConnection { get; set; }

        public bool WarnOnFriendsLevelGain { get; set; }
    }
}
