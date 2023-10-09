using System.Diagnostics.CodeAnalysis;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects;

namespace Rollback.World.Database.Pets
{
    public static class PetFoodRelator
    {
        public const string GetFoodInformationsByPetId = "SELECT * FROM pets_foods WHERE PetId = {0}";
    }

    [Table("pets_foods")]
    public sealed record PetFoodRecord
    {
        [Key]
        public int Id { get; set; }

        public short PetId { get; set; }

        private byte[] _effectBin = Array.Empty<byte>();
        public byte[] EffectBin
        {
            get => _effectBin;
            set
            {
                _effectBin = value;

                if (_effectBin.Length > 0)
                    Effect = EffectManager.DeserializeEffects(_effectBin).First();
            }
        }

        [Ignore, NotNull]
        public EffectBase? Effect { get; set; }

        public short BoostedValue { get; set; }

        public FoodType FoodType { get; set; }

        private string _foodInformationsCSV = string.Empty;
        public string FoodInformationsCSV
        {
            get => _foodInformationsCSV;
            set
            {
                _foodInformationsCSV = value;

                if (!string.IsNullOrWhiteSpace(_foodInformationsCSV))
                {
                    foreach (var foodInfos in _foodInformationsCSV.Split(';'))
                    {
                        var foodInfo = foodInfos.Split(',');

                        short monsterCount = 0;
                        if (short.TryParse(foodInfo[0], out var foodId) &&
                            (FoodType is not FoodType.Monsters || (foodInfo.Length > 1 && short.TryParse(foodInfo[1], out monsterCount) && monsterCount > 0)))
                        {
                            FoodInformations[foodId] = monsterCount;
                        }
                        else
                            Logger.Instance.LogError(msg: $"Incorrect food informations {foodInfo} for pet {PetId}, Id : {Id}...");
                    }
                }
            }
        }

        [Ignore]
        public Dictionary<short, short> FoodInformations { get; set; } = new();

        public byte StatIncreaseAmount { get; set; }

        [Ignore]
        public short? EffectBasePod { get; set; }

        [Ignore]
        public short? EffectBasePodBoosted { get; set; }
    }
}
