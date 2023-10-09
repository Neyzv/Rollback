using Rollback.Common.Commands.Types;

namespace Rollback.Common.Commands.Commands
{
    public sealed class HelpCommand : BaseCommand
    {
        public override string[] Aliases =>
            new[] { "help" };

        public override string Description =>
            "This command give you the list of all commands who are available.";

        public override byte Role =>
            0;

        private static void ShowCommandParams(ICommandUser sender, BaseCommand command, int level = 1)
        {
            if (command.ParametersCount > 0)
            {
                for (ushort i = 0; i < command.ParametersCount; i++)
                {
                    var parameter = command.GetParameter(i)!;
                    sender.Reply($"{new string('-', level + 1)}> {parameter.Description} | Optional : {parameter.Optional}{(parameter.Value is not null ? $" | Default value : {parameter.Value}" : string.Empty)}");
                }
            }
        }

        private void ShowSubCommandInfos(ICommandUser sender, CommandContainer cmdContainer, int level = 1)
        {
            if (CommandManager.Instance.SubCommands.TryGetValue(cmdContainer.GetType(), out var subCommands))
                foreach (var subCommand in subCommands.Select(x => x()))
                {
                    sender.Reply($"{new string('=', level)}> {sender.ToBold(string.Join(", ", subCommand.Aliases))} - {subCommand.Description}");
                    ShowCommandParams(sender, cmdContainer, level);

                    if (subCommand.GetType().IsSubclassOf(CommandManager.CmdContainerType))
                        ShowSubCommandInfos(sender, cmdContainer, level + 1);
                }
        }

        public override void Execute(ICommandUser sender)
        {
            sender.Reply(new('-', 30));

            foreach (var command in CommandManager.Instance.Commands.Values.Select(x => x()).Where(x => x.Role <= sender.Role))
            {
                sender.Reply($"{sender.ToBold(string.Join(", ", command.Aliases))} - {command.Description}");

                ShowCommandParams(sender, command);

                //Sub Commands using recusivity
                if (command is CommandContainer cmdContainer)
                    ShowSubCommandInfos(sender, cmdContainer);

                sender.Reply(new('-', 30));
            }
        }
    }
}
