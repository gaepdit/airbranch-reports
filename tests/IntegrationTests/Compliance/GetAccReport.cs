using Domain.Compliance.Models;
using Domain.Facilities.Models;
using FluentAssertions;
using Infrastructure.Compliance;
using NUnit.Framework;
using System.Threading.Tasks;

namespace IntegrationTests.Compliance;

public class GetAccReport
{
    [Test]
    public async Task ReturnsReportIfExists()
    {
        var facilityId = new ApbFacilityId("05100149");
        const int id = 77863;
        var repo = new ComplianceRepository(Global.DbConnectionFactory!);
        var result = await repo.GetAccReportAsync(facilityId, id);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<AccReport>();
            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id!.Should().Be(facilityId);
        });
    }

    [Test]
    public async Task ReturnsNullIfNotExists()
    {
        var repo = new ComplianceRepository(Global.DbConnectionFactory!);
        var result = await repo.GetAccReportAsync("000-00000", 1);

        result.Should().BeNull();
    }
}
