using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Quests.Objectives.Events.Types
{
    [Identifier("Wait")]
    public sealed class WaitQuestObjectiveEvent : QuestObjectiveEvent
    {
        private short? _itemId;
        public short ItemId =>
            _itemId ??= GetParameterValue<short>(0);

        private TimeSpan? _time;
        public TimeSpan Time =>
            _time ??= TimeSpan.FromSeconds(GetParameterValue<long>(1));

        public WaitQuestObjectiveEvent(Character owner, QuestObjective objective, QuestObjectiveEventRecord record)
            : base(owner, objective, record) { }

        public override void Trigger() =>
            _owner.ChangeMap += OnChangeMap;

        public override void UnTrigger() =>
            _owner.ChangeMap -= OnChangeMap;

        private void OnChangeMap(Character owner, MapInstance mapInstance)
        {
            var items = _owner.Inventory.GetItems(x => x.Id == ItemId);
            var now = DateTime.Now;

            if (items.Length is not 0 && items.Any(x => x.Effects.OfType<EffectDate>().Any(effect => effect.Date + Time <= now)))
                _objective.Complete();
        }
    }
}
