using FluentAssertions;
using LocalRepository.Compliance;
using LocalRepository.Data;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.Compliance;

public class GetAccReport
{
    [Test]
    public async Task ReturnsReportIfExists()
    {
        var report = AccData.GetAccReports.First();

        var repo = new ComplianceRepository();
        var result = await repo.GetAccReportAsync(report.Facility.Id, report.AccReportingYear);
        result.Should().BeEquivalentTo(report);
    }

    [Test]
    public async Task ReturnsNullIfNotExists()
    {
        var repo = new ComplianceRepository();
        var result = await repo.GetAccReportAsync(default, default);
        result.Should().BeNull();
    }
}
