namespace Rollback.Common.Config
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigAttribute : Attribute
    {
        public string Name { get; set; }

        public ConfigAttribute(string name) =>
            Name = name;
    }
}
