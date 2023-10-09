using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.Protocol.Enums;
using Rollback.World.Database.Breeds;

namespace Rollback.World.Game.RolePlayActors.Characters.Breeds
{
    public class BreedManager : Singleton<BreedManager>
    {

        private static readonly Dictionary<BreedEnum, KeyValuePair<int, short>> _statueMap = new()
        {
            { BreedEnum.Sram, new KeyValuePair<int, short>(1814, 384) },
            { BreedEnum.Eniripsa, new KeyValuePair<int, short>(789, 313) },
            { BreedEnum.Cra, new KeyValuePair<int, short>(1300, 342) },
            { BreedEnum.Enutrof, new KeyValuePair<int, short>(3348, 301) },
            { BreedEnum.Sadida, new KeyValuePair<int, short>(1811, 372) },
            { BreedEnum.Iop, new KeyValuePair<int, short>(2835, 372) },
            { BreedEnum.Xelor, new KeyValuePair<int, short>(275, 384) },
            { BreedEnum.Osamodas, new KeyValuePair<int, short>(131857, 356) },
            { BreedEnum.Pandawa, new KeyValuePair<int, short>(3857, 340) },
            { BreedEnum.Ecaflip, new KeyValuePair<int, short>(3344, 303) },
            { BreedEnum.Feca, new KeyValuePair<int, short>(1808, 331) },
            { BreedEnum.Sacrieur, new KeyValuePair<int, short>(131854, 259) },
        };

        private readonly Dictionary<int, BreedRecord> _breeds;

        public BreedManager() =>
            _breeds = new();

        [Initializable(InitializationPriority.DatasManager, "Breeds")]
        public void Initialize()
        {
            foreach (var breed in DatabaseAccessor.Instance.Select<BreedRecord>(BreedRelator.GetBreeds))
                _breeds[breed.Id] = breed;

            foreach (var spell in DatabaseAccessor.Instance.Select<BreedSpellRecord>(BreedSpellRelator.GetBreedsSpells))
            {
                if (_breeds.ContainsKey(spell.BreedId))
                    _breeds[spell.BreedId].Spells.Add(spell);
            }
        }

        public static KeyValuePair<int, short>? GetStatueMapCellInfos(BreedEnum breed) =>
            _statueMap.ContainsKey(breed) ? _statueMap[breed] : default;

        public BreedRecord? GetBreedById(int breedId) =>
            _breeds.ContainsKey(breedId) ? _breeds[breedId] : default;

        public static Dictionary<short, KeyValuePair<byte, byte>> ParseBreedStats(string statsString)
        {
            Dictionary<short, KeyValuePair<byte, byte>> statsFloors = new();

            try
            {
                foreach (var floor in statsString.Split('|'))
                {
                    var splited = floor.Split(',');

                    byte amount = 1;
                    if (splited.Length > 2)
                        amount = Convert.ToByte(splited[2]);

                    statsFloors.Add(Convert.ToInt16(splited[0]), new KeyValuePair<byte, byte>(Convert.ToByte(splited[1]), amount));
                }
            }
            catch
            {
                Logger.Instance.LogError(msg: $"Error while parsing Breed Stats floors {statsString}...");
            }

            return statsFloors;
        }

        public static KeyValuePair<byte, byte> GetFloor(Dictionary<short, KeyValuePair<byte, byte>> floors, short statPoints)
        {
            foreach (var floor in floors.OrderByDescending(x => x.Key))
            {
                if (floor.Key <= statPoints)
                    return floor.Value;
            }

            return floors.Last().Value;
        }

        public static short AssignStatsPoints(Dictionary<short, KeyValuePair<byte, byte>> floors, short amount, short actual, out short surplus)
        {
            short res = 0;
            surplus = amount;

            var floor = GetFloor(floors, (short)(actual + res));

            while (surplus - floor.Key >= 0)
            {
                res += floor.Value;
                surplus -= floor.Key;

                floor = GetFloor(floors, (short)(actual + res));
            }

            return res;
        }
    }
}
