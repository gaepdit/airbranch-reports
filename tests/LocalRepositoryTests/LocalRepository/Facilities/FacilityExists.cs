using FluentAssertions;
using LocalRepository.Data;
using LocalRepository.Facilities;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace InfrastructureTests.LocalRepository.Compliance;

public class FacilityExists
{
    [Test]
    public async Task ReturnsTrueIfExists()
    {
        var facility = FacilityData.GetFacilities.First();

        var repo = new FacilitiesRepository();
        var result = await repo.FacilityExistsAsync(facility.Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var repo = new FacilitiesRepository();
        var result = await repo.FacilityExistsAsync(default);
        result.Should().BeFalse();
    }
}