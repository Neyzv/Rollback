using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Rollback.Common.Commands.Types;
using Rollback.Common.DesignPattern.Assemblies;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;

namespace Rollback.Common.Commands
{
    public sealed class CommandManager : Singleton<CommandManager>
    {
        internal static readonly Type CmdContainerType;

        private readonly Regex _splitCommandsRegex = new(" (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)", RegexOptions.Compiled);
        private readonly Dictionary<Type, object> _converters;

        internal readonly Dictionary<string[], Func<BaseCommand>> Commands;
        internal readonly Dictionary<Type, List<Func<SubCommand>>> SubCommands;

        static CommandManager() =>
            CmdContainerType = typeof(CommandContainer);

        public CommandManager() =>
            (Commands, SubCommands, _converters) = (new(), new(), new());

        private void AddCommand(BaseCommand instance, Func<BaseCommand> instanceFunc, Type type)
        {
            foreach (var alias in instance!.Aliases)
                if (Commands.Keys.Any(x => x.Contains(alias)))
                    Logger.Instance.LogWarn($"Found two commands with alias {alias}...");

            if (!Commands.TryAdd(instance.Aliases, instanceFunc))
                throw new Exception($"Duplicate command aliases : {type.Name}");
        }

        private void AddSubCommand(SubCommand instance, Func<SubCommand> instanceFunc)
        {
            if (SubCommands.ContainsKey(instance.ParentCommand))
            {
                foreach (var subCommandF in SubCommands[instance.ParentCommand])
                {
                    var subInst = subCommandF();
                    foreach (var alias in instance!.Aliases)
                        if (subInst.Aliases.Contains(alias))
                            Logger.Instance.LogWarn($"Found two commands with alias {alias} for command container {instance.ParentCommand.Name}...");
                }

                SubCommands[instance.ParentCommand].Add(instanceFunc);
            }
            else
                SubCommands[instance.ParentCommand] = new() { instanceFunc };
        }

        [Initializable(InitializationPriority.Database, "Commands")]
        public void Initialize()
        {
            var subCommandType = typeof(SubCommand);
            var baseCommandType = typeof(BaseCommand);
            var argumentConverterType = typeof(IArgumentConverter<>);

            foreach (var type in from assembly in AssemblyManager.Instance.Assemblies
                                 from type in assembly.GetTypes()
                                 where !type.IsAbstract
                                 select type)
            {
                if (type.IsSubclassOf(subCommandType))
                {
                    var instanceFunc = Expression.Lambda<Func<SubCommand>>(Expression.New(type)).Compile();
                    var instance = instanceFunc();

                    if (instance.ParentCommand != CmdContainerType)
                        AddSubCommand(instance, instanceFunc);
                    else
                        AddCommand(instance, instanceFunc, type);
                }
                else if (type.IsSubclassOf(baseCommandType))
                {
                    var instanceFunc = Expression.Lambda<Func<BaseCommand>>(Expression.New(type)).Compile();
                    var instance = instanceFunc();

                    AddCommand(instance, instanceFunc, type);
                }
                else
                {
                    var argumentConverterInterface = type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == argumentConverterType);

                    if (argumentConverterInterface is not null)
                    {
                        var valueType = argumentConverterInterface.GetGenericArguments().FirstOrDefault();

                        if (valueType is not null)
                        {
                            var instanceFunc = Expression.Lambda<Func<object>>(Expression.New(type)).Compile();

                            _converters[valueType] = instanceFunc();
                        }
                    }
                }
            }
        }

        public IArgumentConverter<T>? GetConverter<T>() =>
            _converters.TryGetValue(typeof(T), out var converter) ? (IArgumentConverter<T>?)converter : default;

        public void HandleCommand(string commandText, ICommandUser sender)
        {
            try
            {
                var position = 0;
                var commandSplitted = _splitCommandsRegex.Split(commandText);

                if (commandSplitted.Length > 0)
                {
                    var commandFunc = Commands.FirstOrDefault(x => x.Key.Contains(commandSplitted[position]));
                    var command = default(BaseCommand);

                    if (commandFunc.Key is not null)
                        command = commandFunc.Value();

                    if (command is not null && command.Role <= sender.Role)
                    {
                        position++;
                        var type = command.GetType();
                        var oldCommand = default(BaseCommand);
                        var isCommandContainer = type.IsSubclassOf(CmdContainerType);

                        while (isCommandContainer && command != oldCommand && commandSplitted.Length > position && commandSplitted[position] != string.Empty)
                        {
                            oldCommand = command;

                            if (SubCommands.ContainsKey(type))
                                foreach (var subCommand in SubCommands[type].Select(x => x()).Where(x => x.Role <= sender.Role))
                                {
                                    if (subCommand.Aliases.Contains(commandSplitted[position]))
                                    {
                                        command = subCommand;
                                        type = subCommand.GetType();
                                        isCommandContainer = type.IsSubclassOf(CmdContainerType);
                                        position++;
                                        break;
                                    }
                                }
                        }

                        if (isCommandContainer && (commandSplitted.Length <= position || commandSplitted[position] == string.Empty))
                            sender.ReplyError($"Can not find the sub command alias, the command you're trying to use is a command container...");
                        else if (isCommandContainer && command == oldCommand)
                            sender.ReplyError($"Can not find a sub command with alias \"{sender.ToBold(commandSplitted[position])}\" for command {type.Name}...");
                        else
                        {
                            for (ushort i = 0; i < command.ParametersCount; i++)
                                if (position + i < commandSplitted.Length)
                                    command.SetValueToParameter(i, commandSplitted[position + i]);
                                else
                                {
                                    var parameter = command.GetParameter(i);
                                    if (parameter is not null && !parameter.Optional)
                                    {
                                        sender.ReplyError($"Missing parameter \"{sender.ToBold(parameter.Description)}\"...");
                                        return;
                                    }
                                }

                            command.Execute(sender);
                        }
                    }
                    else
                        sender.ReplyError($"Uknown command {commandSplitted[position]}...");
                }
                else
                    sender.ReplyError("Error while trying to parse command...");
            }
            catch (Exception e)
            {
                sender.ReplyError(e.Message);
            }
        }
    }
}
