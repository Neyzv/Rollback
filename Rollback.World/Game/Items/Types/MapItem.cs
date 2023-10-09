using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Items.Types
{
    public sealed class MapItem
    {
        public PlayerItem Item { get; }

        public Cell Cell { get; }

        public MapItem(PlayerItem item, Cell cell)
        {
            item.LastInteractionDate = DateTime.Now;
            Item = item;
            Cell = cell;
        }
    }
}
