using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.Database.Characters;
using Rollback.World.Game.Experiences;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Compass;
using Rollback.World.Handlers.Party;
using Rollback.World.Handlers.Social;

namespace Rollback.World.Game.Accounts.Friends
{
    public sealed class Spouse
    {
        public const byte MaxDistanceToSpouse = 20;

        private readonly CharacterRecord _record;
        private readonly Character _owner;

        public int Id =>
            _record.Id;

        public Character? Character { get; set; }

        public bool IsFollowing { get; private set; }

        public FriendSpouseInformations FriendSpouseInformations =>
            Character is null ? new FriendSpouseInformations(_record.Id, _record.Name, Level, (sbyte)_record.Breed, Convert.ToSByte(_record.Sex),
                _record.Look.GetEntityLook())
            : new FriendSpouseOnlineInformations(Character.Id, Character.Name, Character.Level, (sbyte)Character.Breed, Convert.ToSByte(_record.Sex),
                Character.Look.GetEntityLook(), Character.Fighter is not null, false, Character.PvPEnabled, Character.MapInstance.Map.Record.Id,
                Character.MapInstance.Map.SubArea?.Id ?? 0, Character.GuildMember?.Guild.Name ?? string.Empty, (sbyte)Character.AlignmentSide); // TO DO

        private byte? _level;
        public byte Level =>
           Character is null ? _level ??= ExperienceManager.Instance.GetCharacterLevel(_record.Experience) : Character.Level;

        public bool Sex =>
            Character is null ? _record.Sex : Character.Sex;

        public Spouse(Character owner, CharacterRecord record)
        {
            _owner = owner;
            _record = record;
        }

        public void Tp()
        {
            if (Character is not null && !Character.MapInstance.IsDungeon)
                _owner.Teleport(Character.MapInstance, Character.Cell.Id, Character.Direction);
        }

        private void OnSpouseChangeMap(Character character, MapInstance map) =>
            CompassHandler.SendCompassUpdateSpouseMessage(_owner.Client);

        public void Follow()
        {
            if (Character is not null && !IsFollowing)
            {
                IsFollowing = true;

                Character.ChangeMap += OnSpouseChangeMap;
                Character.Disconnect += Unfollow;

                CompassHandler.SendCompassUpdateSpouseMessage(_owner.Client);
                // <b>%1</b> suit votre déplacement.
                Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 52, _owner.Name);
            }
        }

        public void Unfollow()
        {
            if (Character is not null && IsFollowing)
            {
                IsFollowing = false;

                Character.ChangeMap -= OnSpouseChangeMap;
                Character.Disconnect -= Unfollow;

                PartyHandler.SendPartyFollowStatusUpdateMessage(_owner.Client, 0);
                // <b>%1</b> ne suit plus votre déplacement.
                Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 53, _owner.Name);
            }
        }

        public void SetOnline(Character character)
        {
            if (Character is null && character.Id == _record.Id)
            {
                Character = character;

                Refresh();
            }
        }

        public void SetOffline()
        {
            Character = default; // to do actu
            _level = default;

            Refresh();
        }

        public void Refresh() =>
            SocialHandler.SendFriendsListWithSpouseMessage(_owner.Client);
    }
}
