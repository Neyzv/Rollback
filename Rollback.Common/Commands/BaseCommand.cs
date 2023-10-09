using Rollback.Common.Extensions;

namespace Rollback.Common.Commands
{
    public abstract class BaseCommand
    {
        private readonly List<CommandParameter> _parameters;

        public abstract string[] Aliases { get; }

        public abstract string Description { get; }

        public abstract byte Role { get; }

        public int ParametersCount =>
            _parameters.Count;

        public BaseCommand() =>
            _parameters = new();

        public void AddParameter(string description, object? defaultValue = default, bool optional = false) =>
            _parameters.Add(new(description, defaultValue, optional));

        public void SetValueToParameter(ushort index, object value)
        {
            var parameter = _parameters.ElementAtOrDefault(index);

            if (parameter is not null)
                parameter.Value = value;
        }

        public CommandParameter? GetParameter(ushort index) =>
            _parameters.ElementAtOrDefault(index);

        public T? GetParameterValue<T>(ushort index)
        {
            var converter = CommandManager.Instance.GetConverter<T>();
            var parameterValue = GetParameter(index)!.Value;

            return converter is null ? parameterValue.ChangeType<T>() : parameterValue is null ? default : converter.Convert(parameterValue);
        }

        public abstract void Execute(ICommandUser sender);
    }
}
