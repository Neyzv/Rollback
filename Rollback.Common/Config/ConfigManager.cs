using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Rollback.Common.DesignPattern.Assemblies;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;

namespace Rollback.Common.Config
{
    public class ConfigManager : Singleton<ConfigManager>
    {
        private IConfiguration? Config { get; set; }

        [Initializable(InitializationPriority.Config, "Config")]
        public void Initialize()
        {
            CreateConfig();

            var configBuilder = new ConfigurationBuilder();
            Config = configBuilder.SetBasePath($"{Directory.GetCurrentDirectory()}/config").AddJsonFile("config.json", false, true).Build();
        }

        private static void CreateConfig()
        {
            var path = "./config/config.json";

            if (!File.Exists(path))
            {
                Logger.Instance.LogInfo("Creation of the config file...");

                if (!Directory.Exists("./config/"))
                    Directory.CreateDirectory("./config/");

                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true,
                };

                var storage = (from assembly in AssemblyManager.Instance.Assemblies
                               from type in assembly.GetTypes()
                               let attribute = type.GetCustomAttribute<ConfigAttribute>()
                               where attribute is not null
                               select (type, attribute)).ToArray();

                var builder = new StringBuilder();
                builder.AppendLine("{");

                for (var i = 0; i < storage.Length; i++)
                {
                    var (type, attribute) = storage[i];
                    var instance = Activator.CreateInstance(type);

                    builder.AppendLine($"\t\"{attribute.Name}\":").AppendLine($"\t{JsonSerializer.Serialize(instance, options).Replace("\n", "\n\t")}");

                    if (i != storage.Length - 1)
                        builder.AppendLine("\t,");
                }
                builder.AppendLine("}");

                File.WriteAllText(path, builder.ToString());
            }
        }

        public T Get<T>()
        {
            if (Config is null)
                throw new ObjectDisposedException(nameof(Config));

            var type = typeof(T);
            var attribute = type.GetCustomAttribute<ConfigAttribute>();
            return attribute is null
                ? throw new Exception($"Can not find config attribute for type {type.Name}")
                : Config.GetSection(attribute.Name).Get<T>();
        }
    }
}
