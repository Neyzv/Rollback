namespace Rollback.Common.DesignPattern.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class HandlerAttribute : Attribute
    {
        public uint MessageId { get; set; }

        public HandlerAttribute(uint messageId) =>
            MessageId = messageId;

    }
}
