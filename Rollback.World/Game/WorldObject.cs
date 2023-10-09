using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game
{
    public abstract class WorldObject
    {
        public const short StarBonusLimit = 200;

        public MapInstance MapInstance { get; protected set; }

        public Cell Cell { get; protected set; }

        protected WorldObject(MapInstance mapInstance, Cell cell)
        {
            MapInstance = mapInstance;
            Cell = cell;
        }
    }
}
