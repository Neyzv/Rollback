using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Fights.Types;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests.Objectives
{
    [Identifier(QuestObjectiveType.DefeatMonstersInOneFight)]
    public class DefeatMonstersInOneFightQuestObjective : QuestObjective
    {
        private short? _monsterId;
        public short MonsterId =>
            _monsterId ??= GetParameterValue<short>(0);

        private int? _quantity;
        public int Quantity =>
            _quantity ??= GetParameterValue<int>(1);

        public DefeatMonstersInOneFightQuestObjective(Quest quest, QuestObjectiveRecord record, Character owner, int progression)
            : base(quest, record, owner, progression) { }

        protected override void EnableObjective() =>
            _owner.FightEnded += OnFightEnded;

        protected override void DisableObjective() =>
            _owner.FightEnded -= OnFightEnded;

        private void OnFightEnded(IFight fight)
        {
            if (fight is FightPvM pvmFight && pvmFight.Winners.Any(x => x.Id == _owner.Id))
            {
                var objectiveMonsterKilled = pvmFight.Losers.Where(x => x is MonsterFighter monsterFighter && monsterFighter.MonsterId == MonsterId).Count();

                if (objectiveMonsterKilled >= Quantity)
                    Complete();
            }
        }
    }
}
