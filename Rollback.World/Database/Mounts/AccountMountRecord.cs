using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.CustomEnums;

namespace Rollback.World.Database.Mounts
{
    public static class CharacterMountRelator
    {
        public const string GetAllMounts = "SELECT * FROM accounts_mounts";
    }

    [Table("accounts_mounts")]
    public sealed record AccountMountRecord
    {
        [Key]
        public int Id { get; set; }

        public int AccountId { get; set; }

        public bool Sex { get; set; }

        public string Name { get; set; } = string.Empty;

        public short ModelId { get; set; }

        public long Experience { get; set; }

        public int Energy { get; set; }

        public int Tiredness { get; set; }

        public int Stamina { get; set; }

        public int Maturity { get; set; }

        public int Love { get; set; }

        public int Serinity { get; set; }

        public int ReproductionCount { get; set; }

        private string _ancestorsCSV = string.Empty;
        public string AncestorsCSV
        {
            get => _ancestorsCSV;
            set
            {
                _ancestorsCSV = value;

                if (!string.IsNullOrWhiteSpace(_ancestorsCSV))
                {
                    var splittedValue = _ancestorsCSV.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                    Ancestors = new int[splittedValue.Length];

                    for (var i = 0; i < splittedValue.Length; i++)
                        if (int.TryParse(splittedValue[i], out var ancestorId))
                            Ancestors[i] = ancestorId;
                        else
                            Logger.Instance.LogError(msg: $"Can not parse ancestors of mount {Id}...");
                }
            }
        }

        [Ignore]
        public int[] Ancestors { get; set; } = Array.Empty<int>();

        private string _behaviorsCSV = string.Empty;
        public string BehaviorsCSV
        {
            get => _behaviorsCSV;
            set
            {
                _behaviorsCSV = value;

                if (!string.IsNullOrWhiteSpace(_behaviorsCSV))
                {
                    var mountBehaviorType = typeof(MountBehavior);

                    foreach (var behavior in _behaviorsCSV.Split(","))
                    {
                        if (int.TryParse(behavior, out var behaviorId) && Enum.IsDefined(mountBehaviorType, behaviorId))
                            Behaviors.Add((MountBehavior)behaviorId);
                        else
                            Logger.Instance.LogError(msg: $"Can not parse behaviors of mount {Id}...");
                    }
                }
            }
        }

        [Ignore]
        public HashSet<MountBehavior> Behaviors { get; set; } = new();

        public int? PaddockMapId { get; set; }

        public bool IsInStable { get; set; }
    }
}
