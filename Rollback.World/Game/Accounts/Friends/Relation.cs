using Rollback.Common.ORM;
using Rollback.World.Database.Accounts.Friends;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Accounts.Friends
{
    public abstract class Relation
    {
        protected readonly FriendsBook _book;
        protected readonly AccountRelationRecord _record;
        protected Character? _character;

        public string Nickname =>
            _character is null ? _record.AccountInformationsRecord!.Nickname : _character.Client.Account!.Nickname;

        public Relation(FriendsBook book, AccountRelationRecord record)
        {
            _book = book;
            _record = record;
        }

        protected virtual void Online() { }

        public void SetOnline(Character character)
        {
            if (_character is null && _record.TargetId == character.Client.Account!.Id)
            {
                _character = character;
                Online();
            }
        }

        protected virtual void Offline() { }

        public void SetOffline()
        {
            if (_character is not null)
            {
                Offline();

                _record.AccountInformationsRecord!.Nickname = _character.Client.Account!.Nickname;
                _record.AccountInformationsRecord!.LastServerConnection = _character.Client.Account!.LastServerConnection;
                _character = default;
            }
        }

        public void Save()
        {
            if (!DatabaseAccessor.Instance.IsInDatabase(_record))
                DatabaseAccessor.Instance.Insert(_record);
        }

        public void Delete() =>
            DatabaseAccessor.Instance.Delete(_record);
    }
}
