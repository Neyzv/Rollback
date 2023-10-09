using Rollback.Common.ORM;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.Game.Experiences;
using Rollback.World.Game.Looks;

namespace Rollback.World.Database.Characters
{
    public static class CharacterRelator
    {
        public const string GetCharactersIds = "SELECT Id FROM characters";
        public const string GetCharacterById = "SELECT * FROM characters WHERE Id = {0}";
        public const string GetCharacterByName = "SELECT * FROM characters WHERE Name = BINARY '{0}'";
    }

    [Table("characters")]
    public sealed record CharacterRecord
    {
        public CharacterRecord()
        {
            Name = string.Empty;
            BaseEntityLookString = string.Empty;
            _lookString = string.Empty;
            BinaryKnownZaaps = Array.Empty<byte>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public BreedEnum Breed { get; set; }

        public bool Sex { get; set; }

        public string BaseEntityLookString { get; set; }

        private string _lookString;
        public string EntityLookString
        {
            get => _lookString;
            set
            {
                _lookString = value;
                _look = ActorLook.Parse(_lookString);
            }
        }

        private ActorLook? _look;
        [Ignore]
        public ActorLook Look
        {
            get => _look ?? throw new Exception("Can not use Look, it's not yet initialized...");
            set
            {
                _look = value;
                _lookString = value is null ? string.Empty : value.ToString();
            }
        }

        public PlayerLifeStatusEnum LifeState { get; set; }

        public long Experience { get; set; }

        public int Kamas { get; set; }

        public int MapId { get; set; }

        public short CellId { get; set; }

        public DirectionsEnum Direction { get; set; }

        public byte[] BinaryKnownZaaps { get; set; }

        public int? SaveMapId { get; set; }

        public AlignmentSideEnum AlignmentSide { get; set; }

        public ushort Honor { get; set; }

        public ushort Dishonor { get; set; }

        public bool PvPEnabled { get; set; }

        public short StatsPoints { get; set; }

        public short SpellsPoints { get; set; }

        public short Energy { get; set; }

        public int Health { get; set; }

        public short AP { get; set; }

        public short MP { get; set; }

        public short Vitality { get; set; }

        public short Wisdom { get; set; }

        public short Strength { get; set; }

        public short Intelligence { get; set; }

        public short Chance { get; set; }

        public short Agility { get; set; }

        public short PermanentVitality { get; set; }

        public short PermanentWisdom { get; set; }

        public short PermanentStrength { get; set; }

        public short PermanentIntelligence { get; set; }

        public short PermanentChance { get; set; }

        public short PermanentAgility { get; set; }

        public DateTime? LastSelection { get; set; }

        public int? SpouseId { get; set; }

        public int? EquipedMountId { get; set; }

        public sbyte MountXpPercent { get; set; }
        
        public bool IsRiding { get; set; }

        [Ignore]
        public CharacterBaseInformations CharacterBaseInformations =>
            new(Id, Name!, ExperienceManager.Instance.GetCharacterLevel(Experience), Look.GetEntityLook(), (sbyte)Breed, Sex);
    }
}
