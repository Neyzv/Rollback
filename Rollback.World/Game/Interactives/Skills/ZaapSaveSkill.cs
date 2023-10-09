using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Interactives;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Interactives.Skills
{
    [Identifier(44)]
    public sealed class ZaapSaveSkill : Skill
    {
        public ZaapSaveSkill(InteractiveObject interactive, InteractiveSkillRecord record)
            : base(interactive, record) { }

        public override void Execute(Character character) =>
            character.SetSpawnPoint(character.MapInstance.Map.Record.Id);
    }
}
