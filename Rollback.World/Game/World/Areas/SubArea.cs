using Rollback.Common.Network;
using Rollback.Common.Network.Protocol;
using Rollback.World.Database.World;
using Rollback.World.Network;

namespace Rollback.World.Game.World.Areas
{
    public sealed class SubArea : ClientCollection<WorldClient, Message>
    {
        private readonly SubAreaRecord _record;

        public short Id =>
            _record.Id;

        public Area Area { get; }

        public SubArea(SubAreaRecord record, Area area)
        {
            _record = record;
            Area = area;
        }

        protected override IEnumerable<WorldClient> GetClients() =>
            WorldServer.Instance.GetClients(x => x.Account?.Character?.MapInstance?.Map.SubArea?.Id == Id);
    }
}
