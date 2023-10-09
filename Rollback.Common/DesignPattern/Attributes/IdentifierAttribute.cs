namespace Rollback.Common.DesignPattern.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class IdentifierAttribute : Attribute
    {
        public object Identifier { get; set; }

        public IdentifierAttribute(object identifier) =>
            Identifier = identifier;
    }
}
