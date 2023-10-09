using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Accounts.Friends;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Social
{
    public static class SocialHandler
    {
        [WorldHandler(FriendsGetListMessage.Id)]
        public static void HandleFriendsGetListMessage(WorldClient client, FriendsGetListMessage message)
        {
            if (client.Account!.Character!.Spouse is null)
                SendFriendsListMessage(client);
            else
                SendFriendsListWithSpouseMessage(client);
        }

        [WorldHandler(IgnoredGetListMessage.Id)]
        public static void HandleIgnoredGetListMessage(WorldClient client, IgnoredGetListMessage message) =>
            SendIgnoredListMessage(client);

        private static void AddRelation(WorldClient client, AccountRelationType relationType, string name)
        {
            var account = (WorldServer.Instance.GetClient(x => x.Account?.Character?.Name == name)
                ?? WorldServer.Instance.GetClient(x => x.Account?.Nickname == name))?.Account;

            if (account is not null && (client.Account!.Role is not GameHierarchyEnum.PLAYER || account.Role is GameHierarchyEnum.PLAYER))
            {
                if (account.Id != client.Account!.Id)
                {
                    if (!client.Account!.Character!.FriendsBook.IsInRelation(account.Id))
                    {
                        if (!client.Account.Character.FriendsBook.AddRelation(relationType, account))
                            SendFriendAddFailureMessage(client, ListAddFailureEnum.LIST_ADD_FAILURE_OVER_QUOTA);
                    }
                    else
                        SendFriendAddFailureMessage(client, ListAddFailureEnum.LIST_ADD_FAILURE_IS_DOUBLE);
                }
                else
                    SendFriendAddFailureMessage(client, ListAddFailureEnum.LIST_ADD_FAILURE_EGOCENTRIC);
            }
            else
                SendFriendAddFailureMessage(client, ListAddFailureEnum.LIST_ADD_FAILURE_NOT_FOUND);
        }

        [WorldHandler(FriendAddRequestMessage.Id)]
        public static void HandleFriendAddRequestMessage(WorldClient client, FriendAddRequestMessage message) =>
            AddRelation(client, AccountRelationType.Friend, message.name);

        [WorldHandler(IgnoredAddRequestMessage.Id)]
        public static void HandleIgnoredAddRequestMessage(WorldClient client, IgnoredAddRequestMessage message) =>
            AddRelation(client, AccountRelationType.Ignored, message.name);

        [WorldHandler(FriendSetWarnOnConnectionMessage.Id)]
        public static void HandleFriendSetWarnOnConnectionMessage(WorldClient client, FriendSetWarnOnConnectionMessage message) =>
            client.Account!.WarnOnFriendsConnection = message.enable;

        [WorldHandler(FriendSetWarnOnLevelGainMessage.Id)]
        public static void HandleFriendSetWarnOnLevelGainMessage(WorldClient client, FriendSetWarnOnLevelGainMessage message) =>
            client.Account!.WarnOnFriendsLevelGain = message.enable;

        [WorldHandler(FriendDeleteRequestMessage.Id)]
        public static void HandleFriendDeleteRequestMessage(WorldClient client, FriendDeleteRequestMessage message) =>
            client.Account!.Character!.FriendsBook.DeleteRelation(AccountRelationType.Friend, message.name);

        [WorldHandler(IgnoredDeleteRequestMessage.Id)]
        public static void HandleIgnoredDeleteRequestMessage(WorldClient client, IgnoredDeleteRequestMessage message) =>
            client.Account!.Character!.FriendsBook.DeleteRelation(AccountRelationType.Ignored, message.name);

        [WorldHandler(FriendSpouseJoinRequestMessage.Id)]
        public static void HandleFriendSpouseJoinRequestMessage(WorldClient client, FriendSpouseJoinRequestMessage message)
        {
            if (client.Account!.Character!.Spouse is not null && !client.Account.Character.IsBusy)
            {
                if (client.Account.Character.Spouse.Character is not null)
                    if (client.Account.Character.Spouse.Character.Cell.Point.ManhattanDistanceTo(client.Account.Character.Cell.Point) <= Spouse.MaxDistanceToSpouse)
                        if (!client.Account.Character.Spouse.Character.IsBusy)
                            client.Account.Character.Spouse.Tp();
                        else
                            // Ton épouse n\'est pas disponible pour l\'instant...
                            // Ton époux n\'est pas disponible pour l\'instant...
                            client.Account.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR,
                                    (short)(client.Account.Character.Spouse.Sex is true ? 39 : 40));
                    else
                        // "Votre femme est trop éloignée pour vous permettre de la rejoindre."
                        // "Votre mari est trop éloigné pour vous permettre de le rejoindre."
                        client.Account.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR,
                                (short)(client.Account.Character.Spouse.Sex is true ? 81 : 80));
                else
                    // "Ton épouse n\'est pas connectée.";
                    // "Ton mari n\'est pas connecté.";
                    client.Account.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR,
                            (short)(client.Account.Character.Spouse.Sex is true ? 36 : 37));
            }
        }

        [WorldHandler(FriendSpouseFollowWithCompassRequestMessage.Id)]
        public static void HandleFriendSpouseFollowWithCompassRequestMessage(WorldClient client, FriendSpouseFollowWithCompassRequestMessage message)
        {
            if (client.Account!.Character!.Spouse is not null)
                if (client.Account.Character.Spouse.IsFollowing)
                    client.Account.Character.Spouse.Unfollow();
                else
                    client.Account.Character.Spouse.Follow();
        }

        public static void SendFriendsListMessage(WorldClient client) =>
            client.Send(new FriendsListMessage(client.Account!.Character!.FriendsBook.FriendsInformations));

        public static void SendIgnoredListMessage(WorldClient client) =>
            client.Send(new IgnoredListMessage(client.Account!.Character!.FriendsBook.IgnoredsInformations));

        public static void SendFriendAddFailureMessage(WorldClient client, ListAddFailureEnum reason) =>
            client.Send(new FriendAddFailureMessage((sbyte)reason));

        public static void SendFriendAddedMessage(WorldClient client, Friend friend) =>
            client.Send(new FriendAddedMessage(friend.FriendInformations));

        public static void SendIgnoredAddedMessage(WorldClient client, Ignored ignored) =>
            client.Send(new IgnoredAddedMessage(ignored.IgnoredInformations));

        public static void SendFriendWarnOnConnectionStateMessage(WorldClient client) =>
            client.Send(new FriendWarnOnConnectionStateMessage(client.Account!.WarnOnFriendsConnection));

        public static void SendFriendWarnOnLevelGainStateMessage(WorldClient client) =>
            client.Send(new FriendWarnOnLevelGainStateMessage(client.Account!.WarnOnFriendsLevelGain));

        public static void SendFriendUpdateMessage(WorldClient client, Friend friend) =>
            client.Send(new FriendUpdateMessage(friend.FriendInformations));

        public static void SendFriendDeleteResultMessage(WorldClient client, bool success, string name) =>
            client.Send(new FriendDeleteResultMessage(success, name));

        public static void SendIgnoredDeleteResultMessage(WorldClient client, bool success, string name) =>
            client.Send(new IgnoredDeleteResultMessage(success, name));

        public static void SendFriendsListWithSpouseMessage(WorldClient client) =>
            client.Send(new FriendsListWithSpouseMessage(client.Account!.Character!.FriendsBook.FriendsInformations,
                client.Account.Character.Spouse!.FriendSpouseInformations));
    }
}
