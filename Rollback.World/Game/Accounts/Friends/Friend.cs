using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Accounts.Friends;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Characters;
using Rollback.World.Handlers.Social;

namespace Rollback.World.Game.Accounts.Friends
{
    public sealed class Friend : Relation
    {
        public FriendInformations FriendInformations =>
            _character is null ? new FriendInformations(_record.AccountInformationsRecord!.Nickname, (sbyte)PlayerConnectedStatus.Offline,
                (int)(System.DateTime.Now - _record.AccountInformationsRecord!.LastServerConnection).TotalHours)
            : new FriendOnlineInformations(_character.Client.Account!.Nickname, (sbyte)(_character.Fighter is null ? PlayerConnectedStatus.Online : PlayerConnectedStatus.InFight),
                (int)(System.DateTime.Now - _character.Client.Account!.LastServerConnection).TotalHours, _character.Name,
                _character.Level, (sbyte)_character.AlignmentSide, (sbyte)_character.Breed, _character.Sex, _character.GuildMember is null ? string.Empty : _character.GuildMember.Guild.Name);

        public Friend(FriendsBook book, AccountRelationRecord record) : base(book, record) { }

        private void OnFriendLevelChanged(Character character)
        {
            SocialHandler.SendFriendUpdateMessage(_book.Owner.Client, this);

            if (_book.Owner.Client.Account!.WarnOnFriendsLevelGain)
                CharacterHandler.SendCharacterLevelUpInformationMessage(_book.Owner.Client, character);
        }

        protected override void Online()
        {
            if (_book.Owner.Client.Account!.WarnOnFriendsConnection)
                // %1 est en ligne.
                _book.Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 143, _character!.Name);

            _character!.LevelChanged += OnFriendLevelChanged;
        }

        protected override void Offline() =>
            _character!.LevelChanged -= OnFriendLevelChanged;
    }
}
