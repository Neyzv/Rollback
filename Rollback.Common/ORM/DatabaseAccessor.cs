using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using MySqlConnector;
using Rollback.Common.Config;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Logging;
using Rollback.Common.ORM.Config;

namespace Rollback.Common.ORM
{
    public sealed class DatabaseAccessor : Singleton<DatabaseAccessor>
    {
        private const int _defaultTimeOut = 15;
        private readonly DatabaseConfiguration _configuration;
        private readonly Func<int, MySqlConnection> _connectionFactory;

        public DatabaseAccessor()
        {
            _configuration = ConfigManager.Instance.Get<DatabaseConfiguration>();
            _connectionFactory = (int timeOut) =>
            {
                var connection = new MySqlConnection($"{_configuration.ConnectionString}Connection Timeout={timeOut};");
                connection.Open();
                return connection;
            };
        }

        public DatabaseAccessor(DatabaseConfiguration configuration)
        {
            _configuration = configuration;
            _connectionFactory = (int timeOut) =>
            {
                var connection = new MySqlConnection($"{_configuration.ConnectionString}Connection Timeout={timeOut};");
                connection.Open();
                return connection;
            };
        }

        public bool TestConnection()
        {
            var result = true;
            try
            {
                _connectionFactory(_defaultTimeOut);
            }
            catch
            { result = false; }

            return result;
        }

        private static TableAttribute GetTableAttribute(Type type)
        {
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();

            return tableAttribute is null ?
                throw new Exception($"Can not find TableAttribute on class {type.Name}")
                : tableAttribute;
        }

        private static void ConvertMySQLDatasToObject<T>(MySqlDataReader reader, Type type, TableAttribute tableAttribute, ref T? result)
            where T : class
        {
            if (reader.Read())
            {
                Dictionary<string, object> datas = new();

                for (var i = 0; i < reader.FieldCount; i++)
                    datas[reader.GetName(i)] = reader.GetValue(i);

                var createFunc = Expression.Lambda<Func<T>>(Expression.New(type)).Compile();
                result = createFunc();

                foreach (var property in type.GetProperties().Where(x => x.GetCustomAttribute<IgnoreAttribute>() is null))
                {
                    if (datas.TryGetValue(property.Name, out var value))
                    {
                        try
                        {
                            if (value != DBNull.Value)
                                property.SetValue(result, value);
                        }
                        catch (Exception e)
                        {
                            Logger.Instance.LogError(e, $"Error wrong value type in database for property {property.Name} on table {tableAttribute.Name} :\n{e.Message}");
                        }
                    }
                    else
                        throw new Exception($"Could not find column {property.Name} in table {tableAttribute.Name}...");
                }
            }
            else
                result = default;
        }

        public bool ExecuteNonQuery(string queryString, int? timeOut = default)
        {
            try
            {
                using var connection = _connectionFactory(timeOut ?? _defaultTimeOut);
                using var command = connection.CreateCommand();

                command.CommandText = queryString;

                return command.ExecuteNonQuery() is not 0;
            }
            catch (Exception e)
            {
                Logger.Instance.LogError(e);
            }

            return default;
        }

        public bool Insert<T>(T data, int? timeOut = default)
            where T : class
        {
            var type = typeof(T);

            try
            {
                var tableAttribute = GetTableAttribute(type);

                var requestString = new StringBuilder($"INSERT INTO {tableAttribute.Name} ");

                Dictionary<string, object?> datas = new();
                foreach (var property in type.GetProperties().Where(x => x.GetCustomAttribute<IgnoreAttribute>() is null))
                    datas[property.Name] = property.GetValue(data);

                requestString.Append($"({string.Join(",", datas.Keys)}) VALUES ({string.Join(",", datas.Keys.Select(x => $"@{x}"))})");

                using var connection = _connectionFactory(timeOut ?? _defaultTimeOut);
                using var command = connection.CreateCommand();

                command.CommandText = requestString.ToString();

                foreach (var key in datas.Keys)
                    command.Parameters.AddWithValue($"@{key}", datas[key]);

                return command.ExecuteNonQuery() is not 0;
            }
            catch (Exception e)
            {
                Logger.Instance.LogError(e);
            }

            return default;
        }

        public bool Delete<T>(T data, int? timeOut = default)
            where T : class
        {
            var type = typeof(T);

            try
            {
                var tableAttribute = GetTableAttribute(type);

                var requestString = new StringBuilder($"DELETE FROM {tableAttribute.Name} WHERE ");

                Dictionary<string, object?> keys = new();
                foreach (var property in type.GetProperties().Where(x => x.GetCustomAttribute<KeyAttribute>() is not null))
                    keys[property.Name] = property.GetValue(data);

                requestString.Append($"{string.Join(" AND ", keys.Select(x => $"{x.Key} = @{x.Key}"))}");

                using var connection = _connectionFactory(timeOut is not null ? timeOut.Value : _defaultTimeOut);
                using var command = connection.CreateCommand();

                command.CommandText = requestString.ToString();

                foreach (var key in keys.Keys)
                    command.Parameters.AddWithValue($"@{key}", keys[key]);

                return command.ExecuteNonQuery() is not 0;
            }
            catch (Exception e)
            {
                Logger.Instance.LogError(e);
            }

            return false;
        }

        public bool Update<T>(T data, int? timeOut = default)
            where T : class
        {
            var type = typeof(T);

            try
            {
                var tableAttribute = GetTableAttribute(type);

                var requestString = new StringBuilder($"UPDATE {tableAttribute.Name} SET ");

                Dictionary<string, object?> keys = new();
                Dictionary<string, object?> datas = new();
                foreach (var property in type.GetProperties().Where(x => x.GetCustomAttribute<IgnoreAttribute>() is null))
                {
                    var value = property.GetValue(data);

                    if (property.GetCustomAttribute<KeyAttribute>() is null)
                        datas[property.Name] = value;
                    else
                        keys[property.Name] = value;
                }

                requestString.Append($"{string.Join(",", datas.Select(x => $"{x.Key} = @{x.Key}"))} WHERE {string.Join(" AND ", keys.Select(x => $"{x.Key} = @{x.Key}"))}");

                using var connection = _connectionFactory(timeOut ?? _defaultTimeOut);
                using var command = connection.CreateCommand();

                command.CommandText = requestString.ToString();

                // Binding datas
                foreach (var key in datas.Keys)
                    command.Parameters.AddWithValue($"@{key}", datas[key]);

                // Binding keys
                foreach (var key in keys.Keys)
                    command.Parameters.AddWithValue($"@{key}", keys[key]);

                return command.ExecuteNonQuery() is not 0;
            }
            catch (Exception e)
            {
                Logger.Instance.LogError(e);
            }

            return false;
        }

        public bool IsInDatabase<T>(T data, int? timeOut = default)
        {
            var type = typeof(T);

            try
            {
                var tableAttribute = GetTableAttribute(type);
                var queryString = new StringBuilder($"SELECT 1 FROM {tableAttribute.Name} WHERE ");

                Dictionary<string, object?> keys = new();
                foreach (var property in type.GetProperties().Where(x => x.GetCustomAttribute<KeyAttribute>() is not null))
                    keys[property.Name] = property.GetValue(data);

                queryString.Append(string.Join(" AND ", keys.Select(x => $"{x.Key} = @{x.Key}")));

                using var connection = _connectionFactory(timeOut ?? _defaultTimeOut);
                using var command = connection.CreateCommand();

                command.CommandText = queryString.ToString();

                // Binding keys
                foreach (var key in keys.Keys)
                    command.Parameters.AddWithValue($"@{key}", keys[key]);

                return command.ExecuteReader().Read();
            }
            catch (Exception e)
            {
                Logger.Instance.LogError(e);
            }

            return false;
        }

        public bool InsertOrUpdate<T>(T data, int? timeOut = default)
            where T : class =>
            IsInDatabase(data, timeOut) ? Update(data, timeOut) : Insert(data, timeOut);

        public List<Dictionary<string, object?>> Select(string queryString, int? timeOut = default)
        {
            List<Dictionary<string, object?>> result = new();

            try
            {
                using var connection = _connectionFactory(timeOut ?? _defaultTimeOut);
                using var command = connection.CreateCommand();

                command.CommandText = queryString;

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var datas = new Dictionary<string, object?>();

                    for (var i = 0; i < reader.FieldCount; i++)
                        datas[reader.GetName(i)] = reader[i] != DBNull.Value ? reader[i] : null;

                    result.Add(datas);
                }
            }
            catch (Exception e)
            {
                Logger.Instance.LogError(e);
            }

            return result;
        }

        public T? SelectSingle<T>(string queryString, int? timeOut = default)
            where T : class
        {
            var result = default(T?);
            var type = typeof(T);

            try
            {
                var tableAttribute = GetTableAttribute(type);

                using var connection = _connectionFactory(timeOut ?? _defaultTimeOut);
                using var command = connection.CreateCommand();

                command.CommandText = queryString;

                using var reader = command.ExecuteReader();

                ConvertMySQLDatasToObject(reader, type, tableAttribute, ref result);
            }
            catch (Exception e)
            {
                Logger.Instance.LogError(e);
            }

            return result;
        }

        public Dictionary<string, object?>? SelectSingle(string queryString, int? timeOut = default)
        {
            Dictionary<string, object?>? result = default;

            try
            {
                using var connection = _connectionFactory(timeOut ?? _defaultTimeOut);
                using var command = connection.CreateCommand();

                command.CommandText = queryString;

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var datas = new Dictionary<string, object?>();

                    for (var i = 0; i < reader.FieldCount; i++)
                        datas[reader.GetName(i)] = reader[i] != DBNull.Value ? reader[i] : null;

                    result = datas;
                }
            }
            catch (Exception e)
            {
                Logger.Instance.LogError(e);
            }

            return result;
        }

        public List<T> Select<T>(string queryString, int? timeOut = default)
            where T : class
        {
            var result = new List<T>();
            var type = typeof(T);

            try
            {
                var tableAttribute = GetTableAttribute(type);

                using var connection = _connectionFactory(timeOut ?? _defaultTimeOut);
                using var command = connection.CreateCommand();

                command.CommandText = queryString;

                using var reader = command.ExecuteReader();

                var instance = default(T?);
                do
                {
                    ConvertMySQLDatasToObject(reader, type, tableAttribute, ref instance);
                    if (instance is not null)
                        result.Add(instance);
                }
                while (instance is not null);
            }
            catch (Exception e)
            {
                Logger.Instance.LogError(e);
            }

            return result;
        }
    }
}
