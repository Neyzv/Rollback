namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades
{
    public interface ITrade : IExchange
    {
        void ChangeReadyState(int actorId, bool state);
    }
}
