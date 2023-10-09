using System.Net.Sockets;
using Rollback.Auth.Database;
using Rollback.Auth.Network.Handler;
using Rollback.Common.Logging;
using Rollback.Common.Network;
using Rollback.Common.Network.Protocol;
using Rollback.Common.ORM;

namespace Rollback.Auth.Network
{
    public sealed class AuthClient : DofusClient<Message>
    {
        public string? Ticket { get; set; }

        public AccountRecord? Account { get; set; }

        public AuthClient(Socket socket, CancellationToken ct) : base(socket, ct)
        {
            if (AuthServer.Instance.Config.DebugMod)
            {
                MessageSend += OnMessageSend;
                MessageReceived += OnMessageReceived;
            }
        }

        private void OnMessageReceived(Message message) =>
            Logger.Instance.LogReceive(this, message);

        private void OnMessageSend(Message message) =>
            Logger.Instance.LogSend(this, message);

        protected override bool GetMessage(int msgId, out Message? message) =>
            ProtocolManager.Instance.TryGetMessage(msgId, out message);

        protected override void ExecuteHandler(Message message)
        {
            var (handler, attribute) = AuthHandlerManager.Instance.GetHandler(message.MessageId);
            if (handler is not null && attribute is not null)
            {
                if (!attribute.NeedAccount || Account is not null)
                {
                    handler.DynamicInvoke(this, message);

                    if (AuthServer.Instance.Config.DebugMod)
                        Logger.Instance.LogInfo($"Message {message.GetType().Name} handled successfully !");
                }
                else
                {
                    Dispose();
                    Logger.Instance.LogWarn($"Force disconnection of client {this} : Can not handle message {message.GetType().Name} with out an account linked to the account");
                }
            }
            else
                Logger.Instance.LogWarn($"Can not find a handler for message {message.GetType().Name}...");
        }

        public override void Save()
        {
            if (Account is not null)
                DatabaseAccessor.Instance.Update(Account);
        }

        public override string ToString() =>
            $"{(Account is not null ? $"{Account.Nickname}:" : string.Empty)}{IP}";
    }
}
