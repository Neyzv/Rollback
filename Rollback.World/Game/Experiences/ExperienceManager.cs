using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.ORM;
using Rollback.World.Database.Experiences;

namespace Rollback.World.Game.Experiences
{
    internal sealed class ExperienceManager : Singleton<ExperienceManager>
    {
        private static readonly IReadOnlyCollection<double[]> _xpGaps = new double[][]
        {
            new double[] {0, 10},
            new double[] {10, 8},
            new double[] {20, 6},
            new double[] {30, 4},
            new double[] {40, 3},
            new double[] {50, 2},
            new double[] {60, 1.5},
            new double[] {70, 1}
        };
        
        private readonly List<ExperienceRecord> _floors;

        public byte MaxCharacterLevel { get; private set; }
        public byte MaxAlignmentLevel { get; private set; }
        public byte MaxGuildLevel { get; private set; }
        public byte MaxJobLevel { get; private set; }
        public byte MaxMountLevel { get; private set; }

        public ExperienceManager() =>
            _floors = new();

        [Initializable(InitializationPriority.DatasManager, "Experience's floors")]
        public void Initialize()
        {
            foreach (var floor in DatabaseAccessor.Instance.Select<ExperienceRecord>(ExperienceRelator.GetFloors))
                _floors.Add(floor);

            // CONSTANTS
            MaxAlignmentLevel = _floors.MaxBy(x => x.Alignments) is { } lastAlignmentLevel ?
                lastAlignmentLevel.Level : (byte)1;
            MaxCharacterLevel = _floors.MaxBy(x => x.Characters) is { } lastCharacterLevel ?
                lastCharacterLevel.Level : (byte)1;
            MaxGuildLevel = _floors.MaxBy(x => x.Guilds) is { } lastGuildLevel ?
                lastGuildLevel.Level : (byte)1;
            MaxJobLevel = _floors.MaxBy(x => x.Jobs) is { } lastJobLevel ?
                lastJobLevel.Level : (byte)1;
            MaxMountLevel = _floors.MaxBy(x => x.Mounts) is { } lastMountLevel ?
                lastMountLevel.Level : (byte)1;
        }        
        

        public static double GetXpGap(short levelDifference) =>
            _xpGaps.First(x => x[0] <= levelDifference)[1];

        #region Characters
        public byte GetCharacterLevel(long experience) =>
            (byte)(_floors.FirstOrDefault(x => x.Characters > experience) is { } floor ? floor.Level - 1 : MaxCharacterLevel);

        public long GetCharacterExperienceForLevel(byte level) =>
            _floors.FirstOrDefault(x => x.Level == level) is { } floor && floor.Characters >= 0 ? floor.Characters : 0;

        public long GetCharacterUpperExperienceLevelFloor(long experience) =>
            _floors.FirstOrDefault(x => x.Characters > experience) is { } floor ? floor.Characters : 0;

        public long GetCharacterLowerExperienceLevelFloor(long experience) =>
            _floors.LastOrDefault(x => x.Characters <= experience && x.Characters > 0) is { } floor ? floor.Characters : 0;
        #endregion

        #region Alignments
        public sbyte GetGrade(ushort honor) =>
            (sbyte)(_floors.FirstOrDefault(x => x.Alignments > honor) is { } floor ? floor.Level - 1 : MaxAlignmentLevel);
        #endregion

        #region Guilds
        public byte GetGuildLevel(long experience) =>
            (byte)(_floors.FirstOrDefault(x => x.Guilds > experience) is { } floor ? floor.Level - 1 : MaxGuildLevel);

        public long GetGuildUpperExperienceLevelFloor(long experience) =>
            _floors.FirstOrDefault(x => x.Guilds > experience) is { } floor ? floor.Guilds : 0;

        public long GetGuildLowerExperienceLevelFloor(long experience) =>
            _floors.LastOrDefault(x => x.Guilds <= experience && x.Guilds > 0) is { } floor ? floor.Guilds : 0;
        #endregion

        #region Jobs
        public sbyte GetJobLevel(long experience) =>
            (sbyte)(_floors.FirstOrDefault(x => x.Jobs > experience) is { } floor ? floor.Level - 1 : MaxJobLevel);

        public long GetJobUpperExperienceLevelFloor(long experience) =>
            _floors.FirstOrDefault(x => x.Jobs > experience) is { } floor ? floor.Jobs : 0;

        public long GetJobLowerExperienceLevelFloor(long experience) =>
            _floors.LastOrDefault(x => x.Jobs <= experience && x.Jobs > 0) is { } floor ? floor.Jobs : 0;
        #endregion

        #region Mounts
        public sbyte GetMountLevel(long experience) =>
            (sbyte)(_floors.FirstOrDefault(x => x.Mounts > experience) is { } floor ? floor.Level - 1 : MaxMountLevel);

        public long GetMountUpperExperienceLevelFloor(long experience) =>
            _floors.FirstOrDefault(x => x.Mounts > experience) is { } floor ? floor.Mounts : 0;

        public long GetMountLowerExperienceLevelFloor(long experience) =>
            _floors.LastOrDefault(x => x.Mounts <= experience && x.Mounts > 0) is { } floor ? floor.Mounts : 0;
        #endregion
    }
}
