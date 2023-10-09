using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Interactives;
using Rollback.World.Game.Interactions.Dialogs.Interactives;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Interactives.Skills
{
    [Identifier(186)]
    public sealed class GuildCreationSkill : Skill
    {
        public GuildCreationSkill(InteractiveObject interactive, InteractiveSkillRecord record)
            : base(interactive, record) { }

        public override void Execute(Character character) =>
            new GuildCreationDialog(character).Open();
    }
}
