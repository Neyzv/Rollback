using System.Collections.Concurrent;
using Rollback.Common.Logging;
using Rollback.Common.ORM;

namespace Rollback.World.Database.Guilds
{
    public static class GuildRelator
    {
        public const string GetGuilds = "SELECT * FROM guilds";
    }

    [Table("guilds")]
    public sealed record GuildRecord
    {
        public GuildRecord()
        {
            Name = string.Empty;
            _spellsLevelsCSV = string.Empty;
            SpellsLevels = new();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public short Symbol { get; set; }

        public int SymbolColor { get; set; }

        public short Background { get; set; }

        public int BackgroundColor { get; set; }

        public long Experience { get; set; }

        public sbyte MaxTaxCollectors { get; set; }

        public short TaxCollectorPods { get; set; }

        public short TaxCollectorProspecting { get; set; }

        public short TaxCollectorWisdom { get; set; }

        public short BoostPoints { get; set; }

        private string _spellsLevelsCSV;
        public string SpellsLevelsCSV
        {
            get => _spellsLevelsCSV;
            set
            {
                _spellsLevelsCSV = value;

                if (!string.IsNullOrEmpty(value))
                {
                    foreach (var spellInfos in value.Split(';'))
                    {
                        var spellInfosSplitted = spellInfos.Split(',');

                        if (spellInfosSplitted.Length == 2 && short.TryParse(spellInfosSplitted[0], out var spellId) &&
                            sbyte.TryParse(spellInfosSplitted[1], out var spellLevel))
                            SpellsLevels.TryAdd(spellId, spellLevel);
                        else
                            Logger.Instance.LogError(msg: $"Can not parse spell informations {spellInfos}, for guild {Id}...");
                    }
                }
            }
        }

        [Ignore]
        public ConcurrentDictionary<short, sbyte> SpellsLevels { get; private set; }
    }
}
