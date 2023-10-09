namespace Rollback.World.CustomEnums
{
    public enum SpellTargetType
    {
        None = 0,
        Self = 1,

        AllyMonsterSummon = 2,
        AllySummon = 4,
        AllyNonMonsterSummon = 8,
        AllyStaticSummon = 16,
        AllyMonster = 32,
        AllySummoner = 64,
        AllyPlayer = 128,

        EnemyMonsterSummon = 256,
        EnemySummon = 512,
        EnemyNonMonsterSummon = 1024,
        EnnemyStaticSummon = 2048,
        EnemyMonster = 4096,
        EnemyUnkn_1 = 8192,
        EnemyPlayer = 16384,

        SelfOnly = 32768,

        // Customs
        AllyExceptSummons = 6,
        AllySummonsAndEnnemies = 1800,
        EnemyCorruption = 3840,
    }
}
