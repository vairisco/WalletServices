using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace WalletService.Infrastructure.Dapper.Persistence
{
    public class WalletDapperContext
    {
        private readonly IConfiguration _configuration;

        public WalletDapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection(string _connectionString) => new SqlConnection(_connectionString);
    }
}
