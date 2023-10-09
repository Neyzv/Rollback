using Rollback.Common.DesignPattern.Instance;
using Rollback.World.Database.World;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.World.Maps.Triggers
{
    public abstract class CellTrigger : ParameterContainer
    {
        protected readonly WorldCellsTriggersRecord _record;

        public short CellId =>
            _record.CellId;

        public int Priority =>
            _record.Priority;

        public CellTriggerType Type =>
            _record.Type;

        public CellTrigger(WorldCellsTriggersRecord record) : base(record.ParametersCSV.Split(';')) =>
            _record = record;

        public bool CanExecute(Character character) =>
            _record.Criterion is null || _record.Criterion.Eval(character);

        public abstract void Trigger(Character character);
    }
}
