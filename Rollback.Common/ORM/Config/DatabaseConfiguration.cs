using System.Text.Json.Serialization;
using Rollback.Common.Config;

namespace Rollback.Common.ORM.Config
{
    [Config("Database")]
    public sealed class DatabaseConfiguration
    {
        public string Host { get; set; } = "127.0.0.1";

        public string User { get; set; } = "root";

        public string Password { get; set; } = string.Empty;

        public string DatabaseName { get; set; } = "rollback_X";

        [JsonIgnore]
        public string ConnectionString =>
            $"Server={Host};User ID={User};Pwd={Password};Database={DatabaseName};";
    }
}
