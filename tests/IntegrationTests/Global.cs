using Infrastructure.DbConnection;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace IntegrationTests;

[SetUpFixture]
public class Global
{
    internal static IDbConnectionFactory? DbConnectionFactory;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var config = new ConfigurationBuilder().AddJsonFile("testsettings.json").Build();
        DbConnectionFactory = new DbConnectionFactory(config.GetConnectionString("DefaultConnection"));
    }
}
