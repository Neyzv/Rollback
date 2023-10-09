using Rollback.Common.ORM;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Game.Experiences;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Database.Guilds
{
    public static class GuildMemberRelator
    {
        public const string GetMembers = "SELECT * FROM guilds_members";
    }

    [Table("guilds_members")]
    public sealed record GuildMemberRecord
    {
        [Key]
        public int GuildId { get; set; }

        private int _memberId;
        [Key]
        public int MemberId
        {
            get => _memberId;
            set
            {
                _memberId = value;
                MemberRecord = CharacterManager.GetCharacterRecordById(value);

                if (MemberRecord is not null)
                    Level = ExperienceManager.Instance.GetCharacterLevel(MemberRecord.Experience);
            }
        }

        [Ignore]
        public CharacterRecord? MemberRecord { get; private set; }

        [Ignore]
        public byte Level { get; set; }

        public GuildRank MemberRank { get; set; }

        public GuildRight Rights { get; set; }

        public sbyte GivenXPPercent { get; set; }

        public long GivenXP { get; set; }
    }
}
