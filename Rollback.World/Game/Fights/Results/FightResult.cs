using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.Game.Items.Storages;

namespace Rollback.World.Game.Fights.Results
{
    public abstract class FightResult<T> : IFightResult
        where T : ILooter
    {
        protected readonly Dictionary<short, short> _itemsToLoot;

        protected FightLoot Loot =>
            new(_itemsToLoot.Select(x => new[] { x.Key, x.Value }).SelectMany(x => x).ToArray(), Kamas);

        public abstract bool CanDrop { get; }

        public int Kamas { get; protected set; }

        public abstract short Wisdom { get; }

        public abstract short Prospecting { get; }

        public T Looter { get; }

        protected FightResult(T looter)
        {
            _itemsToLoot = new();
            Looter = looter;
        }

        public void AddEarnedKamas(int amount) =>
            Kamas += Kamas + amount > Inventory.MaxKamasInInventory ? Inventory.MaxKamasInInventory : Kamas + amount > 0 ? amount : 0;

        public virtual void AddEarnedItem(short itemId, short quantity)
        {
            if (quantity > 0)
            {
                if (_itemsToLoot.ContainsKey(itemId))
                    _itemsToLoot[itemId] += quantity;
                else
                    _itemsToLoot[itemId] = quantity;
            }
        }

        public virtual void Apply() { }

        public abstract FightResultFighterListEntry GetResult(FightOutcomeEnum outcome);
    }
}
