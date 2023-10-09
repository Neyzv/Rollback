using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.Interactions.Dialogs.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("Dialog")]
    public sealed class DialogReply : NpcReply
    {
        private short? _newMessageId;
        public short? NewMessageId =>
            _newMessageId ??= GetParameterValue<short>(0);

        public DialogReply(NpcReplyRecord record) : base(record)
        {
        }

        public override bool Execute(Npc npc, Character character)
        {
            if (character.Interaction is NpcDialog dialog && NewMessageId.HasValue)
                dialog.SetCurrentMessage(NewMessageId.Value);

            return false;
        }
    }
}
