using Rollback.Protocol.Enums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Party;

namespace Rollback.World.Game.Interactions.Requests.Parties
{
    public sealed class PartyRequest : Request
    {
        public PartyRequest(Character sender, Character receiver) : base(sender, receiver) { }

        protected override void InternalOpen()
        {
            PartyHandler.SendPartyInvitationMessage(Sender.Client, Sender, Receiver);
            PartyHandler.SendPartyInvitationMessage(Receiver.Client, Sender, Receiver);
        }

        protected override void InternalAccept()
        {
            PartyHandler.SendPartyRefuseInvitationNotificationMessage(Sender.Client);

            if (Sender.Party is not null)
                Sender.Party.Join(Receiver);
            else
                PartyHandler.SendPartyCannotJoinErrorMessage(Receiver.Client, PartyJoinErrorEnum.PARTY_JOIN_ERROR_PARTY_NOT_FOUND);
        }

        protected override void InternalClose()
        {
            PartyHandler.SendPartyRefuseInvitationNotificationMessage(Sender.Client);
            PartyHandler.SendPartyRefuseInvitationNotificationMessage(Receiver.Client);

            if (Sender.Party is not null)
                Sender.Party.Leave(Receiver);
            else
                PartyHandler.SendPartyCannotJoinErrorMessage(Receiver.Client, PartyJoinErrorEnum.PARTY_JOIN_ERROR_PARTY_NOT_FOUND);
        }
    }
}
