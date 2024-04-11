using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using test.book.DAL.Entities;
using static Dapper.SqlMapper;

namespace test.book.DAL.Postgras
{
    internal class DataContextOfPostgrass : IDatabaseInit, IMessageManager
    {
        private readonly PostgreSqlOptions _dbSettings;
        private readonly ILogger<DataContextOfPostgrass> _log;
        public DataContextOfPostgrass(IOptions<PostgreSqlOptions> dbSettings, ILogger<DataContextOfPostgrass> log)
        {
            _dbSettings = dbSettings.Value;
            _log = log;
        }

        public async Task Init()
        {
            await _initDatabase();
            await _initMessagesTable();
        }


        public async Task<int> GetCount()
        {
            using var connection = CreateConnection();
            var sql = @$"select count(*)
                        from messages";
            return await connection.QuerySingleAsync<int>(sql);
        }

        public async Task<IEnumerable<Message>> GetMessages(int perpage, int page)
        {
            using var connection = CreateConnection();
            var sql = @$"select 
	                        id,
	                        ""text"",
	                        participant,
                            created
                        from messages 
                        order by created
                        limit @count offset @skip ";
            return await connection.QueryAsync<Message>(sql, new { count = perpage, skip = page * perpage });
        }

        public async Task<int> AddMessage(Message mess)
        {
            using var connection = CreateConnection();
            var sql = $@"insert into messages  
                                    (""text"" , participant, created) 
                                    values (@text, @participant, @created);
                        select currval(pg_get_serial_sequence('messages','id'));";

            return await connection.ExecuteScalarAsync<int>(sql, new { text = mess.Text, participant = mess.Participant, created = DateTime.UtcNow });
        }


        #region private
        private IDbConnection CreateConnection()
        {
            var connectionString = $"Host={_dbSettings.Server}; Database={_dbSettings.Database}; Username={_dbSettings.User}; Password={_dbSettings.Password};";
            return new NpgsqlConnection(connectionString);
        }
        private IDbConnection CreateServerConnection()
        {
            var connectionString = $"Host={_dbSettings.Server}; Username={_dbSettings.User}; Password={_dbSettings.Password};";
            return new NpgsqlConnection(connectionString);
        }

        private async Task _initDatabase()
        {
            using var connection = CreateServerConnection();
            var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_dbSettings.Database}';";
            _log.LogInformation(sqlDbCount);
            var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);
            if (dbCount == 0)
            {
                var sql = $"CREATE DATABASE \"{_dbSettings.Database}\"";
                await connection.ExecuteAsync(sql);
            }
        }

        private async Task _initMessagesTable()
        {
            using var connection = CreateConnection();

            var sqlCheck = @"SELECT EXISTS (
                                    SELECT FROM information_schema.tables  
                                    WHERE  table_schema = 'public' AND table_name   = 'messages');";

            var dbCount = await connection.ExecuteScalarAsync<bool>(sqlCheck);
            if (!dbCount)
            {
                var sql = """
                CREATE TABLE messages (
            	    Id serial NOT NULL,
            	    Text varchar NOT NULL,
            	    Participant varchar NOT NULL,
                    Created DATE,
            	    CONSTRAINT newtable_pk PRIMARY KEY (id)
                );
            """;
                await connection.ExecuteAsync(sql);
            }
        }
        #endregion
    }
}
