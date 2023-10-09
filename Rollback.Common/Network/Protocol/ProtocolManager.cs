using System.Linq.Expressions;
using Rollback.Common.DesignPattern.Assemblies;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.Network.IPC;

namespace Rollback.Common.Network.Protocol
{
    public class ProtocolManager : Singleton<ProtocolManager>
    {
        private readonly Dictionary<int, Func<Message>> _messages = new();
        private readonly Dictionary<int, Func<object>> _types = new();
        private readonly Dictionary<int, Func<IPCMessage>> _ipcMessages = new();

        [Initializable(InitializationPriority.Database, "Protocol")]
        public void Initialize()
        {
            foreach (var type in from assembly in AssemblyManager.Instance.Assemblies
                                 from type in assembly.GetTypes()
                                 where type.Namespace is not null && type != typeof(IPCMessage) && (type.IsSubclassOf(typeof(Message)) || type.Namespace.Contains("Protocol.Types"))
                                 select type)
            {
                var fieldId = type.GetField("Id");
                if (fieldId is not null)
                {
                    int messId = Convert.ToInt32(fieldId.GetValue(type));

                    if (type.Namespace!.Contains("Protocol.Types"))
                    {
                        var createTypeFunc = Expression.Lambda<Func<object>>(Expression.New(type)).Compile();
                        if (!_types.TryAdd(messId, createTypeFunc))
                            throw new Exception($"Duplicate id {messId} for Type Registration ...");
                    }
                    else if (type.IsSubclassOf(typeof(IPCMessage)))
                    {
                        var createMessageFunc = Expression.Lambda<Func<IPCMessage>>(Expression.New(type)).Compile();
                        if (!_ipcMessages.TryAdd(messId, createMessageFunc))
                            throw new Exception($"Duplicate id {messId} for IPCMessage Registration ...");
                    }
                    else
                    {
                        var createMessageFunc = Expression.Lambda<Func<Message>>(Expression.New(type)).Compile();
                        if (!_messages.TryAdd(messId, createMessageFunc))
                            throw new Exception($"Duplicate id {messId} for Message Registration ...");
                    }
                }
                else
                    throw new Exception($"Can not find field Id for message or type {type.Name}");
            }
        }

        public bool TryGetMessage(int messId, out Message? msg)
        {
            msg = default;
            if (_messages.TryGetValue(messId, out Func<Message>? message))
                msg = message();

            return msg is not null;
        }

        public bool TryGetIPCMessage(int messId, out IPCMessage? msg)
        {
            msg = default;
            if (_ipcMessages.TryGetValue(messId, out Func<IPCMessage>? message))
                msg = message();

            return msg is not null;
        }

        public object GetType(int typeId)
        {
            _types.TryGetValue(typeId, out Func<object>? type);

            return type!();
        }
    }
}
