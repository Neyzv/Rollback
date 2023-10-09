using System.Reflection;
using Rollback.Common.DesignPattern.Instance;

namespace Rollback.Common.DesignPattern.Assemblies
{
    public class AssemblyManager : Singleton<AssemblyManager>
    {
        public Assembly[] Assemblies { get; }

        public AssemblyManager() =>
            Assemblies = RegisterAssemblies();

        private static Assembly[] RegisterAssemblies()
        {
            var assemblies = new List<Assembly>();
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly is not null)
            {
                assemblies.Add(entryAssembly);

                foreach (var assemblyName in entryAssembly.GetReferencedAssemblies())
                    assemblies.Add(Assembly.Load(assemblyName));
            }

            return assemblies.ToArray();
        }
    }
}
