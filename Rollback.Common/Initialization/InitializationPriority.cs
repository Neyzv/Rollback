namespace Rollback.Common.Initialization
{
    public enum InitializationPriority
    {
        Console,
        Config,
        Database,
        IPCClient,
        DatasManager,
        DependantDatasManager,
        LowLevelDatasManager,
        ScheduledTasks,
        IPCServer,
        Network
    }
}
