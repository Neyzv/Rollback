using System.Linq.Expressions;
using System.Reflection;
using Rollback.Common.DesignPattern.Assemblies;
using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Logging;

namespace Rollback.Common.Network.Handler
{
    public abstract class HandlerManager<TInstance, TAttribute> : Singleton<TInstance>
        where TInstance : HandlerManager<TInstance, TAttribute>
        where TAttribute : HandlerAttribute
    {
        protected readonly Dictionary<uint, (Delegate, TAttribute)> _handlers = new();

        protected void Initialize()
        {
            foreach (var type in from assembly in AssemblyManager.Instance.Assemblies
                                 from type in assembly.GetTypes()
                                 select type)
            {
                foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
                {
                    var handlerAttribute = method.GetCustomAttribute<TAttribute>();
                    if (handlerAttribute is not null)
                    {
                        if (_handlers.ContainsKey(handlerAttribute.MessageId))
                            Logger.Instance.LogWarn($"Two handler have been found for message {handlerAttribute.MessageId}.");

                        var handlerFunction = method.CreateDelegate(Expression.GetDelegateType(
                            (from parameter in method.GetParameters() select parameter.ParameterType)
                            .Concat(new[] { method.ReturnType })
                            .ToArray()));

                        _handlers[handlerAttribute.MessageId] = (handlerFunction, handlerAttribute);
                    }
                }
            }
        }

        public abstract (Delegate, TAttribute) GetHandler(uint messId);
    }
}
