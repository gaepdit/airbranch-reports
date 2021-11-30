using FluentAssertions;
using LocalRepository.Compliance;
using LocalRepository.Data;
using LocalRepository.Facilities;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace InfrastructureTests.LocalRepository.Compliance;

public class GetFacility
{
    [Test]
    public async Task ReturnsFacilityIfExists()
    {
        var facility = FacilityData.GetFacilities.First();

        var repo = new FacilitiesRepository();
        var result = await repo.GetFacilityAsync(facility.Id);
        result.Should().BeEquivalentTo(facility);
    }

    [Test]
    public async Task ReturnsNullIfNotExists()
    {
        var repo = new FacilitiesRepository();
        var result = await repo.GetFacilityAsync(default);
        result.Should().BeNull();
    }
}
