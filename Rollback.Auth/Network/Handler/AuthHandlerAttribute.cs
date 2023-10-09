using Rollback.Common.DesignPattern.Attributes;

namespace Rollback.Auth.Network.Handler
{
    internal class AuthHandlerAttribute : HandlerAttribute
    {
        public bool NeedAccount { get; set; }

        public AuthHandlerAttribute(uint messageId, bool needAccount = true) : base(messageId) =>
            NeedAccount = needAccount;
    }
}
