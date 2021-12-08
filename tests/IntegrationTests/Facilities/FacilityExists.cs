using FluentAssertions;
using Infrastructure.Facilities;
using NUnit.Framework;
using System.Threading.Tasks;

namespace IntegrationTests.Facilities;

public class FacilityExists
{
    [Test]
    public async Task ReturnsTrueIfExists()
    {
        var repo = new FacilitiesRepository(Global.db!);
        var result = await repo.FacilityExistsAsync("001-00001");
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        var repo = new FacilitiesRepository(Global.db!);
        var result = await repo.FacilityExistsAsync("000-00000");
        result.Should().BeFalse();
    }
}