using Rollback.Common.ORM;

namespace Rollback.World.Database.Effects
{
    public static class EffectRelator
    {
        public const string GetEffects = "SELECT * FROM effects";
    }

    [Table("effects")]
    public sealed record EffectRecord
    {
        [Key]
        public int Id { get; set; }

        public int Characteristic { get; set; }

        public string? Operator { get; set; }
    }
}
