using Domain.Facilities.Models;
using FluentAssertions;
using LocalRepository.Data;
using LocalRepository.Monitoring;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.Monitoring;

public class GetStackTestReport
{
    [Test]
    public async Task ReturnsReportIfExists()
    {
        var report = StackTestData.GetStackTestReports.First();
        report.ParseConfidentialParameters();

        var repo = new MonitoringRepository();
        var result = await repo.GetStackTestReportAsync(report.Facility!.Id!, report.ReferenceNumber);
        result.Should().BeEquivalentTo(report);
    }

    [Test]
    public async Task ReturnsNullIfNotExists()
    {
        var repo = new MonitoringRepository();
        var result = await repo.GetStackTestReportAsync(new ApbFacilityId(Constants.FacilityIdThatDoesNotExist), default);
        result.Should().BeNull();
    }
}
