using System.Diagnostics.CodeAnalysis;
using Rollback.Common.ORM;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Looks;

namespace Rollback.World.Database.Mounts
{
    public static class MountRelator
    {
        public const string GetAll = "SELECT * FROM mounts";
    }

    [Table("mounts")]
    public sealed record MountRecord
    {
        [Key]
        public short Id { get; set; }

        private string _lookString = string.Empty;
        public string LookString
        {
            get => _lookString;
            set
            {
                _lookString = value;

                if (!string.IsNullOrWhiteSpace(_lookString))
                    Look = ActorLook.Parse(_lookString);
            }
        }

        [Ignore, NotNull]
        public ActorLook? Look { get; set; }

        public short CertificateId { get; set; }

        public short BasePods { get; set; }

        public byte PodsPerLevel { get; set; }

        public short BaseEnergy { get; set; }

        public byte EnergyPerLevel { get; set; }

        public short MaturityForAdult { get; set; }

        public byte FecondationTime { get; set; }

        private byte[] _effectsBin = Array.Empty<byte>();
        public byte[] EffectsBin
        {
            get => _effectsBin;
            set
            {
                _effectsBin = value;

                if (_effectsBin.Length > 0)
                    Effects = EffectManager.DeserializeEffects(_effectsBin);
            }
        }

        [Ignore]
        public List<EffectBase> Effects { get; set; } = new();
    }
}
