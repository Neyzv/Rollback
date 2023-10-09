using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Fights.Types;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests.Objectives
{
    [Identifier(QuestObjectiveType.DefeatMonsters)]
    public sealed class DefeatMonstersQuestObjective : DefeatMonstersInOneFightQuestObjective
    {
        protected override int ProgressionReference =>
            Quantity;

        public DefeatMonstersQuestObjective(Quest quest, QuestObjectiveRecord record, Character owner, int progression)
            : base(quest, record, owner, progression) { }

        protected override void EnableObjective() =>
            _owner.FightEnded += OnFightEnded;

        protected override void DisableObjective() =>
            _owner.FightEnded -= OnFightEnded;

        private void OnFightEnded(IFight fight)
        {
            if (fight is FightPvM pvmFight && pvmFight.Winners.FirstOrDefault(x => x.Id == _owner.Id) is not null)
            {
                Progression += pvmFight.Losers.Where(x => x is MonsterFighter monsterFighter && monsterFighter.MonsterId == MonsterId).Count();

                if (!IsInProgress)
                    Complete();
            }
        }
    }
}
