using Rollback.Common.ORM;
using Rollback.Protocol.Enums;
using Rollback.World.Database.Accounts;
using Rollback.World.Database.Characters;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Social;

namespace Rollback.World.Game.Accounts
{
    public class AccountInformations
    {
        private readonly AccountInformationsRecord _record;

        public int Id { get; }

        public GameHierarchyEnum Role { get; }

        public string Ticket { get; }

        public Character? Character { get; set; }

        public Dictionary<int, CharacterRecord> Characters { get; }

        public string SecretAnswer { get; }

        public DateTime? LastConnection { get; }

        public string? LastIP { get; }

        public sbyte FreeCharacterSlots { get; set; }

        public Dictionary<int, Gift> Gifts { get; set; }

        public string Nickname =>
            _record.Nickname;

        public int BankKamas
        {
            get => _record.BankKamas;
            private set => _record.BankKamas = value;
        }

        public DateTime LastServerConnection =>
            _record.LastServerConnection;

        public bool WarnOnFriendsConnection
        {
            get => _record.WarnOnFriendsConnection;
            set
            {
                _record.WarnOnFriendsConnection = value;

                if (Character is not null)
                    SocialHandler.SendFriendWarnOnConnectionStateMessage(Character.Client);
            }
        }

        public bool WarnOnFriendsLevelGain
        {
            get => _record.WarnOnFriendsLevelGain; set
            {
                _record.WarnOnFriendsLevelGain = value;

                if (Character is not null)
                    SocialHandler.SendFriendWarnOnLevelGainStateMessage(Character.Client);
            }
        }

        public AccountInformations(AccountInformationsRecord? record, int id, string nickname, GameHierarchyEnum role, string ticket, Dictionary<int, CharacterRecord> characters, string secretAnswer,
            sbyte freeCharacterSlots, DateTime? lastConnection, string? lastIP, Dictionary<int, Gift> gifts)
        {
            Id = id;
            Role = role;
            Ticket = ticket;
            Characters = characters;
            SecretAnswer = secretAnswer;
            FreeCharacterSlots = freeCharacterSlots;
            LastConnection = lastConnection;
            LastIP = lastIP;
            Gifts = gifts;

            _record = record is null ? CreateRecord() : record;
            _record.Nickname = nickname;
            _record.LastServerConnection = DateTime.Now;
        }

        private AccountInformationsRecord CreateRecord()
        {
            var record = new AccountInformationsRecord() { AccountId = Id };
            DatabaseAccessor.Instance.Insert(record);

            return record;
        }

        public void Save()
        {
            if (Character is not null)
            {
                BankKamas = Character.Bank.Kamas;

                Character.LeaveGame();
            }

            DatabaseAccessor.Instance.Update(_record);
        }
    }
}
