using Rollback.World.Game.Items;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.Items.Types;
using Rollback.World.Game.RolePlayActors.Npcs;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Traders
{
    public sealed class NpcTrader : Trader
    {
        private readonly Npc _npc;
        private readonly Dictionary<int, PlayerItem> _playerItems;

        public override int Id =>
            _npc.Id;

        public override bool Ready =>
            true;

        public NpcTrader(Npc npc)
        {
            _npc = npc;
            _playerItems = new Dictionary<int, PlayerItem>();

            ClearItems += ClearItemStorage;
        }

        private void ClearItemStorage()
        {
            foreach (var item in _playerItems.Values)
                ItemManager.Instance.IdProvider.Free(item.UID);

            _playerItems.Clear();
        }

        protected override PlayerItem? RetrieveFromStorageByUID(int uid) =>
            _playerItems.ContainsKey(uid) ? _playerItems[uid] : default;

        public override bool ChangeKamas(int amount)
        {
            var result = false;

            if (amount >= 0 && amount <= Inventory.MaxKamasInInventory)
            {
                Kamas = amount;
                result = true;
            }

            return result;
        }

        public void AddItem(PlayerItem item) =>
            _playerItems[item.UID] = item;
    }
}
