using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.World;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.World.Maps.Triggers.Actions
{
    [Identifier("Marry")]
    public sealed class MarryTriggerAction : CellTrigger
    {
        public MarryTriggerAction(WorldCellsTriggersRecord record) : base(record) { }

        public override void Trigger(Character character)
        {
            if (character.MapInstance.GetActor<Character>(x => x.Cell.Id == CellId && x.Id != character.Id) is not null || character.Spouse is not null)
                character.Teleport(character.MapInstance, character.MapInstance.GetRandomAdjacentFreeCell(character.Cell.Point, true)!.Id);
        }
    }
}
