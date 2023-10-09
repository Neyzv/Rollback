using System.Drawing;
using System.Net.Sockets;
using Rollback.Common.DesignPattern.Threading.Schedul;
using Rollback.Common.DesignPattern.Threading.Schedul.Callback;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.Common.Network;
using Rollback.Common.Network.Config;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Enums;
using Rollback.World.Game.Guilds;
using Rollback.World.Game.Mounts;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.TaxCollectors;
using Rollback.World.Handlers.Basics;
using Rollback.World.Handlers.Connection;
using Rollback.World.Network.IPC.Config;
using Rollback.World.Network.IPC.Handlers.World;

namespace Rollback.World.Network
{
    internal class WorldServer : Server<WorldServer, WorldClient, Message, NetworkConfig>
    {
        private TimeSpan? _shutdownTime;
        private TimeSpan? _lastReportTime;
        private IExecution? _reportExecution;
        private IExecution? _shutdownExecution;

        public ServerStatusEnum Status { get; private set; }

        public WorldServer() : base() =>
            Stop += () =>
            {
                ChangeStatus(ServerStatusEnum.OFFLINE);

                if (Config.DebugMod)
                {
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            };

        [Initializable(InitializationPriority.Network, "Network")]
        public void Initialize() =>
            Start();

        protected override WorldClient CreateClient(Socket socket)
        {
            var client = new WorldClient(socket, CancellationToken);

            ConnectionHandler.SendHelloGameMessage(client);

            return client;
        }

        protected override bool CanAddClient(WorldClient client)
        {
            var isFull = _clients.Count < IPCServiceConfiguration.Instance.ServerCapacity;

            if (!isFull)
                ChangeStatus(ServerStatusEnum.FULL);

            return isFull && Status is ServerStatusEnum.ONLINE;
        }

        public void ChangeStatus(ServerStatusEnum status)
        {
            if (Status != status)
            {
                Status = status;
                IPCWorldHandler.SendWorldStateUpdateMessage(Status);
            }
        }

        public void SendAnnounce(string message, Predicate<Character>? p = default)
        {
            foreach (var client in _clients.Where(x => x.Account?.Character is not null && (p is null || p(x.Account.Character))))
                client.Account!.Character!.SendServerMessage($"<b>• Announce : {message}</b>", Color.OrangeRed);
        }

        private void AnnounceTimeBeforeShutDown(TimeSpan time) =>
            // Pour des raisons de maintenances, le serveur va être redémarré dans %1.
            Send(x => x.Account?.Character is not null, BasicHandler.SendTextInformationMessage,
                new object[] { TextInformationTypeEnum.TEXT_INFORMATION_ERROR, (short)15, new[] { (time.Days >= 1 ? time.ToString(@"dd\ d\") : time.ToString(@"hh\h\ mm\m\ ss\s")) } });

        private void ReportTimeBeforeShutDown()
        {
            if (_shutdownTime is not null)
            {
                var timeToReport = default(TimeSpan?);
                var now = DateTime.Now.TimeOfDay;
                var timeBeforeShutDown = _shutdownTime.Value.Subtract(now);

                if (timeBeforeShutDown.Days < 3)
                {
                    var announceDiff = _lastReportTime.HasValue ? now.Subtract(_lastReportTime.Value) : default(TimeSpan?);

                    if (timeBeforeShutDown.TotalDays > 1 && announceDiff is null or { TotalDays: >= 1 })
                        timeToReport = TimeSpan.FromDays(Math.Ceiling(timeBeforeShutDown.TotalDays));
                    else if (timeBeforeShutDown is { TotalDays: < 1, TotalHours: > 1 } && announceDiff is null or { TotalHours: >= 1 })
                        timeToReport = TimeSpan.FromHours(Math.Ceiling(timeBeforeShutDown.TotalHours));
                    else if ((timeBeforeShutDown is { TotalHours: < 1, TotalMinutes: > 30 } && Math.Ceiling(timeBeforeShutDown.TotalMinutes) % 5 is 0 && announceDiff is null or { TotalMinutes: >= 5 }) ||
                        (timeBeforeShutDown is { TotalMinutes: < 30 and > 10 } && announceDiff is null or { TotalMinutes: >= 2 }) ||
                        (timeBeforeShutDown is { TotalMinutes: < 10 and > 1 } && announceDiff is null or { TotalMinutes: >= 1 }))
                        timeToReport = TimeSpan.FromMinutes(Math.Ceiling(timeBeforeShutDown.TotalMinutes));
                    else if ((timeBeforeShutDown is { TotalMinutes: < 1, TotalSeconds: > 10 } && Math.Ceiling(timeBeforeShutDown.TotalSeconds) % 10 is 0 && announceDiff is null or { TotalSeconds: >= 10 }) ||
                        timeBeforeShutDown is { TotalSeconds: < 10 })
                        timeToReport = TimeSpan.FromSeconds(Math.Ceiling(timeBeforeShutDown.TotalSeconds));

                    if (timeToReport.HasValue)
                    {
                        _lastReportTime = now;
                        AnnounceTimeBeforeShutDown(timeToReport.Value);
                    }
                }
            }
        }

        public void SaveWorld()
        {
            Logger.Instance.LogInfo("World server saving...");

            ChangeStatus(ServerStatusEnum.SAVING);

            // Une sauvegarde du serveur est en cours... Vous pouvez continuer de jouer, mais l\'accès au serveur est temporairement bloqué.
            // La connexion sera de nouveau possible d\'ici quelques instants. Merci de votre patience.
            Send(x => x.Account?.Character is not null, BasicHandler.SendTextInformationMessage,
                new object[] { TextInformationTypeEnum.TEXT_INFORMATION_ERROR, (short)164, Array.Empty<string>() });

            GuildManager.Instance.Save();
            TaxCollectorManager.Instance.Save();
            MountManager.Instance.Save();

            ChangeStatus(ServerStatusEnum.ONLINE);

            // La sauvegarde du serveur est terminée. L'accès au serveur est de nouveau possible. Merci de votre compréhension.
            Send(x => x.Account?.Character is not null, BasicHandler.SendTextInformationMessage,
                new object[] { TextInformationTypeEnum.TEXT_INFORMATION_ERROR, (short)165, Array.Empty<string>() });
        }

        public void Shutdown(TimeSpan time)
        {
            _shutdownTime = DateTime.Now.TimeOfDay + time;

            ReportTimeBeforeShutDown();

            _reportExecution ??= Scheduler.Instance.ExecutePeriodically(ReportTimeBeforeShutDown)
                    .WithTime(TimeSpan.FromSeconds(1));

            (_shutdownExecution ??= Scheduler.Instance.ExecuteDelayed(async () =>
            {
                Scheduler.Instance.CancelExecutionPeriodically(_reportExecution);

                ChangeStatus(ServerStatusEnum.OFFLINE);

                Parallel.ForEach(_clients.ToArray(), (x) => x.Dispose());

                SaveWorld();

                GC.Collect();

                await Scheduler.Instance.StopAsync().ConfigureAwait(false);
                Dispose();
                Environment.Exit(0);
            })).WithTime(time);
        }

        public void CancelShutdown()
        {
            if (_shutdownExecution is not null)
            {
                SendAnnounce("Shutdown have been canceled...");

                Scheduler.Instance.CancelExecutionDelayed(_shutdownExecution);

                if (_reportExecution is not null)
                    Scheduler.Instance.CancelExecutionPeriodically(_reportExecution);
            }
        }
    }
}
