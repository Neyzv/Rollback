using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Interactives;
using Rollback.World.Database.Jobs;
using Rollback.World.Game.Experiences;

namespace Rollback.World.Game.Jobs
{
    public sealed class JobManager : Singleton<JobManager>
    {
        public const JobIds BaseJobId = JobIds.Base;
        public const byte MaxJobCount = 3;
        public const byte PodsPerJobLevel = 12;
        public const int HarvestTime = 3000;
        public const byte MinBaseJobHarvest = 1;
        public const byte MaxBaseJobHarvest = 10;

        private readonly Dictionary<JobIds, JobTemplateRecord> _jobsTemplates;
        private readonly Dictionary<short, object> _recipes;

        public JobManager()
        {
            _jobsTemplates = new();
            _recipes = new();
        }

        [Initializable(InitializationPriority.DatasManager, "Jobs")]
        public void Initialize()
        {
            Logger.Instance.Log("\tLoading jobs templates...");
            foreach (var jobTemplate in DatabaseAccessor.Instance.Select<JobTemplateRecord>(JobTemplateRelator.QueryAll))
                _jobsTemplates[jobTemplate.Id] = jobTemplate;

            Logger.Instance.Log("\tLoading recipes...");
            foreach (var recipe in DatabaseAccessor.Instance.Select<RecipeRecord>(RecipeRelator.GetRecipes))
            {
                var workingDic = _recipes;
                var lastQuantitiesInfos = default(Dictionary<int, (object, short?)>?);
                var quantity = -1;

                foreach (var recipeInfos in recipe.IngredientsCSV.Split(';'))
                {
                    if (!string.IsNullOrEmpty(recipeInfos) && recipeInfos.Split(',') is { Length: > 1 } recipeI &&
                        short.TryParse(recipeI[0], out var itemId) && int.TryParse(recipeI[1], out quantity))
                    {
                        if (!workingDic.TryGetValue(itemId, out var infos))
                        {
                            infos = new Dictionary<int, (object, short?)>();
                            workingDic[itemId] = infos;
                        }

                        if (infos is Dictionary<int, (object, short?)> quantities)
                        {
                            if (!quantities.TryGetValue(quantity, out var val))
                            {
                                workingDic = new Dictionary<short, object>();
                                quantities.Add(quantity, (workingDic, default));
                            }
                            else if (val.Item1 is Dictionary<short, object> w)
                                workingDic = w;

                            lastQuantitiesInfos = quantities;
                        }
                    }
                    else
                        Logger.Instance.LogError(msg: $"Error while parsing ingredients of recipe {recipe.ItemId}...");
                }

                if (lastQuantitiesInfos is not null && lastQuantitiesInfos.ContainsKey(quantity))
                    lastQuantitiesInfos[quantity] = (lastQuantitiesInfos[quantity].Item1, recipe.ItemId);
            }
        }

        public JobTemplateRecord? GetRecordById(JobIds id) =>
            _jobsTemplates.ContainsKey(id) ? _jobsTemplates[id] : default;

        public static (short, short) GetHarvestItemMinMax(sbyte jobLevel, InteractiveSkillTemplateRecord skillTemplate) =>
            ((short, short))(skillTemplate.ParentJobId <= BaseJobId ? (MinBaseJobHarvest, MaxBaseJobHarvest) :
            skillTemplate.MinJobLevel > jobLevel ? (default, default)
            : skillTemplate.MinJobLevel >= ExperienceManager.Instance.MaxJobLevel ? (1, 1)
            : (Math.Max(1, jobLevel / 20), Math.Floor((jobLevel - skillTemplate.MinJobLevel) / 10d + 3)));

        public static sbyte GetCraftSuccessPercentage(sbyte jobLevel) =>
            (sbyte)(50 + Math.Floor(jobLevel / 10d) * 5);

        public static sbyte GetCraftMaxSlotsCount(sbyte jobLevel) =>
            (sbyte)(jobLevel < 10 ? 2 : Math.Floor(jobLevel / 20d) + 3);

        public static short GetHarvestJobXp(sbyte minLevel) =>
            (short)(Math.Floor(5 + minLevel / 10d) * GeneralConfiguration.Instance.JobRate);

        public static short GetCraftJobXp(sbyte jobLevel, int slotsCount) =>
            (short)((slotsCount switch
            {
                2 => jobLevel < 60 ? 10 : default,
                3 => jobLevel is < 80 and > 9 ? 25 : default,
                4 => jobLevel > 19 ? 50 : default,
                5 => jobLevel > 39 ? 100 : default,
                6 => jobLevel > 59 ? 250 : default,
                7 => jobLevel > 79 ? 500 : default,
                8 => jobLevel > 99 ? 1000 : default,
                _ => jobLevel < 40 ? 1 : default
            }) * GeneralConfiguration.Instance.JobRate);

        public short? GetRecipeResult((short, int)[] ingredientsInfos)
        {
            var result = default(short?);
            var workingDic = _recipes;

            foreach (var ingredient in ingredientsInfos)
            {
                if (workingDic.ContainsKey(ingredient.Item1) &&
                    workingDic[ingredient.Item1] is Dictionary<int, (object, short?)> quantities &&
                    quantities.ContainsKey(ingredient.Item2) && quantities[ingredient.Item2].Item1 is Dictionary<short, object> nextIngredientInfos)
                {
                    result = quantities[ingredient.Item2].Item2;
                    workingDic = nextIngredientInfos;
                }
                else
                {
                    result = default;
                    break;
                }
            }

            return result;
        }
    }
}
