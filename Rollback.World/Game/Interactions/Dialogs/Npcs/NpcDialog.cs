using Rollback.World.CustomEnums;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.Npcs;
using Rollback.World.Game.RolePlayActors.Npcs.Replies;
using Rollback.World.Handlers.Npcs;

namespace Rollback.World.Game.Interactions.Dialogs.Npcs
{
    public sealed class NpcDialog : Dialog<Npc>
    {
        public short? CurrentMessageId { get; set; }

        public List<NpcReply> Replies { get; private set; }

        public override DialogType DialogType =>
            DialogType.Dialog;

        public NpcDialog(Character character, Npc dialoger) : base(character, dialoger) =>
            Replies = new();

        protected override void InternalOpen() =>
            NpcHandler.SendNpcDialogCreationMessage(Character.Client, Dialoger);

        protected override void InternalClose() =>
            NpcHandler.SendLeaveDialogMessage(Character.Client);

        public void SetCurrentMessage(short messageId)
        {
            if (Dialoger.Record.Messages.ContainsKey(messageId))
            {
                CurrentMessageId = messageId;
                Replies = NpcManager.Instance.GetReplies(Character, Dialoger.Record, messageId);
                Refresh();
            }
            else
            {
                Close();
                Character.ReplyError($"Npc {Dialoger.Record.Id} can not display message {messageId}...");
            }
        }

        public void ExecuteReply(short replyId)
        {
            var closeDialog = true;

            foreach (var reply in Replies.OrderBy(x => x.Priority).Where(x => x.ReplyId == replyId))
            {
                if (reply.CanExecute(Character))
                {
                    if (closeDialog)
                        closeDialog = reply.Execute(Dialoger, Character);
                    else
                        reply.Execute(Dialoger, Character);
                }
                else
                {
                    // Certaines conditions ne sont pas satisfaites.
                    Character.SendInformationMessage(Protocol.Enums.TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 34);
                    break;
                }
            }

            if (closeDialog)
                Close();
        }

        public void SetCustomDialog(short messageId, short[] replyIds)
        {
            if (Dialoger.Record.Messages.ContainsKey(messageId))
            {
                CurrentMessageId = messageId;

                var replies = new List<NpcReply>();
                foreach (short replyId in replyIds)
                {
                    if (Dialoger.Record.Replies.ContainsKey(replyId))
                        replies.Add(new CloseDialogReply(new NpcReplyRecord()
                        {
                            MessageId = CurrentMessageId.Value,
                            ReplyId = replyId,
                            Priority = 0,
                        }));
                }

                Replies = replies;

                Refresh();
            }
            else
            {
                Close();
                Character.ReplyError($"Npc {Dialoger.Record.Id} can not display message {messageId}...");
            }
        }

        public void Refresh() =>
            NpcHandler.SendNpcDialogQuestionMessage(Character.Client);
    }
}
