using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Synchro.Infrastructure.Data
{
    public interface IDapperConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class DapperConnectionFactory : IDapperConnectionFactory
    {
        private readonly string _connectionString;
        public DapperConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
