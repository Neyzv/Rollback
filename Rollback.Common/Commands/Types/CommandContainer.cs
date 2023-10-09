namespace Rollback.Common.Commands.Types
{
    public abstract class CommandContainer : SubCommand
    {
        public override Type ParentCommand =>
            typeof(CommandContainer);

        public CommandContainer() : base() { }

        public override void Execute(ICommandUser sender) { }
    }
}
