using System.Data;
using Microsoft.Data.SqlClient;

namespace Infrastructure.DbConnection;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    public DbConnectionFactory(string connectionString) => _connectionString = connectionString;
    public IDbConnection Create() => new SqlConnection(_connectionString);
}
