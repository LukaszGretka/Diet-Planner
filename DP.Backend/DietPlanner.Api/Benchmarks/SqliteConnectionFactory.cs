using Microsoft.Data.Sqlite;
using System.Data;
using System.Threading.Tasks;

namespace DietPlanner.Api.Benchmarks
{
    public class SqliteConnectionFactory
    {
        private string _connectionString;

        public SqliteConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        { 
            var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}