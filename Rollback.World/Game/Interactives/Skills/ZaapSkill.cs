using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Interactives;
using Rollback.World.Game.Interactions.Dialogs.Interactives;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Interactives.Skills
{
    [Identifier(114)]
    internal class ZaapSkill : Skill
    {
        public ZaapSkill(InteractiveObject interactive, InteractiveSkillRecord record)
            : base(interactive, record) { }

        public override void Execute(Character character) =>
            new ZaapDialog(character, character.KnownZaaps
                                        .Where(x => x.Value.SubArea?.Area.SuperArea.Id == character.MapInstance.Map.SubArea?.Area.SuperArea.Id)
                                        .ToDictionary(x => x.Key, x => x.Value)).Open();
    }
}
