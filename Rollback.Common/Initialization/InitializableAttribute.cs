namespace Rollback.Common.Initialization
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InitializableAttribute : Attribute
    {
        public InitializationPriority Priority { get; set; }

        public string? Name { get; set; }

        public bool Hidden =>
            Name is null;

        public InitializableAttribute(InitializationPriority priority, string? name = default)
        {
            Priority = priority;
            Name = name;
        }
    }
}
