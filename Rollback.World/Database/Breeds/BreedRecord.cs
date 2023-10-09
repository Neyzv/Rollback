using Rollback.Common.ORM;
using Rollback.Protocol.Enums;
using Rollback.World.Game.RolePlayActors.Characters.Breeds;

namespace Rollback.World.Database.Breeds
{
    public static class BreedRelator
    {
        public const string GetBreeds = "SELECT * FROM breeds";
    }

    [Table("breeds")]
    public sealed record BreedRecord
    {
        public BreedRecord()
        {
            MaleLook = string.Empty;
            FemaleLook = string.Empty;
            _vitality = string.Empty;
            _wisdom = string.Empty;
            _chance = string.Empty;
            _agility = string.Empty;
            _intelligence = string.Empty;
            _strength = string.Empty;

            Spells = new()
            {
                new()
                {
                    BreedId = Id,
                    SpellId = 0
                }
            };
        }

        [Key]
        public int Id { get; set; }

        public int StartMapId { get; set; }

        public short StartCellId { get; set; }

        public DirectionsEnum StartDirection { get; set; }

        public string MaleLook { get; set; }

        public string FemaleLook { get; set; }

        private string? _vitality;
        public string? StatsPointsForVitality
        {
            get => _vitality;
            set
            {
                _vitality = value;
                if (value is not null)
                    VitalityFloors = BreedManager.ParseBreedStats(value!);
            }
        }

        [Ignore]
        public Dictionary<short, KeyValuePair<byte, byte>>? VitalityFloors { get; private set; }

        private string? _wisdom;
        public string? StatsPointsForWisdom
        {
            get => _wisdom;
            set
            {
                _wisdom = value;
                if (value is not null)
                    WisdomFloors = BreedManager.ParseBreedStats(value!);
            }
        }

        [Ignore]
        public Dictionary<short, KeyValuePair<byte, byte>>? WisdomFloors { get; private set; }

        private string? _strength;
        public string? StatsPointsForStrength
        {
            get => _strength;
            set
            {
                _strength = value;
                if (value is not null)
                    StrengthFloors = BreedManager.ParseBreedStats(value!);
            }
        }

        [Ignore]
        public Dictionary<short, KeyValuePair<byte, byte>>? StrengthFloors { get; private set; }

        private string? _intelligence;
        public string? StatsPointsForIntelligence
        {
            get => _intelligence;
            set
            {
                _intelligence = value;
                if (value is not null)
                    IntelligenceFloors = BreedManager.ParseBreedStats(value!);
            }
        }

        [Ignore]
        public Dictionary<short, KeyValuePair<byte, byte>>? IntelligenceFloors { get; private set; }

        private string? _chance;
        public string? StatsPointsForChance
        {
            get => _chance;
            set
            {
                _chance = value;
                if (value is not null)
                    ChanceFloors = BreedManager.ParseBreedStats(value!);
            }
        }

        [Ignore]
        public Dictionary<short, KeyValuePair<byte, byte>>? ChanceFloors { get; private set; }

        private string? _agility;
        public string? StatsPointsForAgility
        {
            get => _agility;
            set
            {
                _agility = value;
                if (value is not null)
                    AgilityFloors = BreedManager.ParseBreedStats(value);
            }
        }

        [Ignore]
        public Dictionary<short, KeyValuePair<byte, byte>>? AgilityFloors { get; private set; }

        [Ignore]
        public List<BreedSpellRecord> Spells { get; set; }
    }
}
