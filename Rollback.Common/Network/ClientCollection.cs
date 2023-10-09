namespace Rollback.Common.Network
{
    public abstract class ClientCollection<TClient, TMessage> : IClientCollection<TClient, TMessage>
        where TClient : Client<TMessage>
        where TMessage : IMessage
    {
        protected abstract IEnumerable<TClient> GetClients();

        public void Send(Delegate d, object[]? parameters = default) =>
            IClientCollection<TClient, TMessage>.Send(GetClients(), d, parameters);

        public void Send(Predicate<TClient> p, Delegate d, object[]? parameters = default) =>
            IClientCollection<TClient, TMessage>.Send(GetClients().Where(x => p(x)), d, parameters);
    }
}
