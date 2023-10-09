using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("Kamas")]
    public sealed class AddKamasReply : NpcReply
    {
        private int? _amount;
        public int? Amount =>
            _amount ??= GetParameterValue<int>(0);

        public AddKamasReply(NpcReplyRecord record) : base(record) { }

        public override bool Execute(Npc npc, Character character)
        {
            if (Amount.HasValue)
                character.ChangeKamas(Amount.Value);

            return true;
        }
    }
}
