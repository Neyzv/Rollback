using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Interactives;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.Characters.Breeds;

namespace Rollback.World.Game.Interactives.Skills
{
    [Identifier(183)]
    public sealed class BreedStatueSkill : Skill
    {
        public override int Duration =>
            1000;

        public BreedStatueSkill(InteractiveObject interactive, InteractiveSkillRecord record)
            : base(interactive, record) { }

        public override void Execute(Character character)
        {
            var breed = BreedManager.Instance.GetBreedById((int)character.Breed);

            if (breed is not null)
                character.Teleport(breed.StartMapId, breed.StartCellId, breed.StartDirection);
        }
    }
}
