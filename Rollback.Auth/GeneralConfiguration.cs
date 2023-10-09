using Rollback.Common.Config;
using Rollback.Common.DesignPattern.Instance;

namespace Rollback.Auth
{
    [Config("General")]
    public sealed class GeneralConfiguration : Singleton<GeneralConfiguration>
    {
        public Protocol.Types.Version Version { get; set; } = new Protocol.Types.Version(2, 0, 0, 0);

        public byte MaxCharactersByAccount { get; set; } = 5;
    }
}
