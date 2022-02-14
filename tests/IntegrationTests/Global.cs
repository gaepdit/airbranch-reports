using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Data;
using System.Data.SqlClient;

namespace IntegrationTests;

[SetUpFixture]
public class Global
{
    internal static IDbConnection? DbConn;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var config = new ConfigurationBuilder().AddJsonFile("testsettings.json").Build();
        DbConn = new SqlConnection(config.GetConnectionString("DefaultConnection"));
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => DbConn?.Dispose();
}
