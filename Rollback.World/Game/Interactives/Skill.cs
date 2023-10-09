using Rollback.Common.DesignPattern.Instance;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Interactives;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Interactives
{
    public abstract class Skill : ParameterContainer
    {
        protected InteractiveSkillRecord _record;
        protected InteractiveObject _interactive;

        public short? TemplateId =>
            _record.Template?.Id;

        public JobIds? ParentJobId =>
            _record.Template?.ParentJobId;

        public sbyte? MinJobLevel =>
            _record.Template?.MinJobLevel;

        public virtual int Duration =>
            0;

        public Skill(InteractiveObject interactive, InteractiveSkillRecord record)
            : base(record.ParametersCSV.Split(';'))
        {
            _interactive = interactive;
            _record = record;
        }

        public virtual bool BeforeExecute(Character character) =>
            true;

        public virtual bool CanBeUsed(Character character) =>
            true;

        public bool CanExecute(Character character) =>
            _record.Criterion is null || _record.Criterion.Eval(character);

        public abstract void Execute(Character character);
    }
}
