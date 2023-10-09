using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("LightRestat")]
    public sealed class LightRestatReply : NpcReply
    {
        public LightRestatReply(NpcReplyRecord record) : base(record) { }

        public override bool Execute(Npc npc, Character character)
        {
            character.ResetStats();

            return true;
        }
    }
}
