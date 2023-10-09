namespace Rollback.Common.Commands
{
    public class CommandParameter
    {
        public string Description { get; }

        public object? Value { get; set; }

        public bool Optional { get; }

        public CommandParameter(string description, object? value, bool optional)
        {
            Description = description;
            Value = value;
            Optional = optional || value is not null;
        }
    }
}
