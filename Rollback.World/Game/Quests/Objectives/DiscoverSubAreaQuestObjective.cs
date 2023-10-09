using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Quests;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Quests.Objectives
{
    [Identifier(QuestObjectiveType.DiscoverArea)]
    public sealed class DiscoverSubAreaQuestObjective : QuestObjective
    {
        private short? _subAreaId;
        public short SubAreaId =>
            _subAreaId ??= GetParameterValue<short>(0);

        public DiscoverSubAreaQuestObjective(Quest quest, QuestObjectiveRecord record, Character owner, int progression) : base(quest, record, owner, progression) { }

        private void OnChangeMap(Character owner, MapInstance map)
        {
            if (map.Map.SubArea?.Id == SubAreaId)
                Complete();
        }

        protected override void EnableObjective() =>
            _owner.ChangeMap += OnChangeMap;

        protected override void DisableObjective() =>
            _owner.ChangeMap -= OnChangeMap;
    }
}
