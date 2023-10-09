using Rollback.Common.Network;
using Rollback.Common.Network.Protocol;
using Rollback.World.Database.World;
using Rollback.World.Network;

namespace Rollback.World.Game.World.Areas
{
    public sealed class Area : ClientCollection<WorldClient, Message>
    {
        private readonly AreaRecord _record;

        public short Id =>
            _record.Id;

        public SuperArea SuperArea { get; }

        public Area(AreaRecord record, SuperArea superArea)
        {
            _record = record;
            SuperArea = superArea;
        }

        protected override IEnumerable<WorldClient> GetClients() =>
            WorldServer.Instance.GetClients(x => x.Account?.Character?.MapInstance?.Map.SubArea?.Area.Id == Id);
    }
}
