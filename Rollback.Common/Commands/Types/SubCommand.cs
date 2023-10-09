namespace Rollback.Common.Commands.Types
{
    public abstract class SubCommand : BaseCommand
    {
        public abstract Type ParentCommand { get; }

        public SubCommand() : base() { }
    }
}
