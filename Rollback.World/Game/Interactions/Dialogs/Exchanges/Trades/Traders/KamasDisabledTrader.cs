using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Traders
{
    public sealed class KamasDisabledTrader : PlayerTrader
    {
        public KamasDisabledTrader(Character character)
            : base(character) { }

        public override bool ChangeKamas(int amount) =>
            false;
    }
}
