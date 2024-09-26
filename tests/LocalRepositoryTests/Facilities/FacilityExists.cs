using Domain.Facilities.Models;
using FluentAssertions;
using LocalRepository.Data;
using LocalRepository.Facilities;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.Facilities;

public class FacilityExists
{
    [Test]
    public async Task ReturnsTrueIfExists()
    {
        var facility = FacilityData.Facilities.First();

        var repo = new FacilitiesRepository();
        var result = await repo.FacilityExistsAsync(facility.Id!);
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var repo = new FacilitiesRepository();
        var result = await repo.FacilityExistsAsync(new FacilityId(Constants.FacilityIdThatDoesNotExist));
        result.Should().BeFalse();
    }
}
