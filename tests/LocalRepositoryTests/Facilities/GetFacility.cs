using AwesomeAssertions;
using Domain.Facilities.Models;
using LocalRepository.Data;
using LocalRepository.Facilities;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.Facilities;

public class GetFacility
{
    [Test]
    public async Task ReturnsFacilityIfExists()
    {
        var facility = FacilityData.Facilities.First();

        var repo = new FacilitiesRepository();
        var result = await repo.GetFacilityAsync(facility.Id!);
        result.Should().BeEquivalentTo(facility);
    }

    [Test]
    public async Task ReturnsNullIfNotExists()
    {
        var repo = new FacilitiesRepository();
        var result = await repo.GetFacilityAsync(new ApbFacilityId(Constants.FacilityIdThatDoesNotExist));
        result.Should().BeNull();
    }
}
