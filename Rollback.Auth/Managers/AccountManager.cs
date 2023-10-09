using Rollback.Auth.Database;
using Rollback.Common.Extensions;
using Rollback.Common.ORM;

namespace Rollback.Auth.Managers
{
    internal static class AccountManager
    {
        private static readonly string[] _invalidNicknames = new string[]
        {
            "admin",
            "moderateur",
            "fondateur",
            "animateur",
            "staff",
            "neyzu",
        };

        internal static AccountRecord? GetAccountById(int id) =>
            DatabaseAccessor.Instance.SelectSingle<AccountRecord>(string.Format(AccountRelator.FindAccountById, id));

        internal static AccountRecord? GetAccountByLogin(string login) =>
            DatabaseAccessor.Instance.SelectSingle<AccountRecord>(string.Format(AccountRelator.FindAccountByLogin, login));

        internal static AccountRecord? GetAccountByNickname(string nickname) =>
            DatabaseAccessor.Instance.SelectSingle<AccountRecord>(string.Format(AccountRelator.FindAccountByNickname, nickname));

        internal static AccountRecord? GetAccountByTicket(string ticket) =>
            DatabaseAccessor.Instance.SelectSingle<AccountRecord>(string.Format(AccountRelator.FindAccountByTicket, ticket));

        internal static List<WorldCharacterRecord> GetWorldsCharactersByAccountId(int accountId) =>
            DatabaseAccessor.Instance.Select<WorldCharacterRecord>(string.Format(WorldCharacterRelator.GetWorldsCharactersByAccountId, accountId));

        internal static bool IsNicknameCorrect(string nickname) =>
            nickname.Length > 2 && !nickname.ContainsSpecialChars() && _invalidNicknames.All(x => !nickname.ToLower().Contains(x.ToLower()));
    }
}
