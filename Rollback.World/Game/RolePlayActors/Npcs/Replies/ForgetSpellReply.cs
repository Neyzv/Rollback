using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.Interactions.Dialogs.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("ForgetSpell")]
    public sealed class ForgetSpellReply : NpcReply
    {
        public ForgetSpellReply(NpcReplyRecord record) : base(record) { }

        public override bool Execute(Npc npc, Character character)
        {
            new NpcForgetSpellDialog(character, npc).Open();

            return true;
        }
    }
}
