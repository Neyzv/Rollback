using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Looks;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.RolePlayActors.Monsters
{
    public sealed class MonsterGroup : AutoMoveActor
    {
        private Monster Leader =>
            Monsters.First();

        public override ActorLook Look =>
            Leader.Look;

        public MonsterGroupSize Size { get; private set; }

        public IReadOnlyCollection<Monster> Monsters { get; }

        public DateTime CreationDate { get; private set; }

        public short AgeBonus
        {
            get
            {
                // 10 of bonus = 1 star
                var bonus = (DateTime.Now - CreationDate).TotalMilliseconds / (MonsterConfig.Instance.StarBonusTime);
                return (short)(bonus <= WorldObject.StarBonusLimit ? bonus : WorldObject.StarBonusLimit);
            }
            set
            {
                CreationDate = DateTime.Now - TimeSpan.FromMilliseconds(value * MonsterConfig.Instance.StarBonusTime);
            }
        }

        public int? DungeonId { get; }

        public override GameRolePlayActorInformations GameRolePlayActorInformations(Character character) =>
            new GameRolePlayGroupMonsterInformations(Id, Look.GetEntityLook(), EntityDispositionInformations,
                Leader.Id, Leader.Level, Monsters.Skip(1).Select(x => x.MonsterInGroupInformations).ToArray(), AgeBonus);

        public MonsterGroup(int id, MapInstance mapInstance, Cell cell, DirectionsEnum direction, List<Monster> monsters,
            int? dungeonId)
            : base(id, mapInstance, cell, direction, monsters.First().Look)
        {
            Monsters = monsters.OrderByDescending(x => x.Level).ToList();

            CreationDate = DateTime.Now;
            Size = MonsterManager.GetGroupSize(Monsters.Count);
            DungeonId = dungeonId;
        }

        #region Events
        public event Action<MonsterGroup, IFight>? EnterFight;
        public void OnEnterFight(IFight fight) =>
            EnterFight?.Invoke(this, fight);
        #endregion
    }
}
