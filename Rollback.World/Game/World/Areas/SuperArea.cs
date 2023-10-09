using Rollback.Common.Network;
using Rollback.Common.Network.Protocol;
using Rollback.World.Database.World;
using Rollback.World.Network;

namespace Rollback.World.Game.World.Areas
{
    public sealed class SuperArea : ClientCollection<WorldClient, Message>
    {
        private readonly SuperAreaRecord _record;

        public short Id =>
            _record.Id;

        public SuperArea(SuperAreaRecord record) =>
            _record = record;

        protected override IEnumerable<WorldClient> GetClients() =>
            WorldServer.Instance.GetClients(x => x.Account?.Character?.MapInstance?.Map.SubArea?.Area.SuperArea.Id == Id);
    }
}
