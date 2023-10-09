using Rollback.Auth.Managers;
using Rollback.Common.ORM;
using Rollback.Protocol.Enums;

namespace Rollback.Auth.Database
{
    public static class AccountRelator
    {
        public const string FindAccountById = "SELECT * FROM accounts WHERE Id = {0} LIMIT 1";
        public const string FindAccountByLogin = "SELECT * FROM accounts WHERE Login = BINARY '{0}' LIMIT 1";
        public const string FindAccountByNickname = "SELECT * FROM accounts WHERE Nickname = '{0}' LIMIT 1";
        public const string FindAccountByTicket = "SELECT * FROM accounts WHERE Ticket = '{0}' LIMIT 1";
    }

    [Table("accounts")]
    public sealed record AccountRecord
    {
        public AccountRecord() =>
            (Login, Password, Nickname, SecretQuestion, SecretAnswer) = (string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

        [Key]
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Nickname { get; set; }

        public GameHierarchyEnum Role { get; set; }

        public string? Ticket { get; set; }

        public string SecretQuestion { get; set; }

        public string SecretAnswer { get; set; }

        public DateTime? BannedDate { get; set; }

        public DateTime? LastConnection { get; set; }

        public string? LastIP { get; set; }

        [Ignore]
        public bool IsBanned =>
            BannedDate.HasValue && BannedDate > DateTime.Now;

        [Ignore]
        public bool IsAdmin =>
            Role > GameHierarchyEnum.PLAYER;

        private Dictionary<int, List<int>>? _charactersByWorld;
        [Ignore]
        public Dictionary<int, List<int>>? CharactersByWorld
        {
            get => _charactersByWorld ??= GetCharactersByWorld();
            set => _charactersByWorld = value;
        }

        private List<AccountGiftRecord>? _gifts;
        [Ignore]
        public List<AccountGiftRecord> Gifts =>
            _gifts ??= DatabaseAccessor.Instance.Select<AccountGiftRecord>(string.Format(AccountGiftRelator.GetAccountGiftsByAccountId, Id));

        public Dictionary<int, List<int>> GetCharactersByWorld()
        {
            Dictionary<int, List<int>> result = new();
            foreach (var worldCharacter in AccountManager.GetWorldsCharactersByAccountId(Id))
            {
                if (result.ContainsKey(worldCharacter.WorldId))
                    result[worldCharacter.WorldId].Add(worldCharacter.CharacterId);
                else
                    result[worldCharacter.WorldId] = new() { worldCharacter.CharacterId };
            }

            return result;
        }
    }
}
