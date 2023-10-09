using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Bank;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("Bank")]
    public sealed class BankReply : NpcReply
    {
        public BankReply(NpcReplyRecord record) : base(record) { }

        public override bool Execute(Npc npc, Character character)
        {
            new BankExchange(character, character).Open();

            return true;
        }
    }
}
