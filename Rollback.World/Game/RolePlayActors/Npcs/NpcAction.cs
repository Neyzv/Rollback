using Rollback.Common.DesignPattern.Instance;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs
{
    public abstract class NpcAction : ParameterContainer
    {
        private readonly NpcActionRecord _record;

        public int Priority =>
            _record.Priority;

        public Dictionary<short, NpcItemRecord> Items =>
            _record.Items;

        public abstract NpcActionType NpcActionType { get; }

        public NpcAction(NpcActionRecord record) : base(record.ParametersCSV.Split(";")) =>
            _record = record;

        public bool CanExecute(Character character) =>
            _record.Criterion is null || _record.Criterion.Eval(character);

        public abstract void Execute(Npc npc, Character character);
    }
}
