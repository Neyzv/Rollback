using Rollback.Common.ORM;

namespace Rollback.World.Database.Jobs
{
    public static class RecipeRelator
    {
        public const string GetRecipes = "SELECT * FROM recipes_records";
    }

    [Table("recipes_records")]
    public sealed record RecipeRecord
    {
        [Key]
        public short ItemId { get; set; }

        public string IngredientsCSV { get; set; } = string.Empty;
    }
}
