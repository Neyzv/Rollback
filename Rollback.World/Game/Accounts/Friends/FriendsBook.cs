using System.Collections.Concurrent;
using Rollback.Common.ORM;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Accounts.Friends;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Social;
using Rollback.World.Network;

namespace Rollback.World.Game.Accounts.Friends
{
    public sealed class FriendsBook
    {
        public const byte MaxRelations = 100;

        private readonly ConcurrentQueue<Relation> _deletedRelations;
        private readonly ConcurrentDictionary<int, Relation> _relations;

        public Character Owner { get; }

        public FriendInformations[] FriendsInformations =>
            _relations.Values.OfType<Friend>().Select(x => x.FriendInformations).ToArray();

        public IgnoredInformations[] IgnoredsInformations =>
            _relations.Values.OfType<Ignored>().Select(x => x.IgnoredInformations).ToArray();

        public FriendsBook(Character owner)
        {
            _deletedRelations = new();
            _relations = new();
            Owner = owner;

            Load();
        }

        private void Load()
        {
            foreach (var relation in DatabaseAccessor.Instance.Select<AccountRelationRecord>(
                string.Format(AccountRelationRelator.GetAccountRelationByAccountId, Owner.Client.Account!.Id)))
            {
                if (relation.AccountInformationsRecord is null ||
                    !_relations.TryAdd(relation.TargetId, relation.RelationType is AccountRelationType.Friend ?
                        new Friend(this, relation) : new Ignored(this, relation))) // account deleted
                    DatabaseAccessor.Instance.Delete(relation);
            }

            // Notify our connection
            foreach (var client in WorldServer.Instance.GetClients(x => x.Account?.Character is not null))
                client.Account!.Character!.FriendsBook.NotifyConnection(Owner);
        }

        public bool IsInRelation(int accountId) =>
            _relations.ContainsKey(accountId);

        public bool AddRelation(AccountRelationType relationType, AccountInformations account)
        {
            var record = new AccountRelationRecord()
            {
                AccountId = Owner.Client.Account!.Id,
                TargetId = account.Id,
                RelationType = relationType
            };

            Relation relation = relationType is AccountRelationType.Friend ? new Friend(this, record) : new Ignored(this, record);
            var result = _relations.Count < MaxRelations && _relations.TryAdd(account.Id, relation);

            if (result)
            {
                if (relation is Friend friend)
                {
                    SocialHandler.SendFriendAddedMessage(Owner.Client, friend);
                    SocialHandler.SendFriendsListMessage(Owner.Client);
                }
                else if (relation is Ignored ignored)
                {
                    SocialHandler.SendIgnoredAddedMessage(Owner.Client, ignored);
                    SocialHandler.SendIgnoredListMessage(Owner.Client);
                }

                if (account.Character is not null)
                    relation.SetOnline(account.Character);
            }

            return result;
        }

        public void DeleteRelation(AccountRelationType relationType, string name)
        {
            var result = false;
            if (_relations.FirstOrDefault(x => x.Value.Nickname == name) is { } relation &&
                (result = _relations.TryRemove(relation)))
                _deletedRelations.Enqueue(relation.Value);

            if (relationType is AccountRelationType.Friend)
                SocialHandler.SendFriendDeleteResultMessage(Owner.Client, result, name);
            else
                SocialHandler.SendIgnoredDeleteResultMessage(Owner.Client, result, name);
        }

        public void NotifyConnection(Character character)
        {
            if (_relations.ContainsKey(character.Client.Account!.Id))
                _relations[character.Client.Account.Id].SetOnline(character);
        }

        public void NotifyDisconnection(Character character)
        {
            if (_relations.ContainsKey(character.Client.Account!.Id))
                _relations[character.Client.Account.Id].SetOffline();
        }

        public void Save()
        {
            while (_deletedRelations.TryDequeue(out var relation))
                relation.Delete();

            foreach (var relation in _relations.Values)
                relation.Save();
        }
    }
}
