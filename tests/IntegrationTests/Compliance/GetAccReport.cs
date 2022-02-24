using Domain.Compliance.Models;
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
        const string facilityId = "193-00008";
        const int year = 2019;
        var repo = new ComplianceRepository(Global.DbConn!);
        var result = await repo.GetAccReportAsync(facilityId, year);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<AccReport>();
            result.Should().NotBeNull();
            result!.AccReportingYear.Should().Be(year);
            result.Facility.Should().NotBeNull();
            result.Facility!.Id!.ToString().Should().Be(facilityId);
        });
    }

    [Test]
    public async Task ReturnsNullIfNotExists()
    {
        var repo = new ComplianceRepository(Global.DbConn!);
        var result = await repo.GetAccReportAsync("000-00000", 2019);

        result.Should().BeNull();
    }
}
