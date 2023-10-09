using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;
using Rollback.Protocol.Types;
using Rollback.World.Game.Interactions.Requests.Parties;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Party
{
    public static class PartyHandler
    {
        [WorldHandler(PartyInvitationRequestMessage.Id)]
        public static void HandlePartyInvitationRequestMessage(WorldClient client, PartyInvitationRequestMessage message)
        {
            var target = WorldServer.Instance.GetClient(x => x.Account?.Character?.Name == message.name)?.Account?.Character;

            PartyJoinErrorEnum? partyJoinError = default;

            if (target is null)
                partyJoinError = PartyJoinErrorEnum.PARTY_JOIN_ERROR_PLAYER_NOT_FOUND;
            else if (target.IsBusy || client.Account!.Character!.IsBusy)
                partyJoinError = PartyJoinErrorEnum.PARTY_JOIN_ERROR_PLAYER_BUSY;
            else if (target.Party is not null)
                partyJoinError = PartyJoinErrorEnum.PARTY_JOIN_ERROR_PLAYER_ALREADY_IN_A_PARTY;

            if (partyJoinError is null)
            {
                if (client.Account!.Character!.Party is null)
                    client.Account.Character.Party = new(client.Account.Character);

                client.Account!.Character!.Party.Invite(client.Account.Character, target!);
            }
            else
                SendPartyCannotJoinErrorMessage(client, partyJoinError.Value);
        }

        [WorldHandler(PartyAcceptInvitationMessage.Id)]
        public static void HandlePartyAcceptInvitationMessage(WorldClient client, PartyAcceptInvitationMessage message)
        {
            if (client.Account!.Character!.Interaction is PartyRequest request)
                request.Accept();
            else
                SendPartyCannotJoinErrorMessage(client, PartyJoinErrorEnum.PARTY_JOIN_ERROR_UNKNOWN);
        }

        [WorldHandler(PartyRefuseInvitationMessage.Id)]
        public static void HandlePartyRefuseInvitationMessage(WorldClient client, PartyRefuseInvitationMessage message)
        {
            if (client.Account!.Character!.Interaction is PartyRequest request)
                request.Close();
            else
                SendPartyCannotJoinErrorMessage(client, PartyJoinErrorEnum.PARTY_JOIN_ERROR_UNKNOWN);
        }

        [WorldHandler(PartyAbdicateThroneMessage.Id)]
        public static void HandlePartyAbdicateThroneMessage(WorldClient client, PartyAbdicateThroneMessage message)
        {
            if (client.Account!.Character!.Party?.Leader.Id == client.Account.Character.Id)
            {
                var newLeader = client.Account.Character.Party.GetMemberById(message.playerId);

                if (newLeader is not null)
                    client.Account.Character.Party.ChangeLeader(newLeader);
            }
        }

        [WorldHandler(PartyLeaveRequestMessage.Id)]
        public static void HandlePartyLeaveRequestMessage(WorldClient client, PartyLeaveRequestMessage message) =>
            client.Account!.Character!.Party?.Leave(client.Account.Character);

        [WorldHandler(PartyFollowMemberRequestMessage.Id)]
        public static void HandlePartyFollowMemberRequestMessage(WorldClient client, PartyFollowMemberRequestMessage message)
        {
            var member = client.Account!.Character!.Party?.GetMemberById(message.playerId);
            if (member is not null)
                client.Account.Character.FollowPartyMember(member);
        }

        [WorldHandler(PartyStopFollowRequestMessage.Id)]
        public static void HandlePartyStopFollowRequestMessage(WorldClient client, PartyStopFollowRequestMessage message) =>
            client.Account!.Character!.UnFollowPartyMember();

        [WorldHandler(PartyFollowThisMemberRequestMessage.Id)]
        public static void HandlePartyFollowThisMemberRequestMessage(WorldClient client, PartyFollowThisMemberRequestMessage message)
        {
            if (client.Account!.Character!.Party?.Leader.Id == client.Account.Character.Id)
            {
                var target = client.Account.Character.Party.GetMemberById(message.playerId);

                if (target is not null)
                    foreach (var member in client.Account.Character.Party.GetMembers(x => x.Id != message.playerId))
                        if (message.enabled)
                            member.FollowPartyMember(target);
                        else
                            member.UnFollowPartyMember();
            }
        }

        [WorldHandler(PartyKickRequestMessage.Id)]
        public static void HandlePartyKickRequestMessage(WorldClient client, PartyKickRequestMessage message)
        {
            if (client.Account!.Character!.Party?.GetMemberById(message.playerId) is { } partyMember)
                client.Account.Character.Party.Leave(partyMember, true);
        }

        public static void SendPartyCannotJoinErrorMessage(WorldClient client, PartyJoinErrorEnum reason) =>
            client.Send(new PartyCannotJoinErrorMessage((sbyte)reason));

        public static void SendPartyInvitationMessage(WorldClient client, Character sender, Character target) =>
            client.Send(new PartyInvitationMessage(sender.Id, sender.Name, target.Id, target.Name));

        public static void SendPartyUpdateMessage(WorldClient client, PartyMemberInformations memberInfos) =>
            client.Send(new PartyUpdateMessage(memberInfos));

        public static void SendPartyJoinMessage(WorldClient client) =>
            client.Send(new PartyJoinMessage(client.Account!.Character!.Party!.Leader.Id, client.Account.Character.Party.GetMembers().Select(x => x.PartyMemberInformations).ToArray()));

        public static void SendPartyLeaveMessage(WorldClient client) =>
            client.Send(new PartyLeaveMessage());

        public static void SendPartyLeaderUpdateMessage(WorldClient client) =>
            client.Send(new PartyLeaderUpdateMessage(client.Account!.Character!.Party!.Leader.Id));

        public static void SendPartyKickedByMessage(WorldClient client) =>
            client.Send(new PartyKickedByMessage(client.Account!.Character!.Party!.Leader.Id));

        public static void SendPartyMemberRemoveMessage(WorldClient client, int memberRemovedId) =>
            client.Send(new PartyMemberRemoveMessage(memberRemovedId));

        public static void SendPartyRefuseInvitationNotificationMessage(WorldClient client) =>
            client.Send(new PartyRefuseInvitationNotificationMessage());

        public static void SendPartyFollowStatusUpdateMessage(WorldClient client, int characterId) =>
            client.Send(new PartyFollowStatusUpdateMessage(true, characterId));
    }
}
