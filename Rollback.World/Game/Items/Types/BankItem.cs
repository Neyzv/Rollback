using Rollback.World.Database.Accounts.Bank;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Items.Types
{
    public sealed class BankItem : SaveableItem<BankItem, AccountBankItemRecord, Character, Bank>
    {
        public BankItem(AccountBankItemRecord record, Bank bank)
            : base(record, bank) { }
    }
}
