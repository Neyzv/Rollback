using Rollback.Common.Commands;
using Rollback.Common.Commands.Types;
using Rollback.Protocol.Enums;
using Rollback.World.Network;

namespace Rollback.World.Commands.Commands
{
    public sealed class ShutdownCommand : CommandContainer
    {
        public override string[] Aliases =>
            new[] { "shutdown", "stop", "s" };

        public override string Description =>
            "Command to manage the shutdown of the server.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;
    }

    public sealed class ShutdownSecondCommand : SubCommand
    {
        public override Type ParentCommand =>
            typeof(ShutdownCommand);

        public override string[] Aliases =>
            new[] { "second", "s" };

        public override string Description =>
            "Command to shutdown the server in second.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        public ShutdownSecondCommand() =>
            AddParameter("Time before shutdown in second.");

        public override void Execute(ICommandUser sender) =>
            WorldServer.Instance.Shutdown(TimeSpan.FromSeconds(GetParameterValue<uint>(0)));
    }

    public sealed class ShutdownMinutesCommand : SubCommand
    {
        public override Type ParentCommand =>
            typeof(ShutdownCommand);

        public override string[] Aliases =>
            new[] { "minutes", "m" };

        public override string Description =>
            "Command to shutdown the server in minutes.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        public ShutdownMinutesCommand() =>
            AddParameter("Time before shutdown in minutes.");

        public override void Execute(ICommandUser sender) =>
            WorldServer.Instance.Shutdown(TimeSpan.FromMinutes(GetParameterValue<uint>(0)));
    }

    public sealed class ShutdownHoursCommand : SubCommand
    {
        public override Type ParentCommand =>
            typeof(ShutdownCommand);

        public override string[] Aliases =>
            new[] { "hours", "h" };

        public override string Description =>
            "Command to shutdown the server in hours.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        public ShutdownHoursCommand() =>
            AddParameter("Time before shutdown in hours.");

        public override void Execute(ICommandUser sender) =>
            WorldServer.Instance.Shutdown(TimeSpan.FromHours(GetParameterValue<uint>(0)));
    }

    public sealed class ShutdownDaysCommand : SubCommand
    {
        public override Type ParentCommand =>
            typeof(ShutdownCommand);

        public override string[] Aliases =>
            new[] { "days", "d" };

        public override string Description =>
            "Command to shutdown the server in hours.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        public ShutdownDaysCommand() =>
            AddParameter("Time before shutdown in days.");

        public override void Execute(ICommandUser sender) =>
            WorldServer.Instance.Shutdown(TimeSpan.FromDays(GetParameterValue<uint>(0)));
    }

    public sealed class ShutdownCancelCommand : SubCommand
    {
        public override Type ParentCommand =>
            typeof(ShutdownCommand);

        public override string[] Aliases =>
            new[] { "cancel", "c" };

        public override string Description =>
            "Command to cancel the shutdown of the server.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        public override void Execute(ICommandUser sender) =>
            WorldServer.Instance.CancelShutdown();
    }
}
