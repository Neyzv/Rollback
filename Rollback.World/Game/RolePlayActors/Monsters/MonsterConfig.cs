using Rollback.Common.Config;
using Rollback.Common.DesignPattern.Instance;

namespace Rollback.World.Game.RolePlayActors.Monsters
{
    [Config("Monsters")]
    public sealed class MonsterConfig : Singleton<MonsterConfig>
    {
        public byte MaxMonsterGroupsByMap { get; set; } = 3;

        public short StarBonusTime { get; set; } = 6_000;

        public int MinSpawnInterval { get; set; } = 15_000;

        public int MaxSpawnInterval { get; set; } = 30_000;

        public int MinDungeonSpawnInterval { get; set; } = 1_000;

        public int MaxDungeonSpawnInterval { get; set; } = 5_000;

        public int MonsterRareSpawnCheckInterval { get; set; } = 30_000;
    }
}
