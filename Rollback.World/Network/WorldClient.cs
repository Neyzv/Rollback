using System.Net.Sockets;
using Rollback.Common.Logging;
using Rollback.Common.Network;
using Rollback.Common.Network.Protocol;
using Rollback.World.Game.Accounts;
using Rollback.World.Network.Handler;

namespace Rollback.World.Network
{
    public class WorldClient : DofusClient<Message>
    {
        public AccountInformations? Account { get; set; }

        public WorldClient(Socket socket, CancellationToken ct) : base(socket, ct)
        {
            if (WorldServer.Instance.Config.DebugMod)
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
            var (handler, attribute) = WorldHandlerManager.Instance.GetHandler(message.MessageId);
            if (handler is not null && attribute is not null)
            {
                if (!attribute.Connected || Account?.Character is not null)
                {
                    handler.DynamicInvoke(this, message);

                    if (WorldServer.Instance.Config.DebugMod)
                        Logger.Instance.LogInfo($"Message {message.GetType().Name} handled successfully !");
                }
                else
                {
                    Dispose();
                    Logger.Instance.LogWarn($"Force disconnection of client {this} : A character must be connected to handle message {message.GetType().Name}...");
                }
            }
            else
                Logger.Instance.LogWarn($"Can not find a handler for message {message.GetType().Name}...");
        }

        public void Ban(int banEndTime, string? announce = default)
        {
            //IPCAccountHandler.SendAccountBannedMessage(Account!.Id, banEndTime);

            //if (announce is not null)
            //    WorldServer.Instance.SendAnnounce(announce);

            //Dispose();
        }

        public void SafeBotBan() =>
            Ban(int.MaxValue, $"Player {Account!.Character!.Name} have been banned for using an unauthorized software. <i>(Safe Bot)</i>");

        public override void Save() =>
            Account?.Save();

        public override string ToString() =>
            $"{(Account?.Character is not null ? $"{Account.Character.Name}:" : string.Empty)}{IP}";
    }
}
