using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps.Triggers.Actions;
using Rollback.World.Handlers.Npcs;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("Marry")]
    public sealed class MarryReply : NpcReply
    {
        public MarryReply(NpcReplyRecord record) : base(record) { }

        public override bool Execute(Npc npc, Character character)
        {
            if (character.MapInstance.GetCellTrigger<MarryTriggerAction>(x => x.CellId != character.Cell.Id) is { } cellTrigger &&
                character.MapInstance.GetCellTrigger<MarryTriggerAction>(x => x.CellId == character.Cell.Id) is not null)
            {
                var actors = character.MapInstance.GetActors<Character>(x => x.Cell.Id == cellTrigger.CellId);
                if (actors.Length is 1)
                {
                    actors.First().ChangeDirection(actors.First().Cell.Point.OrientationTo(character.Cell.Point));
                    character.ChangeDirection(character.Cell.Point.OrientationTo(actors.First().Cell.Point));

                    character.Marry(actors.First());

                    character.Teleport(character.MapInstance, character.MapInstance.GetRandomAdjacentFreeCell(character.Cell.Point, true)!.Id);
                    actors.First().Teleport(actors.First().MapInstance, actors.First().MapInstance.GetRandomAdjacentFreeCell(actors.First().Cell.Point, true)!.Id);

                    character.MapInstance.Send(NpcHandler.SendEntityTalkMessage, new object[] { npc, (short)1, new[] { character.Name, actors.First().Name } });
                }
            }

            return true;
        }
    }
}
