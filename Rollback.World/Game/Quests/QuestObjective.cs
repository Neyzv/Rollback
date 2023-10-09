using Rollback.Common.DesignPattern.Instance;
using Rollback.World.Database.Quests;
using Rollback.World.Game.Quests.Objectives.Events;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests
{
    public abstract class QuestObjective : ParameterContainer
    {
        private readonly QuestObjectiveRecord _record;
        private readonly Dictionary<QuestObjectiveEventTriggerType, List<QuestObjectiveEvent>> _events;

        protected readonly Character _owner;
        protected readonly Quest _quest;

        protected virtual int ProgressionReference =>
            1;

        public short Id =>
            _record.Id;

        public bool IsInProgress =>
            Progression < ProgressionReference;

        private int _progression;
        public int Progression
        {
            get => _progression;
            set
            {
                _progression = value > ProgressionReference ? ProgressionReference : value;

                TriggerEvents(QuestObjectiveEventTriggerType.Progress);
            }
        }

        public QuestObjective(Quest quest, QuestObjectiveRecord record, Character owner, int progression)
            : base(record.ParametersCSV.Split(','))
        {
            _record = record;

            _quest = quest;
            _owner = owner;

            _progression = progression;

            if (IsInProgress)
            {
                _events = QuestManager.Instance.GetObjectiveEvents(_owner, this);
                EnableObjective();

                TriggerEvents(QuestObjectiveEventTriggerType.Start);
            }
            else
                _events = new();
        }

        public event Action<QuestObjective>? Completed;

        private void TriggerEvents(QuestObjectiveEventTriggerType triggerType)
        {
            if (_events.ContainsKey(triggerType))
                foreach (var objectiveEvent in _events[triggerType])
                    if (objectiveEvent.CanTrigger())
                        objectiveEvent.Trigger();
        }

        public void Complete()
        {
            Progression = ProgressionReference;
            DisableObjective();

            Completed?.Invoke(this);

            TriggerEvents(QuestObjectiveEventTriggerType.Complete);

            foreach (var objectiveEvent in _events.Values.SelectMany(x => x))
                objectiveEvent.UnTrigger();
        }

        protected abstract void EnableObjective();

        protected abstract void DisableObjective();
    }
}
