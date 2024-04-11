using System.Data;
using Microsoft.Data.SqlClient;

namespace Infrastructure.DbConnection;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection Create() => new SqlConnection(connectionString);
}
