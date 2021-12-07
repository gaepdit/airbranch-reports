using Microsoft.Extensions.Configuration;
using FluentAssertions;
using Infrastructure.Facilities;
using NUnit.Framework;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace InfrastructureTests.Facilities;

public class FacilityExists
{
    private readonly IDbConnection db;

    public FacilityExists()
    {
        var config = new ConfigurationBuilder().AddJsonFile("testsettings.json").Build();
        db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
    }

    [Test]
    public async Task ReturnsTrueIfExists()
    {
        var repo = new FacilitiesRepository(db);
        var result = await repo.FacilityExistsAsync("001-00001");
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var repo = new FacilitiesRepository(db);
        var result = await repo.FacilityExistsAsync("000-00000");
        result.Should().BeFalse();
    }
}