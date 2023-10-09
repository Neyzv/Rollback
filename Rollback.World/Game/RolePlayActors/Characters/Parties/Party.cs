using System.Collections.Concurrent;
using Rollback.Common.Network;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Enums;
using Rollback.World.Game.Interactions.Requests.Parties;
using Rollback.World.Handlers.Party;
using Rollback.World.Network;

namespace Rollback.World.Game.RolePlayActors.Characters.Parties
{
    public sealed class Party : ClientCollection<WorldClient, Message>
    {
        public const byte MaxMemberCount = 8;

        private readonly ConcurrentDictionary<int, Character> _requestedMembers;
        private readonly ConcurrentDictionary<int, Character> _members;

        public Character Leader { get; private set; }

        public Party(Character leader)
        {
            _requestedMembers = new();
            _members = new();
            Leader = leader;

            Join(leader);
        }

        protected override IEnumerable<WorldClient> GetClients() =>
            _members.Values.Select(x => x.Client);

        public Character[] GetMembers(Predicate<Character>? p = default) =>
            (p is null ? _members.Values : _members.Values.Where(x => p(x))).ToArray();

        public Character? GetMemberById(int id) =>
            _members.ContainsKey(id) ? _members[id] : default;

        public Character? GetMember(Predicate<Character> p) =>
            _members.Values.FirstOrDefault(x => p(x));

        private void Disband()
        {
            foreach (var member in _members.Values)
            {
                member.Party = default;

                PartyHandler.SendPartyLeaveMessage(member.Client);
            }
        }

        public void ChangeLeader(Character character)
        {
            if (_members.ContainsKey(character.Id) && Leader.Id != character.Id)
            {
                Leader = character;

                Send(PartyHandler.SendPartyLeaderUpdateMessage);
            }
        }

        public void Join(Character character)
        {
            if (_members.TryAdd(character.Id, character))
            {
                _requestedMembers.TryRemove(character.Id, out _);

                character.Party = this;

                PartyHandler.SendPartyJoinMessage(character.Client);
                RefreshMember(character);
            }
        }

        public void Leave(Character character, bool kicked = false)
        {
            if (_members.TryRemove(character.Id, out _))
            {
                if (kicked)
                    PartyHandler.SendPartyKickedByMessage(character.Client);
                else
                    PartyHandler.SendPartyLeaveMessage(character.Client);

                character.Party = default;
            }
            else
                _requestedMembers.TryRemove(character.Id, out _);

            if (_members.Count + _requestedMembers.Count is 1)
                Disband();
            else if (Leader.Id == character.Id)
                ChangeLeader(_members.Values.First());
            else
                Send(PartyHandler.SendPartyMemberRemoveMessage, new object[] { character.Id });
        }

        public void Invite(Character sender, Character receiver)
        {
            if (_members.Count + _requestedMembers.Count < MaxMemberCount)
                if (_requestedMembers.TryAdd(receiver.Id, receiver))
                    new PartyRequest(sender, receiver).Open();
                else
                    PartyHandler.SendPartyCannotJoinErrorMessage(sender.Client, PartyJoinErrorEnum.PARTY_JOIN_ERROR_PLAYER_IS_ALREADY_BEING_INVITED);
            else
                PartyHandler.SendPartyCannotJoinErrorMessage(sender.Client, PartyJoinErrorEnum.PARTY_JOIN_ERROR_PARTY_FULL);
        }

        public void RefreshMember(Character character)
        {
            if (_members.ContainsKey(character.Id))
                Send(PartyHandler.SendPartyUpdateMessage, new object[] { character.PartyMemberInformations });
        }
    }
}
