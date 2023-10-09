using Rollback.Common.ORM;
using Rollback.World.Game.Guilds;
using GuildMember = Rollback.World.Game.Guilds.GuildMember;

namespace Rollback.World.Database.TaxCollectors
{
    public static class TaxCollectorRelator
    {
        public const string GetTaxCollectors = "SELECT * FROM tax_collectors";
    }

    [Table("tax_collectors")]
    public sealed record TaxCollectorRecord
    {
        [Key]
        public int Id { get; set; }

        private int _hirerId;
        public int HirerId
        {
            get => _hirerId;
            set
            {
                _hirerId = value;
                Hirer = GuildManager.Instance.GetGuildMemberById(value);
            }
        }

        [Ignore]
        public GuildMember? Hirer { get; set; }

        public short FirstNameId { get; set; }

        public short LastNameId { get; set; }

        public int MapId { get; set; }

        public DateTime HiredDate { get; set; }

        public int GatheredExperience { get; set; }

        public int GatheredKamas { get; set; }
    }
}
