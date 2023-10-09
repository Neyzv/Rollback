namespace Rollback.Common.Network
{
    public interface IClientCollection<TClient, TMessage>
        where TClient : Client<TMessage>
        where TMessage : IMessage
    {
        protected static void Send(IEnumerable<TClient> clients, Delegate d, object[]? parameters)
        {
            var additionalParametersCount = (parameters is null ? 0 : parameters.Length);
            if (d.Method.GetParameters().Length > additionalParametersCount + 1)
                throw new Exception($"Not enougth parameters to execute {d.Method.Name}...");

            var args = new object[additionalParametersCount + 1];
            parameters?.CopyTo(args, 1);

            foreach (var client in clients)
            {
                args[0] = client;
                d.DynamicInvoke(args);
            }
        }

        public void Send(Delegate d, object[]? parameters = default);

        public void Send(Predicate<TClient> p, Delegate d, object[]? parameters = default);
    }
}
