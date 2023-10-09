using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Interactives;
using Rollback.World.Game.Interactions.Dialogs.Interactives;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Interactives.Skills
{
    [Identifier(157)]
    public sealed class ZaapiSkill : Skill
    {
        public ZaapiSkill(InteractiveObject interactive, InteractiveSkillRecord record)
            : base(interactive, record) { }

        public override void Execute(Character character)
        {
            var destinations = new Dictionary<int, Map>();
            if (character.MapInstance.Map.SubArea is not null)
            {
                foreach (var map in from map in WorldManager.Instance.GetMaps(x => x.SubArea?.Area.Id == character.MapInstance.Map.SubArea.Area.Id)
                                    from interactive in map.GetMainInstance().GetInteractives(x => x.Skills.Values.Any(x => x is ZaapiSkill))
                                    select map)
                    destinations[map.Record.Id] = map;
            }

            new ZaapiDialog(character, destinations).Open();
        }
    }
}
