using Rollback.Common.Config;
using Rollback.Common.DesignPattern.Instance;

namespace Rollback.World
{
    [Config("General")]
    public sealed class GeneralConfiguration : Singleton<GeneralConfiguration>
    {
        public byte CharacterStartLevel { get; set; } = 1;

        public int CharacterStartKamas { get; set; } = 0;

        public byte RegenSpeed { get; set; } = 1;

        public byte XPRate { get; set; } = 1;

        public byte KamasRate { get; set; } = 1;

        public byte DropRate { get; set; } = 1;

        public byte JobRate { get; set; } = 1;

        public string HelloWorldMessage { get; set; } = "Bienvenu sur l'émulateur Rollback 2.0.0 !";

        public string HelloWorldColor { get; set; } = "#22b6f5";

        public int TimeBetweenSalesMessage { get; set; } = 90_000;

        public int TimeBetweenSeekMessage { get; set; } = 90_000;

        public int AutoSaveInterval { get; set; } = 1_800_000;

        public int MinAutoMoveInterval { get; set; } = 30_000;

        public int MaxAutoMoveInterval { get; set; } = 120_000;

        public int AutoMoveDisableInterval { get; set; } = 600_000;
    }
}
