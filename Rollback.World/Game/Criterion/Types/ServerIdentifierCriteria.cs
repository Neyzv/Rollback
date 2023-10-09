using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Extensions;
using Rollback.World.Game.Criterion.Enums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Network.IPC.Config;

namespace Rollback.World.Game.Criterion.Types
{
    [Identifier("SI")]
    public sealed class ServerIdentifierCriteria : BaseCriteria
    {
        private int? _serverId;
        public int ServerId =>
            _serverId ??= Value.ChangeType<int>();

        public ServerIdentifierCriteria(string identifier, Comparator comparator, string value, Operator op) : base(identifier, comparator, value, op) { }

        public override bool Eval(Character character) =>
            Compare(IPCServiceConfiguration.Instance.WorldId, ServerId);
    }
}
