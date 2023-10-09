using Rollback.Protocol.Enums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Guilds;

namespace Rollback.World.Game.Interactions.Requests.Guilds
{
    public sealed class GuildInvitationRequest : Request
    {
        public GuildInvitationRequest(Character sender, Character receiver) : base(sender, receiver) { }

        protected override void InternalOpen()
        {
            GuildHandler.SendGuildInvitationStateRecruterMessage(Sender.Client, Receiver, GuildInvitationStateEnum.GUILD_INVITATION_SENT);
            GuildHandler.SendGuildInvitationStateRecrutedMessage(Receiver.Client, GuildInvitationStateEnum.GUILD_INVITATION_SENT);

            if (Sender.GuildMember is not null)
                GuildHandler.SendGuildInvitedMessage(Receiver.Client, Sender, Receiver);
        }

        protected override void InternalAccept()
        {
            GuildHandler.SendGuildInvitationStateRecruterMessage(Sender.Client, Receiver, GuildInvitationStateEnum.GUILD_INVITATION_OK);
            GuildHandler.SendGuildInvitationStateRecrutedMessage(Receiver.Client, GuildInvitationStateEnum.GUILD_INVITATION_OK);

            if (Sender.GuildMember is not null && Receiver.GuildMember is null)
                Sender.GuildMember.Guild.AddMember(Receiver);
        }

        protected override void InternalClose()
        {
            GuildHandler.SendGuildInvitationStateRecruterMessage(Sender.Client, Receiver, GuildInvitationStateEnum.GUILD_INVITATION_CANCELED);
            GuildHandler.SendGuildInvitationStateRecrutedMessage(Receiver.Client, GuildInvitationStateEnum.GUILD_INVITATION_CANCELED);
        }
    }
}
