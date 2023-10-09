using Rollback.Auth.Database;
using Rollback.Auth.Handlers;
using Rollback.Auth.Network;
using Rollback.Common.DesignPattern.Enums;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;

namespace Rollback.Auth.Objects
{
    public class World
    {
        public WorldRecord Record { get; set; }

        public ServerStatusEnum State { get; set; }

        public string? IP { get; set; }

        public ushort? Port { get; set; }

        public World(WorldRecord record) =>
            (Record, State) = (record, ServerStatusEnum.STATUS_UNKNOWN);

        public GameServerInformations GetGameServerInformations(AccountRecord account) =>
            new((ushort)Record.Id, (sbyte)State, (sbyte)ServerCompletionEnum.CompletionRecomandated, CanAccess(account),
                (sbyte)(account.CharactersByWorld!.ContainsKey(Record.Id) ? account.CharactersByWorld[Record.Id].Count : 0));

        public bool CanAccess(AccountRecord account) =>
            State == ServerStatusEnum.ONLINE && account.Role >= Record.RequiredRole;

        public void ChangeState(ServerStatusEnum state)
        {
            State = state;

            AuthServer.Instance.Send(x => x.Account is not null, ConnectionHandler.SendServerStatusUpdateMessage, new[] { this });
        }
    }
}
