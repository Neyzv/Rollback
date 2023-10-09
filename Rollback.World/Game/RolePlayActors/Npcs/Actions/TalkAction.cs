using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.Interactions.Dialogs.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Actions
{
    [Identifier("Talk")]
    public class TalkAction : NpcAction
    {
        private short? _messageId;
        public short? MessageId =>
            _messageId ??= GetParameterValue<short>(0);

        public override NpcActionType NpcActionType =>
            NpcActionType.Talk;

        public TalkAction(NpcActionRecord record) : base(record)
        {
        }

        public override void Execute(Npc npc, Character character)
        {
            if (MessageId.HasValue)
            {
                var dialog = new NpcDialog(character, npc);
                dialog.Open();
                dialog.SetCurrentMessage(MessageId.Value);
            }
            else
                character.ReplyError($"Could not find message parameter for npc {npc.Record.Id}...");
        }
    }
}
