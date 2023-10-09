using System.Reflection;
using Rollback.Common.DesignPattern.Assemblies;
using Rollback.Common.Extensions;
using Rollback.Common.Logging;

namespace Rollback.Common.Initialization
{
    public static class InitializableManager
    {
        public static void Initialize()
        {
            foreach (var (type, method, attribute) in from assembly in AssemblyManager.Instance.Assemblies
                                                      from type in assembly.GetTypes()
                                                      from method in type.GetMethods()
                                                      let attribute = method.GetCustomAttribute<InitializableAttribute>()
                                                      where attribute is not null
                                                      orderby attribute.Priority
                                                      select (type, method, attribute))
            {
                if (!attribute.Hidden)
                    Logger.Instance.LogInit(attribute.Name is null ? type.Name : attribute.Name);

                try
                {
                    if (method.IsStatic)
                        method.Invoke(null, null);
                    else
                    {
                        var instance = type.GetPropertyFromDerivedClass("Instance")?.GetValue(default);

                        if (instance is not null)
                            method.Invoke(instance, Array.Empty<object>());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Instance.LogError(ex);
                }
            }
        }
    }
}
