using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges
{
    public abstract class Exchange<T> : Dialog<T>, IExchange
        where T : class
    {
        public override DialogType DialogType =>
            DialogType.Exchange;

        public abstract ExchangeTypeEnum ExchangeType { get; }

        public Exchange(Character character, T dialoger) : base(character, dialoger) { }

        public abstract void SetKamas(int actorId, int amount);

        public abstract void MoveItem(int actorId, int uid, int quantity);
    }
}
