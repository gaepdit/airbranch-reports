using Domain.Compliance.Models;
using Domain.Facilities.Models;
using FluentAssertions;
using Infrastructure.Compliance;
using Infrastructure.Facilities;
using NUnit.Framework;
using System.Threading.Tasks;

namespace IntegrationTests.Compliance;

public class GetFceReport
{
    [Test]
    public async Task ReturnsReportIfExists()
    {
        var facilityId = new ApbFacilityId("001-00001");
        const int id = 7136;
        var facilitiesRepo = new FacilitiesRepository(Global.DbConnectionFactory!);
        var repo = new ComplianceRepository(Global.DbConnectionFactory!, facilitiesRepo);
        var result = await repo.GetFceReportAsync(facilityId, id);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<FceReport>();
            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id!.Should().Be(facilityId);
        });
    }

    [Test]
    public async Task ReturnsNullIfNotExists()
    {
        var facilitiesRepo = new FacilitiesRepository(Global.DbConnectionFactory!);
        var repo = new ComplianceRepository(Global.DbConnectionFactory!, facilitiesRepo);
        var result = await repo.GetFceReportAsync("000-00000", 1);

        result.Should().BeNull();
    }
}
