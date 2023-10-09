using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Interactives;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Mounts;
using Rollback.World.Game.Mounts;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Interactives.Skills
{
    [Identifier(175)]
    public sealed class PaddockSkill : Skill
    {
        public override int Duration =>
            1000;

        public PaddockSkill(InteractiveObject interactive, InteractiveSkillRecord record)
            : base(interactive, record) { }

        public override bool CanBeUsed(Character character) =>
            base.CanBeUsed(character) && MountManager.Instance.GetPaddock(_interactive.MapInstance.Map.Record.Id) is not null;

        public override void Execute(Character character) =>
            new PaddockExchange(character, MountManager.Instance.GetPaddock(_interactive.MapInstance.Map.Record.Id)!).Open();
    }
}
