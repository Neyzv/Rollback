using Rollback.Protocol.Types;
using Rollback.World.Database.Accounts.Friends;

namespace Rollback.World.Game.Accounts.Friends
{
    public sealed class Ignored : Relation
    {
        public IgnoredInformations IgnoredInformations =>
            _character is null ? new IgnoredInformations(_record.AccountInformationsRecord!.Nickname)
            : new IgnoredOnlineInformations(_character.Client.Account!.Nickname, _character.Name, (sbyte)_character.Breed, _character.Sex);

        public Ignored(FriendsBook book, AccountRelationRecord record) : base(book, record) { }

        protected override void Online()
        {

        }

        protected override void Offline()
        {

        }
    }
}
