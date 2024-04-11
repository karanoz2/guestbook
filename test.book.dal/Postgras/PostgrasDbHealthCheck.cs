using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using test.book.dal.health;
using test.book.DAL.Postgras;

namespace test.book.dal.Postgras
{
    public class PostgrasDbHealthCheck: IDbHealthCheck
    {
        private readonly IOptions<PostgreSqlOptions> _dbSettings;
        private readonly ILogger<PostgrasDbHealthCheck> _log;
        public PostgrasDbHealthCheck(IOptions<PostgreSqlOptions> dbSettings, ILogger<PostgrasDbHealthCheck> log) {
            _dbSettings = dbSettings;
            _log = log;
        }

        public async Task<(bool state, Exception? ex)> IsHealthyAsync()
        {
            try
            {
                var connectionString = $"Host={_dbSettings.Value.Server}; Database={_dbSettings.Value.Database}; Username={_dbSettings.Value.User}; Password={_dbSettings.Value.Password};";
                var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    _log.LogError($"HealthCheck of DB: bad connection status '{Enum.GetName(typeof(ConnectionState), connection.State)}'");
                    return (false, null);
                }
                await connection.CloseAsync();
                return (true, null);
            } catch(Exception ex) {
                _log.LogError(ex, "HealthCheck of DB  error:");
                return (false, ex);
            }   
        }
    }
}
