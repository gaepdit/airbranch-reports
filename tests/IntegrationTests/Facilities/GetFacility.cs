using Domain.Facilities.Models;
using FluentAssertions;
using Infrastructure.Facilities;
using NUnit.Framework;
using System.Threading.Tasks;

namespace IntegrationTests.Facilities;

public class GetFacility
{
    [Test]
    public async Task ReturnsFacilityIfExists()
    {
        var facilityId = "001-00001";
        var repo = new FacilitiesRepository(Global.conn!);
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
        var repo = new FacilitiesRepository(Global.conn!);
        var result = await repo.GetFacilityAsync("000-00000");

        result.Should().BeNull();
    }
}
