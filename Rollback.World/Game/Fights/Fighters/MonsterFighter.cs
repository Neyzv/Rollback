using Rollback.Protocol.Types;
using Rollback.World.Database.Monsters;
using Rollback.World.Game.RolePlayActors.Monsters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Fights.Fighters
{
    public sealed class MonsterFighter : AIFighter
    {
        private readonly Monster _monster;

        public override short Level =>
            _monster.Level;

        public long XP =>
            _monster.XP;

        public int MinKamas =>
            _monster.MinKamas;

        public int MaxKamas =>
            _monster.MaxKamas;

        public short MonsterId =>
            _monster.Id;

        public List<MonsterDropRecord> Drops =>
            _monster.Drops;

        public override FightTeamMemberInformations FightTeamMemberInformations =>
            new FightTeamMemberMonsterInformations(Id, _monster.Id, _monster.GradeId);

        public MonsterFighter(int contextualId, Monster monster, Cell cell)
            : base(contextualId, monster.Look.Clone(), monster.Stats, new(monster.Spells), cell) =>
            _monster = monster;

        public override GameFightFighterInformations GameFightFighterInformations(FightActor fighter) =>
            new GameFightMonsterInformations(Id, Look.GetEntityLook(), EntityDispositionInformations(fighter),
                (sbyte)Team!.Side, Alive, GameFightMinimalStats, _monster.Id, _monster.GradeId);
    }
}
