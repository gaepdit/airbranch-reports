using Domain.Compliance.Models;
using FluentAssertions;
using Infrastructure.Compliance;
using NUnit.Framework;
using System.Threading.Tasks;

namespace IntegrationTests.Compliance;

public class GetFceReport
{
    [Test]
    public async Task ReturnsReportIfExists()
    {
        const string facilityId = "001-00001";
        const int id = 7136;
        var repo = new ComplianceRepository(Global.DbConn!);
        var result = await repo.GetFceReportAsync(facilityId, id);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<FceReport>();
            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id!.ToString().Should().Be(facilityId);
        });
    }

    [Test]
    public async Task ReturnsNullIfNotExists()
    {
        var repo = new ComplianceRepository(Global.DbConn!);
        var result = await repo.GetFceReportAsync("000-00000", 1);

        result.Should().BeNull();
    }
}
