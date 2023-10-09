using Rollback.Common.ORM;
using Rollback.World.CustomEnums;

namespace Rollback.World.Database.Accounts.Friends
{
    public static class AccountRelationRelator
    {
        public const string GetAccountRelationByAccountId = "SELECT * FROM accounts_relations WHERE AccountId = {0}";
    }

    [Table("accounts_relations")]
    public sealed record AccountRelationRecord
    {
        [Key]
        public int AccountId { get; set; }

        private int _targetId;
        [Key]
        public int TargetId
        {
            get => _targetId;
            set
            {
                _targetId = value;
                AccountInformationsRecord = DatabaseAccessor.Instance.SelectSingle<AccountInformationsRecord>(
                    string.Format(AccountInformationsRelator.GetAccountInformationsById, _targetId));
            }
        }

        [Ignore]
        public AccountInformationsRecord? AccountInformationsRecord { get; private set; }

        [Key]
        public AccountRelationType RelationType { get; set; }
    }
}
