using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("CloseDialog")]
    public sealed class CloseDialogReply : NpcReply
    {
        public CloseDialogReply(NpcReplyRecord record) : base(record) { }

        public override bool Execute(Npc npc, Character character) =>
            true;
    }
}
