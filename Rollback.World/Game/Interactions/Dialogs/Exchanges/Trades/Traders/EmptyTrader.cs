using System.Diagnostics.CodeAnalysis;
using Rollback.World.Game.Items.Types;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Traders
{
    public sealed class EmptyTrader : Trader
    {
        public override int Id =>
            -1;

        public override bool Ready =>
            true;

        protected override PlayerItem? RetrieveFromStorageByUID(int uid) =>
            default;

        public override bool ChangeKamas(int amount) =>
            false;

        public override bool MoveItem(int uid, int quantity, [NotNullWhen(true)] out TradeItem? tradeItem, out bool modified)
        {
            tradeItem = default;
            modified = false;

            return false;
        }
    }
}
