using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Types;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests.Objectives
{
    [Identifier(QuestObjectiveType.DefeatPlayersInDuel)]
    public sealed class DefeatPlayersInDuelQuestObjective : QuestObjective
    {
        public DefeatPlayersInDuelQuestObjective(Quest quest, QuestObjectiveRecord record, Character owner, int progression) : base(quest, record, owner, progression) { }

        private void OnFightEnded(IFight fight)
        {
            if (fight is FightDuel fightDuel && fightDuel.Winners.FirstOrDefault(x => x.Id == _owner.Id) is not null)
                Complete();
        }

        protected override void EnableObjective() =>
            _owner.FightEnded += OnFightEnded;

        protected override void DisableObjective() =>
            _owner.FightEnded -= OnFightEnded;
    }
}
