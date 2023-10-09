using Rollback.Protocol.Enums;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges
{
    public interface IExchange : IInteraction
    {
        ExchangeTypeEnum ExchangeType { get; }

        void SetKamas(int actorId, int amount);

        void MoveItem(int actorId, int uid, int quantity);
    }
}
