using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Data;
using System.Data.SqlClient;

namespace InfrastructureTests;

[SetUpFixture]
public class Global
{
    internal static IDbConnection? db;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var config = new ConfigurationBuilder().AddJsonFile("testsettings.json").Build();
        db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => db?.Dispose();
}
