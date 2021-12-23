﻿using System.Data;
using Microsoft.Data.SqlClient;
using TechnicalDogsbody.Optimizely.DeveloperTools.Core.Contracts;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Factories
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
