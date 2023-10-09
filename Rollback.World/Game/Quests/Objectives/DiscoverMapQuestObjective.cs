using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Quests;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Quests.Objectives
{
    [Identifier(QuestObjectiveType.DiscoverMap)]
    public sealed class DiscoverMapQuestObjective : QuestObjective
    {
        private int? _mapId;
        public int MapId =>
            _mapId ??= GetParameterValue<int>(0);

        public DiscoverMapQuestObjective(Quest quest, QuestObjectiveRecord record, Character owner, int progression) : base(quest, record, owner, progression) { }

        private void OnChangeMap(Character character, MapInstance map)
        {
            if (map.Map.Record.Id == MapId)
                Complete();
        }

        protected override void EnableObjective() =>
            _owner.ChangeMap += OnChangeMap;

        protected override void DisableObjective() =>
            _owner.ChangeMap -= OnChangeMap;
    }
}
