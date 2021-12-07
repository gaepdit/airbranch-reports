using Domain.Facilities.Models;
using FluentAssertions;
using Infrastructure.Facilities;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace InfrastructureTests.Facilities;

public class GetFacility
{
    private readonly IDbConnection db;

    public GetFacility()
    {
        var config = new ConfigurationBuilder().AddJsonFile("testsettings.json").Build();
        db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
    }

    [Test]
    public async Task ReturnsFacilityIfExists()
    {
        var facilityId = "001-00001";
        var repo = new FacilitiesRepository(db);
        var result = await repo.GetFacilityAsync(facilityId);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<Facility>();
            result.HasValue.Should().BeTrue();
            result.Value.Id.ToString().Should().Be(facilityId);
        });
    }

    [Test]
    public async Task ReturnsNullIfNotExists()
    {
        var repo = new FacilitiesRepository(db);
        var result = await repo.GetFacilityAsync("000-00000");

        result.Should().BeNull();
    }
}
