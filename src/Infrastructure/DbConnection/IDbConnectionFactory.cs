using System.Data;

namespace Infrastructure.DbConnection;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}
