using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Interactives;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Interactives.Skills
{
    [Identifier(62)]
    public sealed class SourceOfYouthSkill : Skill
    {
        public SourceOfYouthSkill(InteractiveObject interactive, InteractiveSkillRecord record)
            : base(interactive, record) { }

        public override void Execute(Character character)
        {
            character.RegainLife(character.Stats.Health.ActualMax, false);
            character.ChangeEnergy(Character.MaxEnergy, false);
            character.RefreshStats();
        }
    }
}
