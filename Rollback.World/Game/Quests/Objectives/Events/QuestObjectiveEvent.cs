using Rollback.Common.DesignPattern.Instance;
using Rollback.World.Database.Quests;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Quests.Objectives.Events
{
    public abstract class QuestObjectiveEvent : ParameterContainer
    {
        protected readonly Character _owner;
        protected readonly QuestObjective _objective;
        protected readonly QuestObjectiveEventRecord _record;

        public QuestObjectiveEvent(Character owner, QuestObjective objective, QuestObjectiveEventRecord record)
            : base(record.Parameters.Split(';'))
        {
            _owner = owner;
            _objective = objective;
            _record = record;
        }

        public bool CanTrigger() =>
            _record.Criterion is null || _record.Criterion.Eval(_owner);

        public abstract void Trigger();

        public virtual void UnTrigger()
        {

        }
    }
}
