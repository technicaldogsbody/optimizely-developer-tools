using System.Data;
using Microsoft.Data.SqlClient;
using TechnicalDogsbody.Optimizely.DeveloperTools.Contracts;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Factories
{
    public class SqlServerConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlServerConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
