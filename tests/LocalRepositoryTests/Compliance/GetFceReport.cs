using Domain.Facilities.Models;
using FluentAssertions;
using LocalRepository.Compliance;
using LocalRepository.Data;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.Compliance;

public class GetFceReport
{
    [Test]
    public async Task ReturnsReportIfExists()
    {
        var report = FceData.FceReports.First();

        var repo = new ComplianceRepository();
        var result = await repo.GetFceReportAsync(report.Facility!.Id!, report.Id);
        result.Should().BeEquivalentTo(report);
    }

    [Test]
    public async Task ReturnsNullIfNotExists()
    {
        var repo = new ComplianceRepository();
        var result = await repo.GetFceReportAsync(new ApbFacilityId(Constants.FacilityIdThatDoesNotExist), default);
        result.Should().BeNull();
    }
}
