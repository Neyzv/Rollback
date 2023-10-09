using Rollback.Common.ORM;
using Rollback.World.CustomEnums;

namespace Rollback.World.Database.Pets
{
    public static class PetRelator
    {
        public const string GetRecords = "SELECT * FROM pets";
    }

    [Table("pets")]
    public sealed record PetRecord
    {
        [Key]
        public short PetId { get; set; }

        public byte MaxLifePoints { get; set; }

        public short GhostId { get; set; }

        public short? CertificateId { get; set; }

        public byte? MinMealHours { get; set; }

        public byte? MaxMealHours { get; set; }

        public short? BoostItemId { get; set; }

        [Ignore]
        public Dictionary<EffectId, PetFoodRecord> FoodInformations { get; set; } = new();

        [Ignore]
        public short? MaxEffectPod { get; set; }

        [Ignore]
        public short? MaxEffectPodBoosted { get; set; }
    }
}
